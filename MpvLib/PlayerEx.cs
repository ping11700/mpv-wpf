namespace MpvLib;


/// <summary>
/// https://hooke007.github.io/official_man/mpv.html
/// </summary>
public partial class Player
{
    public string Path => GetPropertyString("path");

    public string FileName => GetPropertyString("filename");

    public double Duration => GetPropertyDouble("duration");

    public double TimePos { get => GetPropertyDouble("time-pos"); set => SetPropertyDouble("time-pos", value); }

    public bool IsMute { get => GetPropertyString("mute") == "yes"; set => SetPropertyString("mute", value ? "yes" : "no"); }

    public int Volume { get => GetPropertyInt("volume"); set => SetPropertyInt("volume", Math.Min(value, VolumeMax)); }

    public int VolumeMax => GetPropertyInt("volume-max");

    public bool IsPause => GetPropertyString("pause") == "yes";

    public int VideoRotate => GetPropertyInt("video-rotate");


    /// <summary>
    /// <0.01-100>
    /// </summary>
    public double Speed { get => GetPropertyDouble("speed"); set => SetPropertyDouble("speed", value); }

    //视频在解复用器中缓存的大致时间，以秒为单位
    public double CacheBufferingTime => GetPropertyInt("demuxer-cache-time");

    /// <summary>
    /// 播放器目前是否正在跳转，或以其他方式尝试重新开始播放
    /// </summary>
    public bool IsSeeking => GetPropertyBool("seeking");


    public string SubFile { set => SetPropertyString("sub-file", value); }

    public int SubScale { set => SetPropertyInt("sub-scale", value); }


    public string KeepOpen => GetPropertyString("keep-open");


    /// <summary>
    /// 
    /// </summary>
    public void SetProperty(string pro, object value)
    {
        if (value is bool boolValue)
        {
            SetPropertyBool(pro, boolValue);
        }
        else if (value is string strValue)
        {
            SetPropertyString(pro, strValue);
        }
        else if (value is int intValue)
        {
            SetPropertyInt(pro, intValue);
        }
        else if (value is double doubleValue)
        {
            SetPropertyDouble(pro, doubleValue);
        }
        else if (value is long longValue)
        {
            SetPropertyLong(pro, longValue);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public T GetProperty<T>(string pro)
    {
        object value = default;

        if (typeof(T) == typeof(bool))
        {
            value = GetPropertyBool(pro);
        }
        else if (typeof(T) == typeof(string))
        {
            value = GetPropertyString(pro);
        }
        else if (typeof(T) == typeof(int))
        {
            value = GetPropertyInt(pro);
        }
        else if (typeof(T) == typeof(double))
        {
            value = GetPropertyDouble(pro);
        }
        else if (typeof(T) == typeof(long))
        {
            value = GetPropertyLong(pro);
        }
        return (T)value;
    }


    /// <summary>
    /// 
    /// </summary>
    public void ToggleMute()
    {
        if (IsMute)
            SetPropertyString("mute", "no");
        else
            SetPropertyString("mute", "yes");
    }


    /// <summary>
    /// 自动/手动强制 暂停, 播放
    /// </summary>
    public void TogglePlay(bool? isPlay = null)
    {
        if (isPlay is null)
        {
            Command("cycle pause");
        }
        else
        {
            if (isPlay == true)
                SetPropertyString("pause", "no");
            else
                SetPropertyString("pause", "yes");
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">秒</param>
    public void Start(string[] urls)
    {
        LoadFiles(urls, false);

        SetPropertyString("start", "0");

        SetPropertyString("pause", "no");
    }


    /// <summary>
    /// 
    /// </summary>
    public void Seek(int position)
    {
        CommandV("seek", position.ToString(), "absolute");
    }





    /// <summary>
    /// 
    /// </summary>
    public void TogSub(string? path = null)
    {
        if (string.IsNullOrEmpty(path))
            Command("sub-remove");
        else
            CommandV("sub-add", path);
    }


    /// <summary>
    /// 
    /// </summary>
    public void Screenshot(string path)
    {
        CommandV("screenshot-to-file", path, "subtitles");
    }


    /// <summary>
    /// 
    /// </summary>
    public void Stop()
    {
        Command("stop");
    }




    /// <summary>
    /// 缩略图
    /// </summary>
    public void Thumb(long? position)
    {
        CommandV("script-message-to", "thumbfast", "thumb", "2.0", "600", "400");
    }




    /// <summary>
    /// 
    /// </summary>
    public void SetShaders(string[]? paths)
    {
        if (paths is null)
        {
            CommandV("change-list", "glsl-shaders", "clr", "");
        }
        else
        {
            if (paths.Length <= 1)
            {
                CommandV("change-list", "glsl-shaders", "toggle", paths[0]);
            }
            else
            {
                CommandV("change-list glsl-shaders set", String.Join(";", paths));
            }
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    public void SetVF(string? path)
    {
        if (path is null)
        {
            CommandV("vf", "clr", "");
        }
        else
        {
            CommandV("vf", "set", $"vapoursynth={path}");
        }

    }



    public event Action? InitializedEvent;

    public event Action<bool>? PauseEvent;

    public event Action<System.Drawing.Size>? VideoSizeChangedEvent;

    public event Action<int>? PlaylistPosChangedEvent;

    public event Action<string>? PlayErrorEvent;

    public event Action? TimePosChangedEvent;

    public event Action<double>? CurrentPosChangedEvent;

    public event Action<long>? CachePosChangedEvent;

    public event Action<long>? SeekedEvent;


    public event Action<string>? SubtitleShowEvent;

    public event Action<bool>? EofReachedEvent;

    /// <summary>
    /// 
    /// </summary>
    public void SubscribeEvents()
    {
        ObservePropertyInt("playlist-pos", value => PlaylistPosChangedEvent?.Invoke(value));

        ObservePropertyInt("time-pos", value => TimePosChangedEvent?.Invoke());

        ObservePropertyBool("pause", value => PauseEvent?.Invoke(value));

        ObservePropertyInt("video-rotate", value => { if (VideoRotate != value) UpdateVideoSize("dwidth", "dheight"); });

        //是否到达播放进度的结束。
        //注意通常只有当 --keep-open 被启用时，这才有意义，因为否则播放器会立即播放下一个文件（或退出或进入空闲模式），
        //在这些情况下， eof-reach 属性被设置后，在逻辑上将立即被清除。
        ObservePropertyBool("eof-reached", value => EofReachedEvent?.Invoke(value));

    }



}