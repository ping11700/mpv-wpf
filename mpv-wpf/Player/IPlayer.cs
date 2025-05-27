namespace mpv_wpf.Player;


public interface IPlayer
{

    /// <summary>
    ///  全屏
    /// </summary>
    bool IsFullScreen { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string Path { get; }

    /// <summary>
    /// 
    /// </summary>
    string FileName { get; }

    /// <summary>
    /// 静音
    /// </summary>
    bool IsMute { get; set; }

    /// <summary>
    /// 播放进度
    /// </summary>
    long CurrentPos { get; }

    /// <summary>
    /// 缓存进度
    /// </summary>
    long CachePos { get; }

    /// <summary>
    /// 当前影片总长
    /// </summary>
    long Duration { get; }

    /// <summary>
    /// 播放速度
    /// </summary>
    double Rate { get; set; }

    /// <summary>
    /// 音量
    /// </summary>
    int Volume { get; set; }

    /// <summary>
    /// 最大音量
    /// </summary>
    int MaxVolume { get; }

    /// <summary>
    /// 是否正在 暂停
    /// </summary>
    bool IsPause { get; }

    /// <summary>
    /// 是否正在加载
    /// </summary>
    bool IsLoading { get; }


    /// <summary>
    /// 是否播放结束
    /// </summary>
    bool IsPlayEnd { get; }


    /// <summary>
    /// 前进或后退的 步长
    /// </summary>
    uint ForwardBackwardStep { get; set; }


    /// <summary>
    /// 字幕字体大小
    /// </summary>
    int SubScale { set; }


    /// <summary>
    ///初始化 event
    /// </summary>
    event Action? InitializedEvent;


    /// <summary>
    /// mini 播放 event
    /// </summary>
    event Action<bool>? MiniEvent;


    /// <summary>
    /// 预播
    /// </summary>
    event Action<bool>? PrePlayEvent;


    /// <summary>
    /// 出问题了
    /// </summary>
    event Action<string>? ErrorEvent;

    /// <summary>
    /// 当前进度变化
    /// </summary>
    event Action<long>? PosChangedEvent;

    /// <summary>
    /// 缓存进度变化
    /// </summary>
    event Action<long>? CachePosChangedEvent;

    /// <summary>
    /// 跳转进度
    /// </summary>
    event Action<long>? SeekedEvent;

    /// <summary>
    /// 总长变化
    /// </summary>
    event Action<long>? DurationChangedEvent;


    /// <summary>
    /// 播放结束
    /// </summary>
    event Action? Play2EndEvent;

    /// <summary>
    /// 播放切换
    /// </summary>
    event Action? PlayStateChangedEvent;

    /// <summary>
    /// 开始加载
    /// </summary>
    public event Action? LoadingStartedEvent;

    /// <summary>
    /// 加载结束
    /// </summary>
    public event Action? LoadingEndEvent;


    /// <summary>
    /// 切换静音
    /// </summary>
    ICommand ToggleMuteCmd { get; }

    /// <summary>
    /// 切换 Play
    /// </summary>
    ICommand TogglePlayCmd { get; }


    /// <summary>
    /// 前进
    /// </summary>
    ICommand ForwardCmd { get; }

    /// <summary>
    /// 后退
    /// </summary>
    ICommand BackwardCmd { get; }

    /// <summary>
    /// 跳转
    /// </summary>
    ICommand SeekCmd { get; }

    /// <summary>
    /// 音频设备列表
    /// </summary>
    IEnumerable AudioDevices { get; }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="intPtr"></param>
    void SetHandle(IntPtr intPtr);



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pro"></param>
    /// <param name="value"></param>
    void SetProperty(string pro, object value);



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pro"></param>
    /// <returns></returns>
    T GetProperty<T>(string pro);


    /// <summary>
    ///  
    /// </summary>
    /// <param name="urls"></param>
    void Start(string url);

    /// <summary>
    ///  
    /// </summary>
    /// <param name="urls"></param>
    /// <param name="isFile">是否本地文件</param>
    void Start(string[] urls, bool isFile = false);



    /// <summary>
    /// 播放/暂停
    /// </summary>
    void TogglePlay();

    /// <summary>
    ///  
    /// </summary>
    void Play();

    /// <summary>
    /// 暂停播放
    /// </summary>
    void Pause();


    /// <summary>
    /// 停止播放
    /// </summary>
    void Stop();



    /// <summary>
    /// 跳转到
    /// </summary>
    /// <param name="seekToPos"></param>
    /// <param name="raiseEvent"></param>
    void SeekTo(long seekToPos, bool raiseEvent = true);


    /// <summary>
    /// 静音开关
    /// </summary>
    void ToggleMute();



    /// <summary>
    /// 开关字幕
    /// </summary>
    void ToggleSub(string? path = null);



    /// <summary>
    /// 截图
    /// </summary>
    /// <param name=""></param>
    void Screenshot(string filePath);



    /// <summary>
    /// 缩略图
    /// </summary>
    /// <param name="positon"></param>
    void Thumb(long? positon);



    /// <summary>
    /// 设置着色器
    /// </summary>
    /// <param name="positon"></param>
    void SetShaders(string[]? paths);



    /// <summary>
    /// 设置滤镜
    /// </summary>
    /// <param name="positon"></param>
    void SetVF(string? path);


    void Dispose();
}