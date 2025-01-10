namespace MpvLib;

public partial class Player : MpvClient
{
    public AutoResetEvent ShutdownAutoResetEvent { get; } = new AutoResetEvent(false);

    public nint MainHandle { get; private set; }

    public List<MediaTrack> MediaTracks { get; private set; } = [];

    public List<TimeSpan> BluRayTitles { get; } = [];

    public object MediaTracksLock { get; } = new object();

    public List<MpvClient> Clients { get; } = [];


    private System.Drawing.Size _videoSize;

    private DateTime _lastLoad;
    private bool _wasAviSynthLoaded;
    private static readonly object _loadFolderLockObject = new();

    public void Init(IntPtr formHandle)
    {
        Handle = MainHandle = mpv_create();

        var events = Enum.GetValues(typeof(mpv_event_id)).Cast<mpv_event_id>();

        foreach (mpv_event_id i in events)
            mpv_request_event(MainHandle, i, 0);

        mpv_request_log_messages(MainHandle, "no");

        if (formHandle != IntPtr.Zero)
            Task.Factory.StartNew(() => MainEventLoop(), TaskCreationOptions.LongRunning);

        if (MainHandle == IntPtr.Zero)
            throw new Exception("error mpv_create");

        if (formHandle != IntPtr.Zero)
            SetPropertyLong("wid", formHandle.ToInt64());

        SetPropertyString("hwdec", "auto");
        SetPropertyString("vo", "gpu");//视频输出驱动 
        SetPropertyString("vd-lavc-dr", "auto");//是否直接解码到显存，个别低端英特尔处理器可能需要显式禁用此功能以大幅提速解码


        SetPropertyString("title", "南瓜播放器");
        SetPropertyInt("volume-max", 100);
        SetPropertyInt("volume", 50);
        SetPropertyString("cache", "yes");
        SetPropertyString("keep-open", "yes");//可保持最后一帧, 和播放完毕后继续重新播放

        SetPropertyString("input-builtin-bindings", "no");
        SetPropertyString("idle", "yes");
        SetPropertyString("script-opts-append", "yes");
        SetPropertyString("osc", "no");
        SetPropertyString("force-window", "no");//即使没有视频也要创建一个视频输出窗口

        SetPropertyString("sub-auto", "fuzzy");//加载包含媒体文件名的所有字幕

        SetPropertyInt("sub-scale", 1);
        SetPropertyInt("sub-pos", 100);
        SetPropertyString("sub-scale-by-window", "yes");

        //SetProperty("loop-playlist", tag);
        //SetPropertyString("video-rotate", "270");

        SetPropertyString("log-file", $"{AppContext.BaseDirectory}logs/mpv_{DateTime.Today:yyyy-MM-dd}.log");//log

        SetPropertyString("config-dir", $"{AppContext.BaseDirectory}portable_config");
        SetPropertyString("config", "yes");

        SetPropertyString("load-scripts", "yes");

        mpv_error err = mpv_initialize(MainHandle);



        if (err < 0)
            throw new Exception("mpv_initialize error" + "\n\n" + GetError(err) + "\n");

        string idle = GetPropertyString("idle");
        //App.Exit = idle == "no" || idle == "once";

        Handle = mpv_create_client(MainHandle, "mpv");

        if (Handle == IntPtr.Zero)
            throw new Exception("mpv_create_client error");

        mpv_request_log_messages(Handle, "info");

        if (formHandle != IntPtr.Zero) Util.Run(EventLoop);

        // otherwise shutdown is raised before media files are loaded,
        // this means Lua scripts that use idle might not work correctly

        //SetPropertyString("user-data/frontend/name", "mpv.net");
        //SetPropertyString("user-data/frontend/version", AppInfo.Version.ToString());
        //SetPropertyString("user-data/frontend/process-path", Environment.ProcessPath!);

        SubscribeEvents();

        InitializedEvent?.Invoke();
    }

    public void MainEventLoop()
    {
        while (true)
            mpv_wait_event(MainHandle, -1);
    }



    void UpdateVideoSize(string w, string h)
    {
        if (string.IsNullOrEmpty(Path))
            return;

        var size = new System.Drawing.Size(GetPropertyInt(w), GetPropertyInt(h));

        if (VideoRotate == 90 || VideoRotate == 270)
            size = new System.Drawing.Size(size.Height, size.Width);

        if (size != _videoSize && size != System.Drawing.Size.Empty)
        {
            _videoSize = size;
            VideoSizeChangedEvent?.Invoke(size);
        }
    }

  
    protected override void OnShutdown()
    {
        //IsQuitNeeded = false;
        base.OnShutdown();
        ShutdownAutoResetEvent.Set();
    }

    protected override void OnLogMessage(mpv_event_log_message data)
    {
        if (data.log_level == mpv_log_level.MPV_LOG_LEVEL_INFO)
        {
            string prefix = ConvertFromUtf8(data.prefix);

            if (prefix == "bd")
                ProcessBluRayLogMessage(ConvertFromUtf8(data.text));
        }

        base.OnLogMessage(data);
    }



    protected override void OnEndFile(mpv_event_end_file data)
    {
        base.OnEndFile(data);
    }

    protected override void OnVideoReconfig()
    {
        UpdateVideoSize("dwidth", "dheight");
        base.OnVideoReconfig();
    }

    // executed before OnFileLoaded
    protected override void OnStartFile()
    {
        base.OnStartFile();
        Util.Run(LoadFolder);
    }

    // executed after OnStartFile
    protected override void OnFileLoaded()
    {
        Util.Run(UpdateTracks);

        base.OnFileLoaded();
    }

    void ProcessBluRayLogMessage(string msg)
    {
        lock (BluRayTitles)
        {
            if (msg.Contains(" 0 duration: "))
                BluRayTitles.Clear();

            if (msg.Contains(" duration: "))
            {
                int start = msg.IndexOf(" duration: ") + 11;
                BluRayTitles.Add(new TimeSpan(
                    msg.Substring(start, 2).ToInt(),
                    msg.Substring(start + 3, 2).ToInt(),
                    msg.Substring(start + 6, 2).ToInt()));
            }
        }
    }

    public void SetBluRayTitle(int id) => LoadFiles([@"bd://" + id], false);


    public void LoadFiles(string[]? files, bool append)
    {
        if (files == null || files.Length == 0)
            return;

        if ((DateTime.Now - _lastLoad).TotalMilliseconds < 1000)
            append = true;

        _lastLoad = DateTime.Now;

        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i];

            if (string.IsNullOrEmpty(file))
                continue;

            if (file.Contains('|'))
                file = file[..file.IndexOf("|")];

            file = ConvertFilePath(file);

            string ext = file.Ext();

            if (OperatingSystem.IsWindows())
            {
                switch (ext)
                {
                    case "avs": LoadAviSynth(); break;
                    case "lnk": file = Util.GetShortcutTarget(file); break;
                }
            }

            if (ext == "iso")
                LoadBluRayISO(file);
            else if (FileTypes.Subtitle.Contains(ext))
                CommandV("sub-add", file);
            else if (!FileTypes.IsMedia(ext) && !file.Contains("://") && Directory.Exists(file) &&
                File.Exists(System.IO.Path.Combine(file, "BDMV\\index.bdmv")))
            {
                Command("stop");
                Thread.Sleep(500);
                SetPropertyString("bluray-device", file);
                CommandV("loadfile", @"bd://");
            }
            else
            {
                if (i == 0 && !append)
                    CommandV("loadfile", file);
                else
                    CommandV("loadfile", file, "append");
            }
        }

        if (string.IsNullOrEmpty(GetPropertyString("path")))
            SetPropertyInt("playlist-pos", 0);
    }

    public static string ConvertFilePath(string path)
    {
        if ((path.Contains(":/") && !path.Contains("://")) || (path.Contains(":\\") && path.Contains('/')))
            path = path.Replace("/", "\\");

        if (!path.Contains(':') && !path.StartsWith("\\\\") && File.Exists(path))
            path = System.IO.Path.GetFullPath(path);

        return path;
    }

    public void LoadBluRayISO(string path)
    {
        Command("stop");
        Thread.Sleep(500);
        SetPropertyString("bluray-device", path);
        LoadFiles(new[] { @"bd://" }, false);
    }

    public void LoadDiskFolder(string path)
    {
        Command("stop");
        Thread.Sleep(500);

        if (Directory.Exists(path + "\\BDMV"))
        {
            SetPropertyString("bluray-device", path);
            LoadFiles(new[] { @"bd://" }, false);
        }
        else
        {
            SetPropertyString("dvd-device", path);
            LoadFiles(new[] { @"dvd://" }, false);
        }
    }


    public void LoadFolder()
    {
        //if (!App.AutoLoadFolder)
        //    return;


        return;

        Thread.Sleep(1000);

        lock (_loadFolderLockObject)
        {
            string path = GetPropertyString("path");

            if (!File.Exists(path) || GetPropertyInt("playlist-count") != 1)
                return;

            string dir = Environment.CurrentDirectory;

            if (path.Contains(":/") && !path.Contains("://"))
                path = path.Replace("/", "\\");

            if (path.Contains('\\'))
                dir = System.IO.Path.GetDirectoryName(path)!;

            List<string> files = FileTypes.GetMediaFiles(Directory.GetFiles(dir)).ToList();

            //if (OperatingSystem.IsWindows())
            //    files.Sort(new StringLogicalComparer());

            int index = files.IndexOf(path);
            files.Remove(path);

            foreach (string file in files)
                CommandV("loadfile", file, "append");

            if (index > 0)
                CommandV("playlist-move", "0", (index + 1).ToString());
        }
    }


    void LoadAviSynth()
    {
        if (!_wasAviSynthLoaded)
        {
            string? dll = Environment.GetEnvironmentVariable("AviSynthDLL");  // StaxRip sets it in portable mode
            Util.LoadLibrary(File.Exists(dll) ? dll : "AviSynth.dll");
            _wasAviSynthLoaded = true;
        }
    }


    //public static string GetShortcutTarget(string path)
    //{
    //    Type? t = Type.GetTypeFromProgID("WScript.Shell");
    //    dynamic? sh = Activator.CreateInstance(t!);
    //    return sh?.CreateShortcut(path).TargetPath!;
    //}

    static string GetLanguage(string id)
    {
        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            if (ci.ThreeLetterISOLanguageName == id || Convert(ci.ThreeLetterISOLanguageName) == id)
                return ci.EnglishName;

        return id;

        static string Convert(string id2) => id2 switch
        {
            "bng" => "ben",
            "ces" => "cze",
            "deu" => "ger",
            "ell" => "gre",
            "eus" => "baq",
            "fra" => "fre",
            "hye" => "arm",
            "isl" => "ice",
            "kat" => "geo",
            "mya" => "bur",
            "nld" => "dut",
            "sqi" => "alb",
            "zho" => "chi",
            _ => id2,
        };
    }

    static string GetNativeLanguage(string name)
    {
        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            if (ci.EnglishName == name)
                return ci.NativeName;

        return name;
    }

    public void UpdateTracks()
    {
        string path = GetPropertyString("path");

        if (!path.ToLowerEx().StartsWithEx("bd://"))
            lock (BluRayTitles)
                BluRayTitles.Clear();

        lock (MediaTracksLock)
        {
            if (/*App.MediaInfo*/ true && !path.Contains("://") && !path.Contains(@"\\.\pipe\") && File.Exists(path))
                MediaTracks = GetMediaInfoTracks(path);
            else
                MediaTracks = GetTracks();
        }
    }

    public List<(string, string)> AudioDevices
    {
        get
        {
            List<(string, string)> list = [];
            string json = GetPropertyString("audio-device-list");
            var enumerator = System.Text.Json.JsonDocument.Parse(json).RootElement.EnumerateArray();

            foreach (var element in enumerator)
            {
                string name = element.GetProperty("name").GetString()!;
                string description = element.GetProperty("description").GetString()!;
                list.Add((name, description));
            }

            return list;
        }
    }

    public List<Chapter> GetChapters()
    {
        var chapters = new List<Chapter>();
        int count = GetPropertyInt("chapter-list/count");

        for (int x = 0; x < count; x++)
        {
            string title = GetPropertyString($"chapter-list/{x}/title");
            double time = GetPropertyDouble($"chapter-list/{x}/time");

            if (string.IsNullOrEmpty(title) ||
                (title.Length == 12 && title.Contains(':') && title.Contains('.')))

                title = "Chapter " + (x + 1);

            chapters.Add(new Chapter() { Title = title, Time = time });
        }

        return chapters;
    }

    public void UpdateExternalTracks()
    {
        int trackListTrackCount = GetPropertyInt("track-list/count");
        int editionCount = GetPropertyInt("edition-list/count");
        int count = MediaTracks.Where(i => i.Type != "g").Count();

        lock (MediaTracksLock)
        {
            if (count != (trackListTrackCount + editionCount))
            {
                MediaTracks = MediaTracks.Where(i => !i.External).ToList();
                MediaTracks.AddRange(GetTracks(false));
            }
        }
    }

    public List<MediaTrack> GetTracks(bool includeInternal = true, bool includeExternal = true)
    {
        var tracks = new List<MediaTrack>();

        int trackCount = GetPropertyInt("track-list/count");

        for (int i = 0; i < trackCount; i++)
        {
            bool external = GetPropertyBool($"track-list/{i}/external");

            if ((external && !includeExternal) || (!external && !includeInternal))
                continue;

            string type = GetPropertyString($"track-list/{i}/type");
            string filename = GetPropertyString($"filename/no-ext");
            string title = GetPropertyString($"track-list/{i}/title").Replace(filename, "");

            title = Regex.Replace(title, @"^[\._\-]", "");

            if (type == "video")
            {
                string codec = GetPropertyString($"track-list/{i}/codec").ToUpperEx();
                if (codec == "MPEG2VIDEO")
                    codec = "MPEG2";
                else if (codec == "DVVIDEO")
                    codec = "DV";
                MediaTrack track = new MediaTrack();
                Add(track, codec);
                Add(track, GetPropertyString($"track-list/{i}/demux-w") + "x" + GetPropertyString($"track-list/{i}/demux-h"));
                Add(track, GetPropertyString($"track-list/{i}/demux-fps").Replace(".000000", "") + " FPS");
                Add(track, GetPropertyBool($"track-list/{i}/default") ? "Default" : null);
                track.Text = "V: " + track.Text.Trim(' ', ',');
                track.Type = "v";
                track.ID = GetPropertyInt($"track-list/{i}/id");
                tracks.Add(track);
            }
            else if (type == "audio")
            {
                string codec = GetPropertyString($"track-list/{i}/codec").ToUpperEx();
                if (codec.Contains("PCM"))
                    codec = "PCM";
                MediaTrack track = new MediaTrack();
                Add(track, GetLanguage(GetPropertyString($"track-list/{i}/lang")));
                Add(track, codec);
                Add(track, GetPropertyInt($"track-list/{i}/audio-channels") + " ch");
                Add(track, GetPropertyInt($"track-list/{i}/demux-samplerate") / 1000 + " kHz");
                Add(track, GetPropertyBool($"track-list/{i}/forced") ? "Forced" : null);
                Add(track, GetPropertyBool($"track-list/{i}/default") ? "Default" : null);
                Add(track, GetPropertyBool($"track-list/{i}/external") ? "External" : null);
                Add(track, title);
                track.Text = "A: " + track.Text.Trim(' ', ',');
                track.Type = "a";
                track.ID = GetPropertyInt($"track-list/{i}/id");
                track.External = external;
                tracks.Add(track);
            }
            else if (type == "sub")
            {
                string codec = GetPropertyString($"track-list/{i}/codec").ToUpperEx();
                if (codec.Contains("PGS"))
                    codec = "PGS";
                else if (codec == "SUBRIP")
                    codec = "SRT";
                else if (codec == "WEBVTT")
                    codec = "VTT";
                else if (codec == "DVB_SUBTITLE")
                    codec = "DVB";
                else if (codec == "DVD_SUBTITLE")
                    codec = "VOB";
                MediaTrack track = new MediaTrack();
                Add(track, GetLanguage(GetPropertyString($"track-list/{i}/lang")));
                Add(track, codec);
                Add(track, GetPropertyBool($"track-list/{i}/forced") ? "Forced" : null);
                Add(track, GetPropertyBool($"track-list/{i}/default") ? "Default" : null);
                Add(track, GetPropertyBool($"track-list/{i}/external") ? "External" : null);
                Add(track, title);
                track.Text = "S: " + track.Text.Trim(' ', ',');
                track.Type = "s";
                track.ID = GetPropertyInt($"track-list/{i}/id");
                track.External = external;
                tracks.Add(track);
            }
        }

        if (includeInternal)
        {
            int editionCount = GetPropertyInt("edition-list/count");

            for (int i = 0; i < editionCount; i++)
            {
                string title = GetPropertyString($"edition-list/{i}/title");

                if (string.IsNullOrEmpty(title))
                    title = "Edition " + i;

                MediaTrack track = new MediaTrack
                {
                    Text = "E: " + title,
                    Type = "e",
                    ID = i
                };

                tracks.Add(track);
            }
        }

        return tracks;

        static void Add(MediaTrack track, object? value)
        {
            string str = (value + "").Trim();

            if (str != "" && !track.Text.Contains(str))
                track.Text += " " + str + ",";
        }
    }

    public List<MediaTrack> GetMediaInfoTracks(string path)
    {
        List<MediaTrack> tracks = new List<MediaTrack>();

        using (MediaInfo mi = new MediaInfo(path))
        {
            MediaTrack track = new MediaTrack();
            Add(track, mi.GetGeneral("Format"));
            Add(track, mi.GetGeneral("FileSize/String"));
            Add(track, mi.GetGeneral("Duration/String"));
            Add(track, mi.GetGeneral("OverallBitRate/String"));
            track.Text = "G: " + track.Text.Trim(' ', ',');
            track.Type = "g";
            tracks.Add(track);

            int videoCount = mi.GetCount(MediaInfoStreamKind.Video);

            for (int i = 0; i < videoCount; i++)
            {
                string fps = mi.GetVideo(i, "FrameRate");

                if (float.TryParse(fps, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                    fps = result.ToString(CultureInfo.InvariantCulture);

                track = new MediaTrack();
                Add(track, mi.GetVideo(i, "Format"));
                Add(track, mi.GetVideo(i, "Format_Profile"));
                Add(track, mi.GetVideo(i, "Width") + "x" + mi.GetVideo(i, "Height"));
                Add(track, mi.GetVideo(i, "BitRate/String"));
                Add(track, fps + " FPS");
                Add(track, (videoCount > 1 && mi.GetVideo(i, "Default") == "Yes") ? "Default" : "");
                track.Text = "V: " + track.Text.Trim(' ', ',');
                track.Type = "v";
                track.ID = i + 1;
                tracks.Add(track);
            }

            int audioCount = mi.GetCount(MediaInfoStreamKind.Audio);

            for (int i = 0; i < audioCount; i++)
            {
                string lang = mi.GetAudio(i, "Language/String");
                string nativeLang = GetNativeLanguage(lang);
                string? title = mi.GetAudio(i, "Title");
                string format = mi.GetAudio(i, "Format");

                if (!string.IsNullOrEmpty(title))
                {
                    if (title.Contains("DTS-HD MA"))
                        format = "DTS-MA";

                    if (title.Contains("DTS-HD MA"))
                        title = title.Replace("DTS-HD MA", "");

                    if (title.ContainsEx("Blu-ray"))
                        title = title.Replace("Blu-ray", "");

                    if (title.ContainsEx("UHD "))
                        title = title.Replace("UHD ", "");

                    if (title.ContainsEx("EAC"))
                        title = title.Replace("EAC", "E-AC");

                    if (title.ContainsEx("AC3"))
                        title = title.Replace("AC3", "AC-3");

                    if (title.ContainsEx(lang))
                        title = title.Replace(lang, "").Trim();

                    if (title.ContainsEx(nativeLang))
                        title = title.Replace(nativeLang, "").Trim();

                    if (title.ContainsEx("Surround"))
                        title = title.Replace("Surround", "");

                    if (title.ContainsEx("Dolby Digital"))
                        title = title.Replace("Dolby Digital", "");

                    if (title.ContainsEx("Stereo"))
                        title = title.Replace("Stereo", "");

                    if (title.StartsWithEx(format + " "))
                        title = title.Replace(format + " ", "");

                    foreach (string i2 in new[] { "2.0", "5.1", "6.1", "7.1" })
                        if (title.ContainsEx(i2))
                            title = title.Replace(i2, "").Trim();

                    if (title.ContainsEx("@ "))
                        title = title.Replace("@ ", "");

                    if (title.ContainsEx(" @"))
                        title = title.Replace(" @", "");

                    if (title.ContainsEx("()"))
                        title = title.Replace("()", "");

                    if (title.ContainsEx("[]"))
                        title = title.Replace("[]", "");

                    if (title.TrimEx() == format)
                        title = null;

                    if (!string.IsNullOrEmpty(title))
                        title = title.Trim(" _-".ToCharArray());
                }

                track = new MediaTrack();
                Add(track, lang);
                Add(track, format);
                Add(track, mi.GetAudio(i, "Format_Profile"));
                Add(track, mi.GetAudio(i, "BitRate/String"));
                Add(track, mi.GetAudio(i, "Channel(s)") + " ch");
                Add(track, mi.GetAudio(i, "SamplingRate/String"));
                Add(track, mi.GetAudio(i, "Forced") == "Yes" ? "Forced" : "");
                Add(track, (audioCount > 1 && mi.GetAudio(i, "Default") == "Yes") ? "Default" : "");
                Add(track, title);

                if (track.Text.Contains("MPEG Audio, Layer 2"))
                    track.Text = track.Text.Replace("MPEG Audio, Layer 2", "MP2");

                if (track.Text.Contains("MPEG Audio, Layer 3"))
                    track.Text = track.Text.Replace("MPEG Audio, Layer 2", "MP3");

                track.Text = "A: " + track.Text.Trim(' ', ',');
                track.Type = "a";
                track.ID = i + 1;
                tracks.Add(track);
            }

            int subCount = mi.GetCount(MediaInfoStreamKind.Text);

            for (int i = 0; i < subCount; i++)
            {
                string codec = mi.GetText(i, "Format").ToUpperEx();

                if (codec == "UTF-8")
                    codec = "SRT";
                else if (codec == "WEBVTT")
                    codec = "VTT";
                else if (codec == "VOBSUB")
                    codec = "VOB";

                string lang = mi.GetText(i, "Language/String");
                string nativeLang = GetNativeLanguage(lang);
                string title = mi.GetText(i, "Title");
                bool forced = mi.GetText(i, "Forced") == "Yes";

                if (!string.IsNullOrEmpty(title))
                {
                    if (title.ContainsEx("VobSub"))
                        title = title.Replace("VobSub", "VOB");

                    if (title.ContainsEx(codec))
                        title = title.Replace(codec, "");

                    if (title.ContainsEx(lang.ToLowerEx()))
                        title = title.Replace(lang.ToLowerEx(), lang);

                    if (title.ContainsEx(nativeLang.ToLowerEx()))
                        title = title.Replace(nativeLang.ToLowerEx(), nativeLang).Trim();

                    if (title.ContainsEx(lang))
                        title = title.Replace(lang, "");

                    if (title.ContainsEx(nativeLang))
                        title = title.Replace(nativeLang, "").Trim();

                    if (title.ContainsEx("full"))
                        title = title.Replace("full", "").Trim();

                    if (title.ContainsEx("Full"))
                        title = title.Replace("Full", "").Trim();

                    if (title.ContainsEx("Subtitles"))
                        title = title.Replace("Subtitles", "").Trim();

                    if (title.ContainsEx("forced"))
                        title = title.Replace("forced", "Forced").Trim();

                    if (forced && title.ContainsEx("Forced"))
                        title = title.Replace("Forced", "").Trim();

                    if (title.ContainsEx("()"))
                        title = title.Replace("()", "");

                    if (title.ContainsEx("[]"))
                        title = title.Replace("[]", "");

                    if (!string.IsNullOrEmpty(title))
                        title = title.Trim(" _-".ToCharArray());
                }

                track = new MediaTrack();
                Add(track, lang);
                Add(track, codec);
                Add(track, mi.GetText(i, "Format_Profile"));
                Add(track, forced ? "Forced" : "");
                Add(track, (subCount > 1 && mi.GetText(i, "Default") == "Yes") ? "Default" : "");
                Add(track, title);
                track.Text = "S: " + track.Text.Trim(' ', ',');
                track.Type = "s";
                track.ID = i + 1;
                tracks.Add(track);
            }
        }

        int editionCount = GetPropertyInt("edition-list/count");

        for (int i = 0; i < editionCount; i++)
        {
            string title = GetPropertyString($"edition-list/{i}/title");

            if (string.IsNullOrEmpty(title))
                title = "Edition " + i;

            MediaTrack track = new MediaTrack
            {
                Text = "E: " + title,
                Type = "e",
                ID = i
            };

            tracks.Add(track);
        }

        return tracks;

        static void Add(MediaTrack track, object? value)
        {
            string str = value?.ToString()?.Trim() ?? "";

            if (str != "" && !(track.Text != null && track.Text.Contains(str)))
                track.Text += " " + str + ",";
        }
    }


    public MpvClient CreateNewPlayer(string name)
    {
        var client = new MpvClient { Handle = mpv_create_client(MainHandle, name) };

        if (client.Handle == IntPtr.Zero)
            throw new Exception("Error CreateNewPlayer");

        Util.Run(client.EventLoop);
        Clients.Add(client);
        return client;
    }


    public override void Dispose()
    {
        base.Dispose();

        mpv_destroy(MainHandle);
        mpv_destroy(Handle);

        foreach (var client in Clients)
            mpv_destroy(client.Handle);

        GC.SuppressFinalize(this);
    }

}