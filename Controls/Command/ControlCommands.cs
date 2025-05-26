namespace Controls.Command;

/// <summary>
/// 控件库使用的所有命令（为了统一，不使用wpf自带的命令）
/// </summary>
public static class ControlCommands
{

    /// <summary>
    ///    置顶/取消置顶
    /// </summary>
    public static RoutedCommand Fix { get; } = new(nameof(Fix), typeof(ControlCommands));


    /// <summary>
    ///    最小化
    /// </summary>
    public static RoutedCommand Min { get; } = new(nameof(Min), typeof(ControlCommands));

    /// <summary>
    ///    最大化/normal
    /// </summary>
    public static RoutedCommand Max_normal { get; } = new(nameof(Max_normal), typeof(ControlCommands));

    /// <summary>
    ///     关闭
    /// </summary>
    public static RoutedCommand Close { get; } = new(nameof(Close), typeof(ControlCommands));




    /// <summary>
    ///    桌面播放
    /// </summary>
    public static RoutedCommand WallPaperEngine { get; } = new(nameof(WallPaperEngine), typeof(ControlCommands));

    /// <summary>
    /// 小窗播放
    /// </summary>
    public static RoutedCommand MiniPlay { get; } = new(nameof(MiniPlay), typeof(ControlCommands));

    /// <summary>
    ///     全屏
    /// </summary>
    public static RoutedCommand ToggleFullScreen { get; } = new(nameof(ToggleFullScreen), typeof(ControlCommands));



    /// <summary>
    ///     抽屉开关
    /// </summary>
    public static RoutedCommand ToggleDrawer { get; } = new(nameof(ToggleDrawer), typeof(ControlCommands));


    /// <summary>
    ///     搜索
    /// </summary>
    public static RoutedCommand Search { get; } = new(nameof(Search), typeof(ControlCommands));



    /// <summary>
    ///     清除
    /// </summary>
    public static RoutedCommand Clear { get; } = new(nameof(Clear), typeof(ControlCommands));


    /// <summary>
    ///     详情
    /// </summary>
    public static RoutedCommand Detail { get; } = new(nameof(Detail), typeof(ControlCommands));


    /// <summary>
    ///     移除
    /// </summary>
    public static RoutedCommand Remove { get; } = new(nameof(Remove), typeof(ControlCommands));



    /// <summary>
    /// 操作日志
    /// </summary>
    public static RoutedCommand Operate { get; } = new(nameof(Operate), typeof(ControlCommands));



    /// <summary>
    ///     下一页
    /// </summary>
    public static RoutedCommand NextPage { get; } = new(nameof(NextPage), typeof(ControlCommands));


    /// <summary>
    ///     上一页
    /// </summary>
    public static RoutedCommand PrevPage { get; } = new(nameof(PrevPage), typeof(ControlCommands));






    /// <summary>
    ///     右转
    /// </summary>
    public static RoutedCommand RotateRight { get; } = new(nameof(RotateRight), typeof(ControlCommands));

    /// <summary>
    ///     左转
    /// </summary>
    public static RoutedCommand RotateLeft { get; } = new(nameof(RotateLeft), typeof(ControlCommands));

    /// <summary>
    ///     小
    /// </summary>
    public static RoutedCommand Reduce { get; } = new(nameof(Reduce), typeof(ControlCommands));

    /// <summary>
    ///     大
    /// </summary>
    public static RoutedCommand Enlarge { get; } = new(nameof(Enlarge), typeof(ControlCommands));

    /// <summary>
    ///     还原
    /// </summary>
    public static RoutedCommand Restore { get; } = new(nameof(Restore), typeof(ControlCommands));

    /// <summary>
    ///     打开
    /// </summary>
    public static RoutedCommand Open { get; } = new(nameof(Open), typeof(ControlCommands));


    /// <summary>
    ///     保存
    /// </summary>
    public static RoutedCommand Save { get; } = new(nameof(Save), typeof(ControlCommands));



    /// <summary>
    ///    排序
    /// </summary>
    public static RoutedCommand Sort { get; } = new(nameof(Sort), typeof(ControlCommands));

    /// <summary>
    ///     选中
    /// </summary>
    public static RoutedCommand Selected { get; } = new(nameof(Selected), typeof(ControlCommands));

    /// <summary>
    ///     取消
    /// </summary>
    public static RoutedCommand Cancel { get; } = new(nameof(Cancel), typeof(ControlCommands));

    /// <summary>
    ///     确定
    /// </summary>
    public static RoutedCommand Confirm { get; } = new(nameof(Confirm), typeof(ControlCommands));

    /// <summary>
    ///     是
    /// </summary>
    public static RoutedCommand Yes { get; } = new(nameof(Yes), typeof(ControlCommands));

    /// <summary>
    ///     否
    /// </summary>
    public static RoutedCommand No { get; } = new(nameof(No), typeof(ControlCommands));

    /// <summary>
    ///     关闭所有
    /// </summary>
    public static RoutedCommand CloseAll { get; } = new(nameof(CloseAll), typeof(ControlCommands));

    /// <summary>
    ///     关闭其他
    /// </summary>
    public static RoutedCommand CloseOther { get; } = new(nameof(CloseOther), typeof(ControlCommands));

    /// <summary>
    ///     上一个
    /// </summary>
    public static RoutedCommand Prev { get; } = new(nameof(Prev), typeof(ControlCommands));

    /// <summary>
    ///     下一个
    /// </summary>
    public static RoutedCommand Next { get; } = new(nameof(Next), typeof(ControlCommands));

    /// <summary>
    ///     跳转
    /// </summary>
    public static RoutedCommand Jump { get; } = new(nameof(Jump), typeof(ControlCommands));

    /// <summary>
    ///     上午
    /// </summary>
    public static RoutedCommand Am { get; } = new(nameof(Am), typeof(ControlCommands));

    /// <summary>
    ///     下午
    /// </summary>
    public static RoutedCommand Pm { get; } = new(nameof(Pm), typeof(ControlCommands));

    /// <summary>
    ///     确认
    /// </summary>
    public static RoutedCommand Sure { get; } = new(nameof(Sure), typeof(ControlCommands));

    /// <summary>
    ///      
    /// </summary>
    public static RoutedCommand More { get; } = new(nameof(More), typeof(ControlCommands));

    /// <summary>
    /// 
    /// </summary>
    public static RoutedCommand Setting { get; } = new(nameof(Setting), typeof(ControlCommands));


    /// <summary>
    ///     小时改变
    /// </summary>
    public static RoutedCommand HourChange { get; } = new(nameof(HourChange), typeof(ControlCommands));

    /// <summary>
    ///     分钟改变
    /// </summary>
    public static RoutedCommand MinuteChange { get; } = new(nameof(MinuteChange), typeof(ControlCommands));

    /// <summary>
    ///     秒改变
    /// </summary>
    public static RoutedCommand SecondChange { get; } = new(nameof(SecondChange), typeof(ControlCommands));

    /// <summary>
    ///     鼠标移动
    /// </summary>
    public static RoutedCommand MouseMove { get; } = new(nameof(MouseMove), typeof(ControlCommands));




}