namespace Core.WindowsAPI;

public class NativeAPI
{

    #region 窗体Dll Window

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetWindow(IntPtr hwnd, uint wCmd);

    [DllImport(ExternDll.User32, EntryPoint = "FindWindow")]

    ///参数1:指的是类名。参数2，指的是窗口的标题名。两者至少要知道1个
    public static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

    [DllImport(ExternDll.User32, EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr SendMessage(IntPtr hwnd, uint wMsg, uint wParam, IntPtr lParam);

    [DllImport(ExternDll.User32, EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, IntPtr lParam);

    [DllImport(ExternDll.User32, EntryPoint = "PostMessage", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport(ExternDll.User32, SetLastError = true)]
    internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


    [DllImport(ExternDll.User32)]
    internal static extern IntPtr SetParent(IntPtr hwnd, IntPtr parentHwnd);

    /*不用UI渲染完成即可获得句柄
      获得焦点窗口的句柄
    */
    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
    internal static extern IntPtr GetForegroundWindow();

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    internal static extern bool GetCursorPos(out POINT pt);

    [DllImport(ExternDll.User32)]
    internal static extern IntPtr GetDesktopWindow();



    [DllImport(ExternDll.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT lpwndpl);



    [SecurityCritical]
    [DllImport(ExternDll.User32, EntryPoint = "GetWindowRect", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);


    [SecurityCritical]
    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);



    [DllImport(ExternDll.User32)]
    internal static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);


    [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
    [ResourceExposure(ResourceScope.None)]
    internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT placement);


    [DllImport(ExternDll.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsIconic(IntPtr hwnd);


    [DllImport(ExternDll.User32, ExactSpelling = true)]
    [ResourceExposure(ResourceScope.None)]
    internal static extern IntPtr MonitorFromRect(ref RECT rect, uint flags);


    [DllImport(ExternDll.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO monitorInfo);


    [DllImport(ExternDll.User32)]
    public static extern IntPtr CreateWindowEx(ExtendedWindow32Styles dwExStyle,
             string lpszClassName,
             string lpszWindowName,
             Window32Styles style,
             int x, int y, int width, int height,
             IntPtr hwndParent,
             IntPtr hMenu,
             IntPtr hInst,
             IntPtr lpParam);


    [DllImport(ExternDll.User32)]
    public static extern bool DestroyWindow(IntPtr hwnd);



    [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool AttachThreadInput(in uint currentForegroundWindowThreadId, in uint thisWindowThreadId, bool isAttach);




    internal static WINDOWPLACEMENT GetWindowPlacementInternal(IntPtr hwnd)
    {
        WINDOWPLACEMENT wINDOWPLACEMENT = WINDOWPLACEMENT.Default;
        if (GetWindowPlacement(hwnd, ref wINDOWPLACEMENT))
        {
            return wINDOWPLACEMENT;
        }
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DllImport(ExternDll.User32, EntryPoint = "GetWindowLong")]
    private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport(ExternDll.User32, EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex) => IntPtr.Size == 8 ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);


    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
    private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) => IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : SetWindowLongPtr32(hWnd, nIndex, dwNewLong);


    [DllImport(ExternDll.DwmApi, ExactSpelling = true, SetLastError = true)]
    internal static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, in int pvAttribute, uint cbAttribute);


    [DllImport(ExternDll.User32)]
    public static extern uint GetDoubleClickTime();

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern uint GetMessageTime();



    [DllImport(ExternDll.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowText(IntPtr hWnd, string text);
    #endregion 窗体Dll


    #region 壁纸引擎DLL

    [DllImport(ExternDll.User32)]
    internal static extern bool EnumWindows(EnumWindowsProc proc, IntPtr lParam);

    internal delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);


    [DllImport(ExternDll.User32)]
    internal static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlage, uint timeout, IntPtr result);

    #endregion


    #region Hook DLL


    /// <summary>
    /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain. 
    /// You would install a hook procedure to monitor the system for certain types of events. These events 
    /// are associated either with a specific thread or with all threads in the same desktop as the calling thread. 
    /// </summary>
    /// <param name="idHook">
    /// [in] Specifies the type of hook procedure to be installed. This parameter can be one of the following values.
    /// </param>
    /// <param name="lpfn">
    /// [in] Pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a 
    /// thread created by a different process, the lpfn parameter must point to a hook procedure in a dynamic-link 
    /// library (DLL). Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
    /// </param>
    /// <param name="hMod">
    /// [in] Handle to the DLL containing the hook procedure pointed to by the lpfn parameter. 
    /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by 
    /// the current process and if the hook procedure is within the code associated with the current process. 
    /// </param>
    /// <param name="dwThreadId">
    /// [in] Specifies the identifier of the thread with which the hook procedure is to be associated. 
    /// If this parameter is zero, the hook procedure is associated with all existing threads running in the 
    /// same desktop as the calling thread. 
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is the handle to the hook procedure.
    /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
    /// </remarks>
    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);




    /// <summary>
    /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function. 
    /// </summary>
    /// <param name="idHook">
    /// [in] Handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx. 
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
    /// </remarks>
    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern int UnhookWindowsHookEx(int idHook);





    /// <summary>
    /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain. 
    /// A hook procedure can call this function either before or after processing the hook information. 
    /// </summary>
    /// <param name="idHook">Ignored.</param>
    /// <param name="nCode">
    /// [in] Specifies the hook code passed to the current hook procedure. 
    /// The next hook procedure uses this code to determine how to process the hook information.
    /// </param>
    /// <param name="wParam">
    /// [in] Specifies the wParam value passed to the current hook procedure. 
    /// The meaning of this parameter depends on the type of hook associated with the current hook chain. 
    /// </param>
    /// <param name="lParam">
    /// [in] Specifies the lParam value passed to the current hook procedure. 
    /// The meaning of this parameter depends on the type of hook associated with the current hook chain. 
    /// </param>
    /// <returns>
    /// This value is returned by the next hook procedure in the chain. 
    /// The current hook procedure must also return this value. The meaning of the return value depends on the hook type. 
    /// For more information, see the descriptions of the individual hook procedures.
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
    /// </remarks>
    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(nint idHook, nint nCode, nint wParam, IntPtr lParam);




    /// <summary>
    /// The ToAscii function translates the specified virtual-key code and keyboard 
    /// state to the corresponding character or characters. The function translates the code 
    /// using the input language and physical keyboard layout identified by the keyboard layout handle.
    /// </summary>
    /// <param name="uVirtKey">
    /// [in] Specifies the virtual-key code to be translated. 
    /// </param>
    /// <param name="uScanCode">
    /// [in] Specifies the hardware scan code of the key to be translated. 
    /// The high-order bit of this value is set if the key is up (not pressed). 
    /// </param>
    /// <param name="lpbKeyState">
    /// [in] Pointer to a 256-byte array that contains the current keyboard state. 
    /// Each element (byte) in the array contains the state of one key. 
    /// If the high-order bit of a byte is set, the key is down (pressed). 
    /// The low bit, if set, indicates that the key is toggled on. In this function, 
    /// only the toggle bit of the CAPS LOCK key is relevant. The toggle state 
    /// of the NUM LOCK and SCROLL LOCK keys is ignored.
    /// </param>
    /// <param name="lpwTransKey">
    /// [out] Pointer to the buffer that receives the translated character or characters. 
    /// </param>
    /// <param name="fuState">
    /// [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise. 
    /// </param>
    /// <returns>
    /// If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values. 
    /// Value Meaning 
    /// 0 The specified virtual key has no translation for the current state of the keyboard. 
    /// 1 One character was copied to the buffer. 
    /// 2 Two characters were copied to the buffer. This usually happens when a dead-key character 
    /// (accent or diacritic) stored in the keyboard layout cannot be composed with the specified 
    /// virtual key to form a single character. 
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
    /// </remarks>
    [DllImport(ExternDll.User32)]
    internal static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);



    /// <summary>
    /// The GetKeyboardState function copies the status of the 256 virtual keys to the 
    /// specified buffer. 
    /// </summary>
    /// <param name="pbKeyState">
    /// [in] Pointer to a 256-byte array that contains keyboard key states. 
    /// </param>
    /// <returns>
    /// If the function succeeds, the return value is nonzero.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
    /// </returns>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
    /// </remarks>
    [DllImport(ExternDll.User32)]
    internal static extern int GetKeyboardState(byte[] pbKeyState);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    internal static extern short GetKeyState(int vKey);
    #endregion


    #region 内存操作

    [DllImport(ExternDll.NTdll, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr Memcpy(IntPtr dest, IntPtr source, int length);



    [DllImport(ExternDll.Kernel32, SetLastError = true)]
    internal static extern void RtlMoveMemory(IntPtr dest, IntPtr source, int size);


    [DllImport(ExternDll.User32)]
    internal static extern int GetSystemMetrics(int nIndex);


    /// <summary>
    /// 删除句柄
    /// </summary>
    /// <param name="hObject"></param>
    /// <returns></returns>
    [DllImport(ExternDll.Gdi32, EntryPoint = "DeleteObject")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject([In] IntPtr hObject);

    #endregion 内存操作


    #region 系统托盘
    [DllImport(ExternDll.Shell32, CharSet = CharSet.Auto)]
    public static extern int Shell_NotifyIcon(uint message, NOTIFYICONDATA pnid);


    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    [DllImport(ExternDll.User32, CharSet = CharSet.Unicode, SetLastError = true, BestFitMapping = false)]
    public static extern short RegisterClass(WNDCLASS4ICON wc);


    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    [ResourceExposure(ResourceScope.None)]
    public static extern int RegisterWindowMessage(string msg);



    [SecurityCritical]
    [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
    public static extern IntPtr CreateWindowEx(
       int dwExStyle,
       [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
       [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
       int dwStyle,
       int x,
       int y,
       int nWidth,
       int nHeight,
       IntPtr hWndParent,
       IntPtr hMenu,
       IntPtr hInstance,
       IntPtr lpParam);


    [DllImport(ExternDll.User32)]
    internal static extern bool ShowWindowAsync(IntPtr hWnd, SW cmdShow);

    [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);


    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);



    [DllImport(ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);


    [DllImport(ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);


    [DllImport(ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);


    [DllImport(ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out TBBUTTON lpBuffer, int dwSize, out int lpNumberOfBytesRead);


    [DllImport(ExternDll.Kernel32, SetLastError = true)]
    internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out RECT lpBuffer, int dwSize, out int lpNumberOfBytesRead);


    [DllImport(ExternDll.Kernel32, SetLastError = true)]
    internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out TRAYDATA lpBuffer, int dwSize, out int lpNumberOfBytesRead);



    [DllImport(ExternDll.Kernel32, SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern int CloseHandle(IntPtr hObject);


    #endregion 


    #region 控制台DLL


    /// <summary>
    ///     为当前进程分配一个新控制台
    /// </summary>
    /// <returns></returns>
    [DllImport(ExternDll.Kernel32)]
    internal static extern bool AllocConsole();

    /// <summary>
    ///     使调用进程从其控制台分离
    /// </summary>
    /// <returns></returns>
    [DllImport(ExternDll.Kernel32)]
    internal static extern bool FreeConsole();

    /// <summary>
    ///     检索与调用进程相关联的控制台窗口句柄
    /// </summary>
    /// <returns></returns>
    [DllImport(ExternDll.Kernel32)]
    internal static extern IntPtr GetConsoleWindow();

    /// <summary>
    ///     检取与调用进程有关的控制台所用的输出代码页的等价内容，以便将输出函数所写入的内容转换成显示图象
    /// </summary>
    /// <returns></returns>
    [DllImport(ExternDll.Kernel32)]
    internal static extern int GetConsoleOutputCP();


    //AttachConsole是把输出附加到指定控制台上
    //当dwProcessId = -1，表示使用当前进程的父进程控制台；当dwProcessId = pid，表示使用指定进程的控制台
    [DllImport(ExternDll.Kernel32, EntryPoint = "AttachConsole", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern void AttachConsole(int dwProcessId);


    /// <summary>
    ///     添加或删除从调用进程处理函数列表中的应用definedhandlerroutinefunction。如果没有指定的事件处理函数，函数集的可继承的属性，确定是否调用过程忽略了Ctrl + C信号。
    /// </summary>
    /// <param name="handlerRoutine">指向要添加或删除的程序定义HandlerRoutine函数的指针。 此参数可以为NULL。</param>
    /// <param name="add">
    ///     如果这个参数为TRUE，处理程序被添加; 如果是FALSE，则处理程序被删除。如果HandlerRoutine参数为NULL，一个TRUE值会导致调用进程忽略CTRL +
    ///     C输入，以及一个FALSE值恢复CTRL + C输入的正常处理。忽略或处理CTRL + C的此属性由子进程继承。
    /// </param>
    /// <returns>如果函数成功，返回值为非零。如果函数失败，返回值为零。</returns>
    [DllImport(ExternDll.Kernel32)]
    internal static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add);
    #endregion


    #region 菜单DLL

    /// <summary>
    ///     该函数允许应用程序为复制或修改而访问窗口菜单（系统菜单或控制菜单）。
    ///     任何没有用GetSystemMenu函数来生成自己的窗口菜单拷贝的窗口将接受标准窗口菜单。
    ///     窗口菜单最初包含的菜单项有多种标识符值，如SC_CLOSE，SC_MOVE和SC_SIZE。
    ///     窗口菜单上的菜单项发送WM_SYSCOMMAND消息。
    /// </summary>
    /// <param name="hWnd">拥有窗口菜单拷贝的窗口的句柄。</param>
    /// <param name="bRevert">指定将执行的操作。如果此参数为FALSE，GetSystemMenu返回当前使用窗口菜单的拷贝的句柄。该拷贝初始时与窗口菜单相同，但可以被修改。如果此参数为TRUE，GetSystemMenu重置窗口菜单到缺省状态。如果存在先前的窗口菜单，将被销毁。</param>
    /// <returns>如果参数bRevert为FALSE，返回值是窗口菜单的拷贝的句柄：如果参数bRevert为TRUE，返回值是NULL。</returns>
    [DllImport(ExternDll.User32, EntryPoint = "GetSystemMenu")]
    internal static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);


    /// <summary>
    ///     删除指定的菜单项或弹出式菜单
    /// </summary>
    /// <param name="hMenu"></param>
    /// <param name="nPos"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    [DllImport(ExternDll.User32, EntryPoint = "RemoveMenu")]
    internal static extern int RemoveMenu(IntPtr hMenu, int nPos, int flags);

    [DllImport(ExternDll.User32, ExactSpelling = true, EntryPoint = "DestroyMenu", CharSet = CharSet.Auto)]
    [ResourceExposure(ResourceScope.None)]
    public static extern bool IntDestroyMenu(HandleRef hMenu);
    #endregion


    #region 任务栏DLL

    [DllImport(ExternDll.User32)]
    public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
    #endregion 任务栏DLL


    #region 系统休眠DLL
    [DllImport(ExternDll.Kernel32)]
    internal static extern ExecutionState SetThreadExecutionState(ExecutionState flags);

    #endregion 系统休眠DLL


    #region 启动一个外部程序DLL

    //https://blog.csdn.net/jinhuicao/article/details/83573246
    [DllImport(ExternDll.Kernel32)]
    public static extern uint WinExec(string lpCmdLine, WinExecFlag uCmdShow);

    #endregion


    #region 网络DLL


    [DllImport(ExternDll.WinInet)]
    public static extern bool InternetGetConnectedState(ref int flag, int dwReserved);

    #endregion

}

