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

     

        SetPropertyString("log-file", $"{AppContext.BaseDirectory}logs/mpv_{DateTime.Today:yyyy-MM-dd}.log");//log
        //SetPropertyString("msg-level", "all=warn");


        SetPropertyString("config-dir", $"{AppContext.BaseDirectory}portable_config");
        SetPropertyString("config", "yes");

        SetPropertyString("profile", "fast");
        SetPropertyString("hwdec", "auto");
        SetPropertyString("vo", "gpu");//视频输出驱动 


        SetPropertyString("title", "MPV-wpf播放器");
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
        SetPropertyString("load-scripts", "yes");

        mpv_error err = mpv_initialize(MainHandle);
        if (err < 0)
            throw new Exception("mpv_initialize error" + "\n\n" + GetError(err) + "\n");

        string idle = GetPropertyString("idle");
        //App.Exit = idle == "no" || idle == "once";

        Handle = mpv_create_client(MainHandle, "mpv");

        if (Handle == IntPtr.Zero)
            throw new Exception("mpv_create_client error");

        mpv_request_log_messages(Handle, "info");//监听log

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
        //Util.Run(UpdateTracks);

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
        LoadFiles([@"bd://"], false);
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