namespace Core.WindowsAPI;

/// <summary>
/// 背景播放引擎
/// </summary>
public class API_WallpaperEngine
{
    public event Action? PlayStateChangeEvent;

    public event Action<bool>? StateChangeEvent;

    /// <summary>
    /// 用于记录窗口桌面播放前位置的附加属性
    /// </summary>
    private static readonly DependencyProperty BeforeWindowPlacementProperty = DependencyProperty.RegisterAttached(
        "BeforeWindowPlacement", typeof(WINDOWPLACEMENT?), typeof(API_WallpaperEngine));


    private Window _win;

    private readonly Key_MouseHook _keyMouseHook = new();

    private int _escClickCount = 0;
    private int _spaceClickCount = 0;


    public void Start(Window win) => WallpaperEngineStart(win);


    /// <summary>
    ///     桌面播放开启
    /// </summary>
    /// <param name="win"></param>
    private void WallpaperEngineStart(Window win)
    {
        try
        {
            _win = win;

            var hwnd = new WindowInteropHelper(win).EnsureHandle();

            //获取当前窗口的位置大小状态并保存
            var placement = NativeAPI.GetWindowPlacementInternal(hwnd);
            win.SetValue(BeforeWindowPlacementProperty, placement);

            win.Left = 0.0;
            win.Top = 0.0;
            win.Width = SystemParameters.PrimaryScreenWidth;
            win.Height = SystemParameters.PrimaryScreenHeight;

            SetDeskTop(win);

            //安装键盘Hook
            _keyMouseHook.KeyUp -= KeyMouseHook_KeyUp;
            _keyMouseHook.KeyUp += KeyMouseHook_KeyUp;
            _keyMouseHook.Start(installMouseHook: false, installKeyboardHook: true);

            StateChangeEvent?.Invoke(true);

            Keyboard.ClearFocus();
        }
        catch (Exception)
        {
        }
    }



    /// <summary>
    ///     桌面播放结束
    /// </summary>
    /// <param name="win"></param>
    private void WallpaperEngineEnd()
    {
        try
        {
            var win = _win;

            var hwnd = new WindowInteropHelper(win).EnsureHandle();

            if (win.GetValue(BeforeWindowPlacementProperty) is WINDOWPLACEMENT placement)
            {
                NativeAPI.SetWindowPlacement(hwnd, ref placement);

                win.ClearValue(BeforeWindowPlacementProperty);
            }
            NativeAPI.SetParent(hwnd, IntPtr.Zero);

            _keyMouseHook.KeyUp -= KeyMouseHook_KeyUp;
            _keyMouseHook.Stop();

            win.Activate();

            _win = null;

            StateChangeEvent?.Invoke(false);
        }
        catch (Exception)
        {

        }

    }


    /// <summary>
    /// 将窗体设置为桌面 
    /// </summary>
    /// <param name="wpfWindow"></param>
    private void SetDeskTop(Window win)
    {
        try
        {
            IntPtr programIntPtr = NativeAPI.FindWindow("Progman", null);

            // 窗口句柄有效
            if (programIntPtr != IntPtr.Zero)
            {
                IntPtr result = IntPtr.Zero;
                NativeAPI.SendMessageTimeout(programIntPtr, 0x052C, IntPtr.Zero, IntPtr.Zero, 0x0000, 1000, IntPtr.Zero);

                // 遍历顶级窗口
                NativeAPI.EnumWindows((hwnd, lParam) =>
                {
                    // 找到包含 SHELLDLL_DefView 这个窗口句柄的 WorkerW
                    if (NativeAPI.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                    {
                        // 找到当前 WorkerW 窗口的，后一个 WorkerW 窗口。
                        IntPtr tempHwnd = NativeAPI.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

                        // 隐藏这个窗口
                        NativeAPI.ShowWindow(tempHwnd, SW.HIDE);
                    }

                    return true;
                }, IntPtr.Zero);
            }

            NativeAPI.SetParent(new System.Windows.Interop.WindowInteropHelper(win).Handle, programIntPtr);
        }
        catch (Exception ex)
        {
        }
    }


    /// <summary>
    /// 监听键盘KeyUp
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeyMouseHook_KeyUp(object sender, Hook.KeyEventArgs e)
    {

        if (e.Key == Keys.Escape)
        {
            _escClickCount++;

            if (_escClickCount > 1)
            {
                _escClickCount = 0;

                WallpaperEngineEnd();
            }
            else
            {
                Task.Delay(300).ContinueWith(_ =>
                {
                    _escClickCount = 0;
                });
            }
        }

        if (e.Key == Keys.Space)
        {
            _spaceClickCount++;

            if (_spaceClickCount > 1)
            {
                _spaceClickCount = 0;

                PlayStateChangeEvent?.Invoke();
            }
            else
            {
                Task.Delay(300).ContinueWith(_ =>
                {
                    _spaceClickCount = 0;
                });
            }
        }

    }

}
