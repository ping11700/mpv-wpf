﻿namespace Controls.Hwnd;


public class VideoHwndHost : HwndHost
{
    private const int HTTRANSPARENT = -1;


    public VideoHwndHost()
    {
        base.VerticalAlignment = VerticalAlignment.Stretch;
        base.HorizontalAlignment = HorizontalAlignment.Stretch;
    }

    protected override HandleRef BuildWindowCore(HandleRef hWndParent)
    {
        CustomWindProcDelegate = CustomWndProc;

        var windClass = WNDCLASSEX.Build();
        windClass.lpszClassName = CustomClassName;
        windClass.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(CustomWindProcDelegate);
        windClass.hbrBackground = CreateSolidBrush(0);
        var classAtom = RegisterClassEx(ref windClass);

        var lastError = Marshal.GetLastWin32Error();

        if (classAtom == 0 && lastError != ERROR_CLASS_ALREADY_EXISTS)
        {
            throw new Exception("Could not register window class");
        }

        IntPtr intPtr = CreateWindowEx(
            0,
            CustomClassName,
            string.Empty,
            WindowStyles.WS_CHILD | WindowStyles.WS_VISIBLE,
            0, 0, 0, 0,
            hWndParent.Handle,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);

        lastError = Marshal.GetLastWin32Error();
        if (lastError != 0)
        {
            throw new Exception($"Could not create window: {lastError}");
        }
        return new HandleRef(this, intPtr);
    }

    protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {

        switch (msg)
        {
            case InteropValues.WM_NCHITTEST:
                // 这里，我强行让所有区域返回 HTTRANSPARENT，于是整个子窗口都交给父窗口处理消息。
                // 正常，你应该在这里计算窗口边缘。
                handled = true;
                return new IntPtr(HTTRANSPARENT);

            default:
                break;
        }

        return IntPtr.Zero;
    }

    protected override void DestroyWindowCore(HandleRef hWnd)
    {
        DestroyWindow(hWnd.Handle);
    }

    private static IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        return DefWindowProc(hWnd, msg, wParam, lParam);
    }


    internal delegate IntPtr CustomWndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    internal CustomWndProcDelegate CustomWindProcDelegate;


    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr CreateWindowEx(
        ExtendedWindowStyles dwExStyle,
        [MarshalAs(UnmanagedType.LPWStr)]
        string lpClassName,
        [MarshalAs(UnmanagedType.LPWStr)]
        string lpWindowName,
        WindowStyles dwStyle,
        int x, int y, int nWidth, int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr CreateSolidBrush(uint theColor);

    internal const string CustomClassName = "mpvcustom";

    internal const int ERROR_CLASS_ALREADY_EXISTS = 1410;

    [Flags]
    internal enum ExtendedWindowStyles : uint
    {
        WS_EX_TRANSPARENT = 0x00000020
    }

    [Flags]
    internal enum WindowStyles : uint
    {
        WS_CHILD = 0x40000000,
        WS_VISIBLE = 0x10000000,
    }

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.U2)]
    static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct WNDCLASSEX
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public int style;
        public IntPtr lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszMenuName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszClassName;
        public IntPtr hIconSm;

        public static WNDCLASSEX Build()
        {
            var nw = new WNDCLASSEX
            {
                cbSize = Marshal.SizeOf(typeof(WNDCLASSEX))
            };
            return nw;
        }
    }
}