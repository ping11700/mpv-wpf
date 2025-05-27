namespace mpv_wpf._View;

/// <summary>
/// 视频播放器 专用窗体 Base
/// </summary>
public abstract class BasePlayerWindow : BaseWindowChrome
{
    protected IPlayer? IPlayer { get; init; }


    #region 判断 Click

    private uint _preDownTime;
    private uint _preClickTime;
    #endregion


    public BasePlayerWindow(IPlayer player)
    {
        IPlayer = player;

        CommandBindings.Add(new CommandBinding(ControlCommands.Fix, Fix));
        CommandBindings.Add(new CommandBinding(ControlCommands.Setting, Setting));

        CommandBindings.Add(new CommandBinding(ControlCommands.ToggleFullScreen, ToggleFullScreen));
        CommandBindings.Add(new CommandBinding(ControlCommands.ToggleDrawer, ToggleDrawer));
    }



    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        var source = PresentationSource.FromVisual(this) as HwndSource;
        source?.AddHook(WndProc);
    }


    /// <summary>
    /// 
    /// </summary>
    public virtual new void Show()
    {
        //阻止系统睡眠，阻止屏幕关闭。
        API_SystemSleep.PreventForCurrentThread();

        API_Window.TopWin(this);

        base.Show();
    }




    /// <summary>
    /// 
    /// </summary>
    public virtual new void Close()
    {
        base.Close();
    }



    /// <summary>
    /// 
    /// </summary>
    public virtual void OnClosing(bool isHide)
    {
        if (isHide == true)
        {
            IPlayer?.Stop();

            base.Hide();
        }
        else
        {
            var source = PresentationSource.FromVisual(this) as HwndSource;
            source?.RemoveHook(WndProc);

            IPlayer?.Dispose();

        }
        //恢复此线程曾经阻止的系统休眠和屏幕关闭。
        API_SystemSleep.RestoreForCurrentThread();
    }


    /// <summary>
    /// 置顶/非置顶
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Fix(object sender, ExecutedRoutedEventArgs e)
    {
        if (sender is Window win)
        {
            bool b = !win.Topmost;
            win.SetCurrentValue(Window.TopmostProperty, b);
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Setting(object sender, ExecutedRoutedEventArgs e) { }




    #region FullScreen


    /// <summary>
    /// 全屏切换
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void ToggleFullScreen(object sender, ExecutedRoutedEventArgs e)
    {
        TogFullScreen();
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    private void TogFullScreen(bool? tag = null)
    {
        bool b = tag is null ? !IPlayer?.IsFullScreen ?? false : (bool)tag;

        LayoutFullScreen(b);

        if (b == true)
        {
            API_Window.StartFullScreen(this);
        }
        else
        {
            API_Window.EndFullScreen(this);
        }

        IPlayer.IsFullScreen = b;

    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract void LayoutFullScreen(bool isIn);

    #endregion


    #region ToggleDrawer


    /// <summary>
    ///  抽屉开关
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void ToggleDrawer(object sender, ExecutedRoutedEventArgs e)
    {
        ToggleDrawer();
    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract void ToggleDrawer();

    #endregion



    #region  Play Local 本地文件播放

    /// <summary>
    /// 
    /// </summary>
    protected abstract void PlayLocalMedias(string[] paths);

    #endregion


    #region Hook

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case InteropValues.WM_LBUTTONDOWN:
                _preDownTime = NativeAPI.GetMessageTime();
                break;
            case InteropValues.WM_LBUTTONUP:
                uint nCurClickTime = NativeAPI.GetMessageTime();
                var doubelClickdelta = nCurClickTime - _preClickTime;
                var clickdelta = nCurClickTime - _preDownTime;

                _preClickTime = NativeAPI.GetMessageTime();

                //防止拖动点击
                if (clickdelta > 150) break;

                //单击
                //if (doubelClickdelta > DoubleClickTime)//500 实际体验 间隔时间较长
                if (doubelClickdelta > 300) IPlayer?.TogglePlay();
                //双击
                else
                {
                    IPlayer?.TogglePlay();

                    TogFullScreen();
                }

                break;
            //WPF 无法获取此消息
            case InteropValues.WM_NCLBUTTONDBLCLK:
                break;

            case InteropValues.WM_KEYDOWN:
                var keyCode = (Core.Hook.Keys)(int)wParam & Core.Hook.Keys.KeyCode;
                switch (keyCode)
                {
                    //退出全屏
                    case Core.Hook.Keys.Escape:
                        TogFullScreen(false);
                        break;
                    //全屏
                    case Core.Hook.Keys.Enter:
                        TogFullScreen(true);
                        break;
                    //播放/暂停
                    case Core.Hook.Keys.Space:
                        IPlayer?.TogglePlay();
                        break;
                    //增加音量
                    case Core.Hook.Keys.Up:
                        IPlayer.Volume += 10;
                        break;
                    //降低音量
                    case Core.Hook.Keys.Down:
                        IPlayer.Volume -= 10;
                        break;
                    //快退10s
                    case Core.Hook.Keys.Left:
                        IPlayer?.SeekTo(Math.Max(0, IPlayer.CurrentPos - 10 * 1000));
                        break;
                    //快进10s
                    case Core.Hook.Keys.Right:
                        IPlayer?.SeekTo(Math.Min(IPlayer.Duration, IPlayer.CurrentPos + 10 * 1000));
                        break;
                    default:
                        break;
                }

                break;
            case InteropValues.WM_SETTINGCHANGE:
                break;


            case InteropValues.WM_INPUTLANGCHANGE:
                break;


            case InteropValues.WM_NCDESTROY:
                break;

            case InteropValues.WM_SETTEXT:
                var str = Marshal.PtrToStringAuto(lParam);
                var arr = System.Text.Json.JsonSerializer.Deserialize<string[]>(str);
                //防止异常参数进入(mpv 入参会包含文件路径)
                if (arr?.Length > 0 && arr.All(x => File.Exists(x)))
                    PlayLocalMedias(arr);

                handled = true;
                break;
            default:
                break;
        }

        return IntPtr.Zero;
    }

    #endregion
}

