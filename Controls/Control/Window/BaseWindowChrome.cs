namespace Controls.Control;


public class BaseWindowChrome : Window
{
    public BaseWindowChrome()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Min, Min));
        CommandBindings.Add(new CommandBinding(ControlCommands.Max_normal, Max_normal));
        CommandBindings.Add(new CommandBinding(ControlCommands.Close, Close));

        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }


    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
    }

    protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
    {
        base.OnDpiChanged(oldDpi, newDpi);
    }



    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        //自动更改大小,重绘
        if (System.Windows.Shell.WindowChrome.GetWindowChrome(this) != null)
            InvalidateMeasure();

        //窗体消息处理
        var source = PresentationSource.FromVisual(this) as HwndSource;
        source?.AddHook(WndProc);
    }


    /// <summary>
    /// 窗体最小化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Min(object sender, ExecutedRoutedEventArgs e) => SystemCommands.MinimizeWindow(this);

    /// <summary>
    /// 窗体最大化/普通
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Max_normal(object sender, ExecutedRoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
            SystemCommands.RestoreWindow(this);
        else
            SystemCommands.MaximizeWindow(this);
    }

    /// <summary>
    /// 窗体关闭
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Close(object sender, ExecutedRoutedEventArgs e) => SystemCommands.CloseWindow(this);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (e?.LeftButton == MouseButtonState.Pressed)
            this.DragMove();
    }


    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // gets called finally
    }



    /// <summary>
    /// 窗体消息处理
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="msg"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <param name="handled"></param>
    /// <returns></returns>
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case InteropValues.WM_GETMINMAXINFO:
                // Adjust the maximized size and position to fit the work area of the correct monitor
                int MONITOR_DEFAULTTONEAREST = 0x00000002;
                var monitor = NativeAPI.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    var monitorInfo = new MONITORINFO();
                    monitorInfo.cbSize = (uint)Marshal.SizeOf(monitorInfo);
                    NativeAPI.GetMonitorInfo(monitor, ref monitorInfo);
                    var rcWorkArea = monitorInfo.rcWork;
                    var rcMonitorArea = monitorInfo.rcMonitor;

                    unsafe
                    {
                        MINMAXINFO* minMaxInfo = (MINMAXINFO*)lParam;

                        minMaxInfo->ptMaxPosition = new POINT()
                        {
                            X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left),
                            Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top)
                        };
                        minMaxInfo->ptMaxSize = new POINT()
                        {
                            X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left),
                            Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top)
                        };

                    }

                }

                handled = true;

                break;

            case InteropValues.WM_KEYDOWN:
                var keyCode = (Core.Hook.Keys)(int)wParam & Core.Hook.Keys.KeyCode;
                switch (keyCode)
                {
                    case Core.Hook.Keys.Escape:
                        SystemCommands.RestoreWindow(this);

                        break;
                }

                break;
        }

        return IntPtr.Zero;
    }
}