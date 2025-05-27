namespace mpv_wpf._ViewModel;

public partial class ViewModel_PlayerBase : NotifyObject, IDisposable
{
    #region Property

    /// <summary>
    /// 播放器
    /// </summary>
    public IPlayer IPlayer { get; init; }

    /// <summary>
    /// 加载中...
    /// </summary>
    public bool IsLoading { get; set => SetProperty(ref field, value); }



    #endregion


    #region CMM

    //public ICommand BackHoneCmd => field ??= new RelayCommand(() => _ipcSendService.BackHome());

    #endregion



    public ViewModel_PlayerBase(IPlayer player)
    {
        SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

        SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
        SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        IPlayer = player ?? throw new ArgumentNullException(nameof(player));

        IPlayer.InitializedEvent -= IPlayer_InitializedEvent;
        IPlayer.InitializedEvent += IPlayer_InitializedEvent;
        IPlayer.MiniEvent -= IPlayer_MiniEvent;
        IPlayer.MiniEvent += IPlayer_MiniEvent;
        IPlayer.ErrorEvent -= IPlayer_ErrorEvent;
        IPlayer.ErrorEvent += IPlayer_ErrorEvent;
        IPlayer.DurationChangedEvent -= IPlayer_DurationChanged;
        IPlayer.DurationChangedEvent += IPlayer_DurationChanged;
        IPlayer.PosChangedEvent -= IPlayer_CurrentPosChanged;
        IPlayer.PosChangedEvent += IPlayer_CurrentPosChanged;
        IPlayer.SeekedEvent -= IPlayer_Seeked;
        IPlayer.SeekedEvent += IPlayer_Seeked;
        IPlayer.PlayStateChangedEvent -= IPlayer_PlayStateChanged;
        IPlayer.PlayStateChangedEvent += IPlayer_PlayStateChanged;
        IPlayer.LoadingStartedEvent -= IPlayer_LoadingStarted;
        IPlayer.LoadingStartedEvent += IPlayer_LoadingStarted;
        IPlayer.LoadingEndEvent -= IPlayer_LoadingEnd;
        IPlayer.LoadingEndEvent += IPlayer_LoadingEnd;
        IPlayer.Play2EndEvent -= IPlayer_Play2End;
        IPlayer.Play2EndEvent += IPlayer_Play2End;

    }






    #region 系统事件



    private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {

    }


    private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
    {

    }


    #endregion


    #region 播放器事件

    private void IPlayer_InitializedEvent() { InitPlayerByConfig(); }
    protected virtual void IPlayer_MiniEvent(bool b) { }
    protected virtual void IPlayer_ErrorEvent(string msg) { }
    protected virtual void IPlayer_DurationChanged(long e) { }
    protected virtual void IPlayer_CurrentPosChanged(long e) { }
    protected virtual void IPlayer_Seeked(long e) { }
    protected virtual void IPlayer_PlayStateChanged() { }
    protected virtual void IPlayer_LoadingStarted() { }
    protected virtual void IPlayer_LoadingEnd() { }
    protected virtual void IPlayer_Play2End() { }
    #endregion



    /// <summary>
    /// 根据 playerConfig 初始化设置player
    /// </summary>
    private void InitPlayerByConfig()
    {
        ////设置解码方式
        //switch (_playerConfig.DecodeType)
        //{
        //    case DecodeType.Auto:
        //        IPlayer.SetProperty("hwdec", "auto");
        //        break;

        //    case DecodeType.Hardware:
        //        IPlayer.SetProperty("hwdec", "d3d11va");
        //        IPlayer.SetProperty("gpu-context", "d3d11va");
        //        break;

        //    case DecodeType.Software:
        //        IPlayer.SetProperty("hwdec", "no");
        //        break;
        //    default:
        //        break;
        //}

        ////设置字幕大小
        //IPlayer.SetProperty("sub-scale", _playerConfig.SubtitleSize / 100.0);
    }



    public virtual void Dispose()
    {
        if (IPlayer is not null)
        {
            IPlayer.InitializedEvent -= IPlayer_InitializedEvent;
            IPlayer.MiniEvent -= IPlayer_MiniEvent;
            IPlayer.ErrorEvent -= IPlayer_ErrorEvent;
            IPlayer.DurationChangedEvent -= IPlayer_DurationChanged;
            IPlayer.PosChangedEvent -= IPlayer_CurrentPosChanged;
            IPlayer.SeekedEvent -= IPlayer_Seeked;
            IPlayer.PlayStateChangedEvent -= IPlayer_PlayStateChanged;
            IPlayer.LoadingStartedEvent -= IPlayer_LoadingStarted;
            IPlayer.LoadingEndEvent -= IPlayer_LoadingEnd;
            IPlayer.Play2EndEvent -= IPlayer_Play2End;
        }

        SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
    }
}
