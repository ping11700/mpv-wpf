
namespace Core.Hook;


/// <summary>
/// 鼠标和键盘Hook
/// </summary>
public class Key_MouseHook
{
    #region Windows structure definitions

    /// <summary>
    /// The POINT structure defines the x- and y- coordinates of a point. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/rectangl_0tiq.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    private class POINT
    {
        /// <summary>
        /// Specifies the x-coordinate of the point. 
        /// </summary>
        public int x;
        /// <summary>
        /// Specifies the y-coordinate of the point. 
        /// </summary>
        public int y;
    }

    /// <summary>
    /// The MOUSEHOOKSTRUCT structure contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    private class MouseHookStruct
    {
        /// <summary>
        /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates. 
        /// </summary>
        public POINT pt;
        /// <summary>
        /// Handle to the window that will receive the mouse message corresponding to the mouse event. 
        /// </summary>
        public int hwnd;
        /// <summary>
        /// Specifies the hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message. 
        /// </summary>
        public int wHitTestCode;
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public int dwExtraInfo;
    }

    /// <summary>
    /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private class MouseLLHookStruct
    {
        /// <summary>
        /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates. 
        /// </summary>
        public POINT pt;
        /// <summary>
        /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
        /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward, 
        /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user. 
        /// One wheel click is defined as WHEEL_DELTA, which is 120. 
        ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
        /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
        /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, mouseData is not used. 
        ///XBUTTON1
        ///The first X button was pressed or released.
        ///XBUTTON2
        ///The second X button was pressed or released.
        /// </summary>
        public int mouseData;
        /// <summary>
        /// Specifies the event-injected flag. An application can use the following value to test the mouse flags. Value Purpose 
        ///LLMHF_INJECTED Test the event-injected flag.  
        ///0
        ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
        ///1-15
        ///Reserved.
        /// </summary>
        public int flags;
        /// <summary>
        /// Specifies the time stamp for this message.
        /// </summary>
        public int time;
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public int dwExtraInfo;
    }


    /// <summary>
    /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    private class KeyboardHookStruct
    {
        /// <summary>
        /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
        /// </summary>
        public int vkCode;
        /// <summary>
        /// Specifies a hardware scan code for the key. 
        /// </summary>
        public int scanCode;
        /// <summary>
        /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
        /// </summary>
        public int flags;
        /// <summary>
        /// Specifies the time stamp for this message.
        /// </summary>
        public int time;
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public int dwExtraInfo;
    }
    #endregion

    #region Windows constants

    //values from Winuser.h in Microsoft SDK.
    /// <summary>
    /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
    /// </summary>
    private const int WH_MOUSE_LL = 14;
    /// <summary>
    /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard  input events.
    /// </summary>
    private const int WH_KEYBOARD_LL = 13;

    /// <summary>
    /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure. 
    /// </summary>
    private const int WH_MOUSE = 7;
    /// <summary>
    /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure. 
    /// </summary>
    private const int WH_KEYBOARD = 2;

    /// <summary>
    /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. 
    /// </summary>
    private const int WM_MOUSEMOVE = 0x200;
    /// <summary>
    /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button 
    /// </summary>
    private const int WM_LBUTTONDOWN = 0x201;
    /// <summary>
    /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button
    /// </summary>
    private const int WM_RBUTTONDOWN = 0x204;
    /// <summary>
    /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button 
    /// </summary>
    private const int WM_MBUTTONDOWN = 0x207;
    /// <summary>
    /// The WM_LBUTTONUP message is posted when the user releases the left mouse button 
    /// </summary>
    private const int WM_LBUTTONUP = 0x202;
    /// <summary>
    /// The WM_RBUTTONUP message is posted when the user releases the right mouse button 
    /// </summary>
    private const int WM_RBUTTONUP = 0x205;
    /// <summary>
    /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button 
    /// </summary>
    private const int WM_MBUTTONUP = 0x208;
    /// <summary>
    /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button 
    /// </summary>
    private const int WM_LBUTTONDBLCLK = 0x203;
    /// <summary>
    /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button 
    /// </summary>
    private const int WM_RBUTTONDBLCLK = 0x206;
    /// <summary>
    /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button 
    /// </summary>
    private const int WM_MBUTTONDBLCLK = 0x209;
    /// <summary>
    /// The WM_MOUSEWHEEL message is posted when the user presses the mouse wheel. 
    /// </summary>
    private const int WM_MOUSEWHEEL = 0x020A;

    /// <summary>
    /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem 
    /// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
    /// </summary>
    private const int WM_KEYDOWN = 0x100;
    /// <summary>
    /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem 
    /// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, 
    /// or a keyboard key that is pressed when a window has the keyboard focus.
    /// </summary>
    private const int WM_KEYUP = 0x101;
    /// <summary>
    /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user 
    /// presses the F10 key (which activates the menu bar) or holds down the ALT key and then 
    /// presses another key. It also occurs when no window currently has the keyboard focus; 
    /// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that 
    /// receives the message can distinguish between these two contexts by checking the context 
    /// code in the lParam parameter. 
    /// </summary>
    private const int WM_SYSKEYDOWN = 0x104;
    /// <summary>
    /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user 
    /// releases a key that was pressed while the ALT key was held down. It also occurs when no 
    /// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent 
    /// to the active window. The window that receives the message can distinguish between 
    /// these two contexts by checking the context code in the lParam parameter. 
    /// </summary>
    private const int WM_SYSKEYUP = 0x105;

    private const byte VK_SHIFT = 0x10;
    private const byte VK_CAPITAL = 0x14;
    private const byte VK_NUMLOCK = 0x90;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates an instance of UserActivityHook object and sets mouse and keyboard hooks.
    /// </summary>
    /// <exception cref="Win32Exception">Any windows problem.</exception>
    public Key_MouseHook()
    {
        new System.Security.Permissions.UIPermission(System.Security.Permissions.UIPermissionWindow.AllWindows).Demand();
    }

    /// <summary>
    /// Creates an instance of UserActivityHook object and installs both or one of mouse and/or keyboard hooks and starts rasing events
    /// </summary>
    /// <param name="installMouseHook"><b>true</b> if mouse events must be monitored</param>
    /// <param name="installKeyboardHook"><b>true</b> if keyboard events must be monitored</param>
    /// <exception cref="Win32Exception">Any windows problem.</exception>
    /// <remarks>
    /// To create an instance without installing hooks call new UserActivityHook(false, false)
    /// </remarks>
    public Key_MouseHook(bool installMouseHook, bool installKeyboardHook)
    {
        Start(installMouseHook, installKeyboardHook);
    }

    /// <summary>
    /// Destruction.
    /// </summary>
    ~Key_MouseHook()
    {
        //uninstall hooks and do not throw exceptions
        Stop(true, true, false);
    }

    #endregion

    #region Variables

    /// <summary>
    /// Custom Mouse Event Handler, Since the WPF version it's way different from the WinForms.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void CustomMouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    /// Custom Key Event Handler, Since the WPF version it's way different from the WinForms.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void CustomKeyEventHandler(object sender, KeyEventArgs e);

    /// <summary>
    /// Custom KeyPress Event Handler, Since the WPF version it's way different from the WinForms.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void CustomKeyPressEventHandler(object sender, KeyPressEventArgs e);

    /// <summary>
    /// Custom KeyUp Event Handler, Since the WPF version it's way different from the WinForms.
    /// </summary>
    /// <param name="sender">Object sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void CustomKeyUpEventHandler(object sender, KeyEventArgs e);

    /// <summary>
    /// Occurs when the user moves the mouse, presses any mouse button or scrolls the wheel
    /// </summary>
    public event CustomMouseEventHandler OnMouseActivity;
    //public event MouseEventHandler OnMouseActivity;

    /// <summary>
    /// Occurs when the user presses a key
    /// </summary>
    public event CustomKeyEventHandler KeyDown;

    ///// <summary>
    ///// Occurs when the user presses and releases 
    ///// </summary>
    public event CustomKeyPressEventHandler KeyPress;

    /// <summary>
    /// Occurs when the user releases a key
    /// </summary>
    public event CustomKeyEventHandler KeyUp;


    /// <summary>
    /// Stores the handle to the mouse hook procedure.
    /// </summary>
    private int hMouseHook = 0;
    /// <summary>
    /// Stores the handle to the keyboard hook procedure.
    /// </summary>
    private int hKeyboardHook = 0;


    /// <summary>
    /// Declare MouseHookProcedure as HookProc type.
    /// </summary>
    private static HookProc MouseHookProcedure;
    /// <summary>
    /// Declare KeyboardHookProcedure as HookProc type.
    /// </summary>
    private static HookProc KeyboardHookProcedure;

    #endregion

    #region Start/Stop

    /// <summary>
    /// Installs both mouse and keyboard hooks and starts rasing events
    /// </summary>
    /// <exception cref="Win32Exception">Any windows problem.</exception>
    public void Start()
    {
        this.Start(true, true);
    }

    /// <summary>
    /// Installs both or one of mouse and/or keyboard hooks and starts rasing events
    /// </summary>
    /// <param name="installMouseHook"><b>true</b> if mouse events must be monitored</param>
    /// <param name="installKeyboardHook"><b>true</b> if keyboard events must be monitored</param>
    /// <exception cref="Win32Exception">Any windows problem.</exception>
    public void Start(bool installMouseHook, bool installKeyboardHook)
    {
        //Gets the system info
        OperatingSystem osInfo = Environment.OSVersion;

        //Install Mouse hook only if it is not installed and must be installed
        if (hMouseHook == 0 && installMouseHook)
        {
            //Create an instance of HookProc.
            MouseHookProcedure = new HookProc(MouseHookProc);

            //XP bug... - Nicke SM
            if (osInfo.Version.Major < 6)
            {
                hMouseHook = NativeAPI.SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            }
            else
            {
                //Install hook
                hMouseHook = NativeAPI.SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, IntPtr.Zero, 0);
            }


            //If SetWindowsHookEx fails.
            if (hMouseHook == 0)
            {
                //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                int errorCode = Marshal.GetLastWin32Error();
                //Do cleanup
                Stop(true, false, false);
                //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                throw new Win32Exception(errorCode);
            }
        }

        //Install Keyboard hook only if it is not installed and must be installed
        if (hKeyboardHook == 0 && installKeyboardHook)
        {
            //Create an instance of HookProc.
            KeyboardHookProcedure = new HookProc(KeyboardHookProc);

            //Install hook
            //XP bug... - Nicke SM
            if (osInfo.Version.Major < 6)
            {
                hKeyboardHook = NativeAPI.SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            }
            else
            {
                hKeyboardHook = NativeAPI.SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, IntPtr.Zero, 0);
            }

            //If SetWindowsHookEx fails.
            if (hKeyboardHook == 0)
            {
                //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                int errorCode = Marshal.GetLastWin32Error();
                //do cleanup
                Stop(false, true, false);
                //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                throw new Win32Exception(errorCode);
            }
        }
    }

    /// <summary>
    /// Stops monitoring both mouse and keyboard events and rasing events.
    /// </summary>
    /// <exception cref="Win32Exception">Any windows problem.</exception>
    public void Stop()
    {
        this.Stop(true, true, true);
    }

    /// <summary>
    /// Stops monitoring both or one of mouse and/or keyboard events and rasing events.
    /// </summary>
    /// <param name="uninstallMouseHook"><b>true</b> if mouse hook must be uninstalled</param>
    /// <param name="uninstallKeyboardHook"><b>true</b> if keyboard hook must be uninstalled</param>
    /// <param name="throwExceptions"><b>true</b> if exceptions which occured during uninstalling must be thrown</param>
    /// <exception cref="Win32Exception">Any windows problem.</exception>
    public void Stop(bool uninstallMouseHook, bool uninstallKeyboardHook, bool throwExceptions)
    {
        //if mouse hook set and must be uninstalled
        if (hMouseHook != 0 && uninstallMouseHook)
        {
            //uninstall hook
            int retMouse = NativeAPI.UnhookWindowsHookEx(hMouseHook);
            //reset invalid handle
            hMouseHook = 0;
            //if failed and exception must be thrown
            if (retMouse == 0 && throwExceptions)
            {
                //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                int errorCode = Marshal.GetLastWin32Error();
                //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                throw new Win32Exception(errorCode);
            }
        }

        //If keyboard hook set and must be uninstalled
        if (hKeyboardHook != 0 && uninstallKeyboardHook)
        {
            //Uninstall hook
            int retKeyboard = NativeAPI.UnhookWindowsHookEx(hKeyboardHook);

            //Reset invalid handle
            hKeyboardHook = 0;

            //If failed and exception must be thrown
            if (retKeyboard == 0 && throwExceptions)
            {
                //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                int errorCode = Marshal.GetLastWin32Error();
                //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                throw new Win32Exception(errorCode);
            }
        }
    }

    #endregion

    #region Event Triggers

    /// <summary>
    /// A callback function which will be called every time a mouse activity detected.
    /// </summary>
    /// <param name="nCode">
    /// [in] Specifies whether the hook procedure must process the message. 
    /// If nCode is HC_ACTION, the hook procedure must process the message. 
    /// If nCode is less than zero, the hook procedure must pass the message to the 
    /// CallNextHookEx function without further processing and must return the 
    /// value returned by CallNextHookEx.
    /// </param>
    /// <param name="wParam">
    /// [in] Specifies whether the message was sent by the current thread. 
    /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
    /// </param>
    /// <param name="lParam">
    /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
    /// </param>
    /// <returns>
    /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
    /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
    /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
    /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
    /// procedure does not call CallNextHookEx, the return value should be zero. 
    /// </returns>
    private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
    {
        //If Ok and someone listens to our events
        if ((nCode >= 0) && (OnMouseActivity != null))
        {
            //Marshall the data from callback.
            var mouseHookStruct = Marshal.PtrToStructure<MouseLLHookStruct>(lParam);

            //Detect button clicked
            var button = MouseButton.XButton1;
            short mouseDelta = 0;

            #region Switch Mouse Actions

            switch (wParam)
            {
                case WM_LBUTTONDOWN:
                    //case WM_LBUTTONUP: 
                    //case WM_LBUTTONDBLCLK: 
                    button = MouseButton.Left;
                    break;
                case WM_RBUTTONDOWN:
                    //case WM_RBUTTONUP: 
                    //case WM_RBUTTONDBLCLK: 
                    button = MouseButton.Right;
                    break;
                case WM_MOUSEWHEEL:
                    //If the message is WM_MOUSEWHEEL, the high-order word of mouseData member is the wheel delta. 
                    //One wheel click is defined as WHEEL_DELTA, which is 120. 
                    //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                    mouseDelta = (short)((mouseHookStruct.mouseData >> 16) & 0xffff);
                    //TODO: X BUTTONS (I havent them so was unable to test)
                    //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                    //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                    //and the low-order word is reserved. This value can be one or more of the following values. 
                    //Otherwise, mouseData is not used. 
                    button = MouseButton.Middle;
                    break;
                case WM_MBUTTONDOWN:
                    //case WM_MBUTTONUP: 
                    //case WM_MBUTTONDBLCLK:
                    button = MouseButton.Middle;
                    break;
                case WM_MOUSEMOVE:
                    break;
                default:
                    return NativeAPI.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
                    //HU3HU3 - A little funny momment: I just frooze my cursor by returning 1 instead of calling the next hook. - Nicke
                    //Congrats to myself. ;D 
                    //05:24 AM 01/02/2014 (day-month-year)
            }

            #endregion

            //Double clicks
            int clickCount = (wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDBLCLK) ? 2 : 1;

            //Generate event 
            var e = new MouseEventArgs(button, clickCount, mouseHookStruct.pt.x, mouseHookStruct.pt.y, mouseDelta);

            //Raise it
            OnMouseActivity?.Invoke(this, e);
        }

        //Call next hook
        return NativeAPI.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
    }

    /// <summary>
    /// A callback function which will be called every time a keyboard activity detected.
    /// </summary>
    /// <param name="nCode">
    /// [in] Specifies whether the hook procedure must process the message. 
    /// If nCode is HC_ACTION, the hook procedure must process the message. 
    /// If nCode is less than zero, the hook procedure must pass the message to the 
    /// CallNextHookEx function without further processing and must return the 
    /// value returned by CallNextHookEx.
    /// </param>
    /// <param name="wParam">
    /// [in] Specifies whether the message was sent by the current thread. 
    /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
    /// </param>
    /// <param name="lParam">
    /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
    /// </param>
    /// <returns>
    /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
    /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
    /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
    /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
    /// procedure does not call CallNextHookEx, the return value should be zero. 
    /// </returns>
    private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
    {
        //Indicates if any of underlaing events set e.Handled flag
        bool handled = false;

        //If was Ok and someone listens to events
        if ((nCode >= 0) && (KeyDown != null || KeyUp != null || KeyPress != null))
        {
            //Read structure KeyboardHookStruct at lParam
            var myKeyboardHookStruct = Marshal.PtrToStructure<KeyboardHookStruct>(lParam);

            if (KeyDown != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                #region Raise KeyDown

                var keyData = (Keys)myKeyboardHookStruct.vkCode;
                var e = new KeyEventArgs(keyData);
                KeyDown?.Invoke(this, e);

                handled = handled || e.Handled;

                #endregion
            }

            if (KeyPress != null && wParam == WM_KEYDOWN)
            {
                #region Raise KeyPress

                bool isDownShift = ((NativeAPI.GetKeyState(VK_SHIFT) & 0x80) == 0x80);
                bool isDownCapslock = (NativeAPI.GetKeyState(VK_CAPITAL) != 0);

                var keyState = new byte[256];
                NativeAPI.GetKeyboardState(keyState);
                var inBuffer = new byte[2];

                if (NativeAPI.ToAscii(myKeyboardHookStruct.vkCode, myKeyboardHookStruct.scanCode, keyState, inBuffer, myKeyboardHookStruct.flags) == 1)
                {
                    var key = (char)inBuffer[0];
                    if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key))
                        key = Char.ToUpper(key);

                    var e = new KeyPressEventArgs(key);
                    KeyPress?.Invoke(this, e);

                    handled = handled || e.Handled;
                }

                #endregion
            }

            if (KeyUp != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
            {
                #region Raise KeyUp

                var keyData = (Keys)myKeyboardHookStruct.vkCode;
                var e = new KeyEventArgs(keyData);
                KeyUp?.Invoke(this, e);

                handled = handled || e.Handled;

                #endregion
            }
        }

        //If event handled in application do not handoff to other listeners
        if (handled)
            return 1;

        return NativeAPI.CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
    }

    #endregion
}