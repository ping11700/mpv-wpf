namespace mpv_wpf.Player;

public class PlayerMpv : NotifyObject, IPlayer
{
    public bool IsFullScreen { get; set; }

    public string Path => _player.Path;

    public string FileName => _player.FileName;

    public long Duration => (long)(_player.Duration * 1000);

    public long CurrentPos => (long)(_player.TimePos * 1000);

    public long CachePos => (long)(_player.CacheBufferingTime * 1000);

    public bool IsMute { get => _player.IsMute; set => _player.IsMute = value; }

    public int MaxVolume => _player.VolumeMax;

    public int Volume { get => _player.Volume; set { _player.Volume = value; IsMute = value <= 0; OnPropertyChanged(nameof(Volume)); OnPropertyChanged(nameof(IsMute)); } }

    public double Rate { get => _player.Speed; set => _player.Speed = value; } //<0.01-100>

    public bool IsPause => _player.IsPause;

    public bool IsLoading => _player.IsSeeking;

    public bool IsPlayEnd { get; private set; }

    public uint ForwardBackwardStep { get; set; }

    public void ToggleSub(string? path = null) => _player.TogSub(path);

    public int SubScale { set => _player.SubScale = value; }

    public void Thumb(long? positon) => _player.Thumb(positon);

    public void SetShaders(string[]? paths) => _player.SetShaders(paths);

    public void SetVF(string? path) => _player.SetVF(path);

    public IEnumerable AudioDevices => _player.AudioDevices;



    #region CMD
    public ICommand TogglePlayCmd => field ??= new RelayCommand(() => TogglePlay());


    public ICommand ForwardCmd => field ??= new RelayCommand(() => SeekTo(Math.Min(Duration, CurrentPos + 10 * 1000)));

    public ICommand BackwardCmd => field ??= new RelayCommand(() => SeekTo(Math.Max(0, CurrentPos - 10 * 1000)));

    public ICommand SeekCmd => field ??= new RelayCommand((pos) => SeekTo((long)pos));

    public ICommand ToggleMuteCmd => field ??= new RelayCommand(() => ToggleMute());

    #endregion



    public event Action? InitializedEvent;

    public event Action<bool>? MiniEvent;

    public event Action<bool>? PrePlayEvent;

    public event Action<string>? ErrorEvent;

    public event Action? LoadingStartedEvent;

    public event Action? LoadingEndEvent;

    public event Action? PlayStateChangedEvent;

    public event Action<long>? PosChangedEvent;

    public event Action<long>? CachePosChangedEvent;

    public event Action<long>? SeekedEvent;

    public event Action<long>? DurationChangedEvent;

    public event Action? Play2EndEvent;


    private readonly MpvLib.Player _player;

    private bool _prePlay;

    public PlayerMpv()
    {
        _player = new MpvLib.Player();

        _player.InitializedEvent -= Player_InitializedEvent;
        _player.InitializedEvent += Player_InitializedEvent;
        _player.ErrorEvent -= Player_ErrorEvent;
        _player.ErrorEvent += Player_ErrorEvent;
        _player.FileLoadedEvent -= Player_FileLoadedEvent;
        _player.FileLoadedEvent += Player_FileLoadedEvent;
        _player.TimePosChangedEvent -= Player_TimePosChangedEvent;
        _player.TimePosChangedEvent += Player_TimePosChangedEvent;
        _player.PauseEvent -= Player_PauseEvent;
        _player.PauseEvent += Player_PauseEvent;
        _player.SeekEvent -= Player_SeekEvent;
        _player.SeekEvent += Player_SeekEvent;
        _player.EofReachedEvent -= Player_EofReachedEvent;
        _player.EofReachedEvent += Player_EofReachedEvent;
        _player.EndFileEvent -= Player_EndFileEvent;
        _player.EndFileEvent += Player_EndFileEvent;
    }





    #region events

    private void Player_InitializedEvent()
    {
        InitializedEvent?.Invoke();
    }


    private void Player_ErrorEvent(string msg)
    {
        Logger.Error($"PlayerError: {msg}");

        ErrorEvent?.Invoke(msg);
    }


    private void Player_FileLoadedEvent()
    {
        OnPropertyChanged(nameof(Duration));
     
        OnPropertyChanged(nameof(IsLoading));

        IsPlayEnd = false;
        OnPropertyChanged(nameof(IsPlayEnd));

        DurationChangedEvent?.Invoke(Duration);
    }

    private void Player_TimePosChangedEvent()
     {
        OnPropertyChanged(nameof(CurrentPos));
        OnPropertyChanged(nameof(CachePos));

        OnPropertyChanged(nameof(IsLoading));


        //TODO 此处涉及的业务计算, 是否过于频繁
        //经打印频率为1s
        //Trace.WriteLine(DateTime.Now.ToString("mm-ss-fff"));

        //任务栏进度
        API_Taskbar.SetProgressValue((int)CurrentPos, (int)Duration);
        //API_Taskbar.SetProgressState(TaskbarProgressBarState.Normal);

        PosChangedEvent?.Invoke(CurrentPos);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    private void Player_PauseEvent(bool b)
    {
        OnPropertyChanged(nameof(IsPause));

        PlayStateChangedEvent?.Invoke();
    }


    /// <summary>
    /// 
    /// </summary>
    private void Player_SeekEvent()
    {
    }



    /// <summary>
    /// 防止keep-open != no 时 Player_EndFileEvent 不执行
    /// SetPropertyString("keep-open", "yes");//可保持最后一帧, 和播放完毕后继续重新播放
    /// </summary>
    /// <param name="obj"></param>
    private void Player_EofReachedEvent(bool b)
    {
        if (_player.KeepOpen != "no" && b == true)
        {
            if (_prePlay == true) return;

            Play2EndEvent?.Invoke();

            IsPlayEnd = true;

            OnPropertyChanged(nameof(IsPlayEnd));
        }
    }


    /// <summary>
    /// keep-open = no 播放完成 执行此方法
    /// SetPropertyString("keep-open", "yes");//可保持最后一帧, 和播放完毕后继续重新播放
    /// </summary>
    /// <param name="error"></param>
    /// <param name="reason"></param>
    private void Player_EndFileEvent(MpvLib.Native.LibMpv.mpv_error error, MpvLib.Native.LibMpv.mpv_end_file_reason reason)
    {
        var reasonTag = reason switch
        {
            //该文件已经结束。这可以（但不一定）包括不完整的文件或网络连接中断的情况。
            MpvLib.Native.LibMpv.mpv_end_file_reason.MPV_END_FILE_REASON_EOF => "Eof",
            //播放被一个命令结束。
            MpvLib.Native.LibMpv.mpv_end_file_reason.MPV_END_FILE_REASON_STOP => "Stop",
            //播放是通过发送退出命令结束的。
            MpvLib.Native.LibMpv.mpv_end_file_reason.MPV_END_FILE_REASON_QUIT => "Quit",
            //发生了一个错误。在这种情况下，有一个 error 字段和错误字符串。
            MpvLib.Native.LibMpv.mpv_end_file_reason.MPV_END_FILE_REASON_ERROR => "Error",
            //发生在播放列表和类似的情况.
            MpvLib.Native.LibMpv.mpv_end_file_reason.MPV_END_FILE_REASON_REDIRECT => "Redirect",
            //未知。通常不会发生
            _ => "Unknown"
        };

        var errorTag = error switch
        {
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_SUCCESS => "MPV_ERROR_SUCCESS",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_EVENT_QUEUE_FULL => "MPV_ERROR_EVENT_QUEUE_FULL",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_NOMEM => "MPV_ERROR_NOMEM",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_UNINITIALIZED => "MPV_ERROR_UNINITIALIZED",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_INVALID_PARAMETER => "MPV_ERROR_INVALID_PARAMETER",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_OPTION_NOT_FOUND => "MPV_ERROR_OPTION_NOT_FOUND",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_OPTION_FORMAT => "MPV_ERROR_OPTION_FORMAT",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_OPTION_ERROR => "MPV_ERROR_OPTION_ERROR",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_PROPERTY_NOT_FOUND => "MPV_ERROR_PROPERTY_NOT_FOUND",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_PROPERTY_FORMAT => "MPV_ERROR_PROPERTY_FORMAT",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_PROPERTY_UNAVAILABLE => "MPV_ERROR_PROPERTY_UNAVAILABLE",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_PROPERTY_ERROR => "MPV_ERROR_PROPERTY_ERROR",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_COMMAND => "MPV_ERROR_COMMAND",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_LOADING_FAILED => "MPV_ERROR_LOADING_FAILED",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_AO_INIT_FAILED => "MPV_ERROR_AO_INIT_FAILED",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_VO_INIT_FAILED => "MPV_ERROR_VO_INIT_FAILED",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_NOTHING_TO_PLAY => "MPV_ERROR_NOTHING_TO_PLAY",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_UNKNOWN_FORMAT => /*"格式错误",*/"不支持此文件",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_UNSUPPORTED => "MPV_ERROR_UNSUPPORTED",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_NOT_IMPLEMENTED => "MPV_ERROR_NOT_IMPLEMENTED",
            MpvLib.Native.LibMpv.mpv_error.MPV_ERROR_GENERIC => "MPV_ERROR_GENERIC",
            _ => "Unknown"
        };

        if (reasonTag == "Eof")
        {
            Play2EndEvent?.Invoke();
        }
        else if (reasonTag == "Error")
        {
            //Growl.Warning(errorTag);

            ErrorEvent?.Invoke(errorTag);
        }

        IsPlayEnd = true;

        OnPropertyChanged(nameof(IsPlayEnd)); //keep-open = true / always 播放结束无法执行 Player_EndFileEvent

    }

    #endregion


    public void SetHandle(IntPtr intPtr)
    {
        _player.Init(intPtr);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pro"></param>
    /// <param name="value"></param>
    public void SetProperty(string pro, object value)
    {
        _player.SetProperty(pro, value);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pro"></param>
    /// <returns></returns>
    public T GetProperty<T>(string pro)
    {
        return _player.GetProperty<T>(pro);
    }



    /// <summary>
    ///  
    /// </summary>
    /// <param name="urls"></param>
    public void Start(string url)
    {
        _player.Start([url]);
    }




    /// <summary>
    ///  
    /// </summary>
    /// <param name="urls"></param>
    public void Start(string[] urls, bool isFile = false)
    {
        if (isFile == true)
        {
            urls = [.. urls.Where(x => !Util_IO.DetecCommonFile(x) && File.Exists(x))];

            if (urls.Length > 0)
                _player.Start(urls);
            //else
            //    Growl.Info("文件格式不支持~");
        }
        else
        {
            _player.Start(urls);
        }
    }

 

    public void TogglePlay()
    {
        //播放完成, 点击播放可重头开始播放
        if (IsPlayEnd == true)
        {
            _player.Start([_player.Path]);
        }
        else
        {
            _player.TogglePlay();
        }
    }


    public void Play()
    {
        _player.TogglePlay(isPlay: true);
    }


    public void Pause()
    {
        _player.TogglePlay(isPlay: false);
    }


    public void SeekTo(long seekToPos, bool raiseEvent = true)
    {
        //lj 代码
        if (raiseEvent)
            SeekedEvent?.Invoke(seekToPos);

        _player.Seek((int)seekToPos / 1000);

        //防止视频 暂停
        _player.TogglePlay(isPlay: true);
    }


    public void ToggleMute()
    {
        _player.ToggleMute();

        OnPropertyChanged(nameof(Volume));
        OnPropertyChanged(nameof(IsMute));
    }



    /// <summary>
    /// 截图
    /// </summary>
    /// <param name="filePath"></param>
    public void Screenshot(string filePath)
    {
        _player.Screenshot(filePath);
    }


    public void Stop()
    {
        _player.Stop();
    }


    public void Dispose()
    {
        _player.InitializedEvent -= Player_InitializedEvent;
        _player.ErrorEvent -= Player_ErrorEvent;
        _player.FileLoadedEvent -= Player_FileLoadedEvent;
        _player.TimePosChangedEvent -= Player_TimePosChangedEvent;
        _player.PauseEvent -= Player_PauseEvent;
        _player.SeekEvent -= Player_SeekEvent;
        _player.EofReachedEvent -= Player_EofReachedEvent;
        _player.EndFileEvent -= Player_EndFileEvent;

        _player?.Dispose();
    }
}
