namespace Core.WindowsAPI;

public class InteropValues
{

    //https://www.cnblogs.com/code1992/p/11239881.html wMsg参数常量值：
    public const int
    //创建一个窗口  
    WM_CREATE = 0x01,
    //当一个窗口被破坏时发送  
    WM_DESTROY = 0x02,
    //移动一个窗口  
    WM_MOVE = 0x03,
    //改变一个窗口的大小  
    WM_SIZE = 0x05,
    //
    WM_SIZING = 0x0214,
    //一个窗口被激活或失去激活状态  
    WM_ACTIVATE = 0x06,
    //一个窗口获得焦点  
    WM_SETFOCUS = 0x07,
    //一个窗口失去焦点  
    WM_KILLFOCUS = 0x08,
    //一个窗口改变成Enable状态  
    WM_ENABLE = 0x0A,
    //设置窗口是否能重画  
    WM_SETREDRAW = 0x0B,
    //应用程序发送此消息来设置一个窗口的文本  
    WM_SETTEXT = 0x000C,
    //应用程序发送此消息来复制对应窗口的文本到缓冲区  
    WM_GETTEXT = 0x0D,
    //得到与一个窗口有关的文本的长度（不包含空字符）  
    WM_GETTEXTLENGTH = 0x0E,
    //要求一个窗口重画自己  
    WM_PAINT = 0x0F,
    //当一个窗口或应用程序要关闭时发送一个信号  
    WM_CLOSE = 0x10,
    //当用户选择结束对话框或程序自己调用ExitWindows函数  
    WM_QUERYENDSESSION = 0x11,
    //用来结束程序运行  
    WM_QUIT = 0x12,
    //当用户窗口恢复以前的大小位置时，把此消息发送给某个图标  
    WM_QUERYOPEN = 0x13,
    //当窗口背景必须被擦除时（例在窗口改变大小时）  
    WM_ERASEBKGND = 0x14,
    //当系统颜色改变时，发送此消息给所有顶级窗口  
    WM_SYSCOLORCHANGE = 0x15,
    //当系统进程发出WM_QUERYENDSESSION消息后，此消息发送给应用程序，通知它对话是否结束  
    WM_ENDSESSION = 0x16,
    //当隐藏或显示窗口是发送此消息给这个窗口  
    WM_SHOWWINDOW = 0x18,
    //发此消息给应用程序哪个窗口是激活的，哪个是非激活的  
    WM_ACTIVATEAPP = 0x1C,
    //当系统的字体资源库变化时发送此消息给所有顶级窗口  
    WM_FONTCHANGE = 0x1D,
    //当系统的时间变化时发送此消息给所有顶级窗口  
    WM_TIMECHANGE = 0x1E,
    //发送此消息来取消某种正在进行的摸态（操作）  
    WM_CANCELMODE = 0x1F,
    //如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，就发消息给某个窗口  
    WM_SETCURSOR = 0x20,
    //当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给//当前窗口  
    WM_MOUSEACTIVATE = 0x21,
    //发送此消息给MDI子窗口//当用户点击此窗口的标题栏，或//当窗口被激活，移动，改变大小  
    WM_CHILDACTIVATE = 0x22,
    //此消息由基于计算机的训练程序发送，通过WH_JOURNALPALYBACK的hook程序分离出用户输入消息  
    WM_QUEUESYNC = 0x23,
    //此消息发送给窗口当它将要改变大小或位置  
    WM_GETMINMAXINFO = 0x24,
    //发送给最小化窗口当它图标将要被重画  
    WM_PAINTICON = 0x26,
    //此消息发送给某个最小化窗口，仅//当它在画图标前它的背景必须被重画  
    WM_ICONERASEBKGND = 0x27,
    //发送此消息给一个对话框程序去更改焦点位置  
    WM_NEXTDLGCTL = 0x28,
    //每当打印管理列队增加或减少一条作业时发出此消息   
    WM_SPOOLERSTATUS = 0x2A,
    //当button，combobox，listbox，menu的可视外观改变时发送  
    WM_DRAWITEM = 0x2B,
    //当button, combo box, list box, list view control, or menu item 被创建时  
    WM_MEASUREITEM = 0x2C,
    //此消息有一个LBS_WANTKEYBOARDINPUT风格的发出给它的所有者来响应WM_KEYDOWN消息   
    WM_VKEYTOITEM = 0x2E,
    //此消息由一个LBS_WANTKEYBOARDINPUT风格的列表框发送给他的所有者来响应WM_CHAR消息   
    WM_CHARTOITEM = 0x2F,
    //当绘制文本时程序发送此消息得到控件要用的颜色  
    WM_SETFONT = 0x30,
    //应用程序发送此消息得到当前控件绘制文本的字体  
    WM_GETFONT = 0x31,
    //应用程序发送此消息让一个窗口与一个热键相关连   
    WM_SETHOTKEY = 0x32,
    //应用程序发送此消息来判断热键与某个窗口是否有关联  
    WM_GETHOTKEY = 0x33,
    //此消息发送给最小化窗口，当此窗口将要被拖放而它的类中没有定义图标，应用程序能返回一个图标或光标的句柄，当用户拖放图标时系统显示这个图标或光标  
    WM_QUERYDRAGICON = 0x37,
    //发送此消息来判定combobox或listbox新增加的项的相对位置  
    WM_COMPAREITEM = 0x39,
    //显示内存已经很少了  
    WM_COMPACTING = 0x41,
    //发送此消息给那个窗口的大小和位置将要被改变时，来调用setwindowpos函数或其它窗口管理函数  
    WM_WINDOWPOSCHANGING = 0x46,
    //发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数  
    WM_WINDOWPOSCHANGED = 0x47,
    //当系统将要进入暂停状态时发送此消息  
    WM_POWER = 0x48,
    //当一个应用程序传递数据给另一个应用程序时发送此消息  
    WM_COPYDATA = 0x4A,
    //当某个用户取消程序日志激活状态，提交此消息给程序  
    WM_CANCELJOURNA = 0x4B,
    //当某个控件的某个事件已经发生或这个控件需要得到一些信息时，发送此消息给它的父窗口   
    WM_NOTIFY = 0x4E,
    //当用户选择某种输入语言，或输入语言的热键改变  
    WM_INPUTLANGCHANGEREQUEST = 0x50,
    //当平台现场已经被改变后发送此消息给受影响的最顶级窗口  
    WM_INPUTLANGCHANGE = 0x51,
    //当程序已经初始化windows帮助例程时发送此消息给应用程序  
    WM_TCARD = 0x52,
    //此消息显示用户按下了F1，如果某个菜单是激活的，就发送此消息个此窗口关联的菜单，否则就发送给有焦点的窗口，如果//当前都没有焦点，就把此消息发送给//当前激活的窗口  
    WM_HELP = 0x53,
    //当用户已经登入或退出后发送此消息给所有的窗口，//当用户登入或退出时系统更新用户的具体设置信息，在用户更新设置时系统马上发送此消息  
    WM_USERCHANGED = 0x54,
    //公用控件，自定义控件和他们的父窗口通过此消息来判断控件是使用ANSI还是UNICODE结构  
    WM_NOTIFYFORMAT = 0x55,
    //当用户某个窗口中点击了一下右键就发送此消息给这个窗口  
    //  WM_CONTEXTMENU = ??,  
    //当调用SETWINDOWLONG函数将要改变一个或多个 窗口的风格时发送此消息给那个窗口  
    WM_STYLECHANGING = 0x7C,
    //当调用SETWINDOWLONG函数一个或多个 窗口的风格后发送此消息给那个窗口  
    WM_STYLECHANGED = 0x7D,
    //当显示器的分辨率改变后发送此消息给所有的窗口  
    WM_DISPLAYCHANGE = 0x7E,
    //此消息发送给某个窗口来返回与某个窗口有关连的大图标或小图标的句柄  
    WM_GETICON = 0x7F,
    //程序发送此消息让一个新的大图标或小图标与某个窗口关联  
    WM_SETICON = 0x80,
    //当某个窗口第一次被创建时，此消息在WM_CREATE消息发送前发送  
    WM_NCCREATE = 0x81,
    //此消息通知某个窗口，非客户区正在销毁   
    WM_NCDESTROY = 0x82,
    //当某个窗口的客户区域必须被核算时发送此消息  
    WM_NCCALCSIZE = 0x83,
    //移动鼠标，按住或释放鼠标时发生  
    WM_NCHITTEST = 0x84,
    //程序发送此消息给某个窗口当它（窗口）的框架必须被绘制时  
    WM_NCPAINT = 0x85,
    //此消息发送给某个窗口仅当它的非客户区需要被改变来显示是激活还是非激活状态  
    WM_NCACTIVATE = 0x86,
    //发送此消息给某个与对话框程序关联的控件，widdows控制方位键和TAB键使输入进入此控件通过应  
    WM_GETDLGCODE = 0x87,
    //当光标在一个窗口的非客户区内移动时发送此消息给这个窗口 非客户区为：窗体的标题栏及窗 的边框体  
    WM_NCMOUSEMOVE = 0xA0,
    //当光标在一个窗口的非客户区同时按下鼠标左键时提交此消息  
    WM_NCLBUTTONDOWN = 0xA1,
    //当用户释放鼠标左键同时光标某个窗口在非客户区十发送此消息   
    WM_NCLBUTTONUP = 0xA2,
    //当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息  
    WM_NCLBUTTONDBLCLK = 0xA3,
    //当用户按下鼠标右键同时光标又在窗口的非客户区时发送此消息  
    WM_NCRBUTTONDOWN = 0xA4,
    //当用户释放鼠标右键同时光标又在窗口的非客户区时发送此消息  
    WM_NCRBUTTONUP = 0xA5,
    //当用户双击鼠标右键同时光标某个窗口在非客户区十发送此消息  
    WM_NCRBUTTONDBLCLK = 0xA6,
    //当用户按下鼠标中键同时光标又在窗口的非客户区时发送此消息  
    WM_NCMBUTTONDOWN = 0xA7,
    //当用户释放鼠标中键同时光标又在窗口的非客户区时发送此消息  
    WM_NCMBUTTONUP = 0xA8,
    //当用户双击鼠标中键同时光标又在窗口的非客户区时发送此消息  
    WM_NCMBUTTONDBLCLK = 0xA9,
    //WM_KEYDOWN 按下一个键  
    WM_KEYDOWN = 0x0100,
    //释放一个键  
    WM_KEYUP = 0x0101,
    //按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息  
    WM_CHAR = 0x102,
    //当用translatemessage函数翻译WM_KEYUP消息时发送此消息给拥有焦点的窗口  
    WM_DEADCHAR = 0x103,
    //当用户按住ALT键同时按下其它键时提交此消息给拥有焦点的窗口  
    WM_SYSKEYDOWN = 0x104,
    //当用户释放一个键同时ALT 键还按着时提交此消息给拥有焦点的窗口  
    WM_SYSKEYUP = 0x105,
    //当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后提交此消息给拥有焦点的窗口  
    WM_SYSCHAR = 0x106,
    //当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后发送此消息给拥有焦点的窗口  
    WM_SYSDEADCHAR = 0x107,
    //在一个对话框程序被显示前发送此消息给它，通常用此消息初始化控件和执行其它任务  
    WM_INITDIALOG = 0x110,
    //当用户选择一条菜单命令项或当某个控件发送一条消息给它的父窗口，一个快捷键被翻译  
    WM_COMMAND = 0x111,
    //当用户选择窗口菜单的一条命令或//当用户选择最大化或最小化时那个窗口会收到此消息  
    WM_SYSCOMMAND = 0x112,
    //发生了定时器事件  
    WM_TIMER = 0x113,
    //当一个窗口标准水平滚动条产生一个滚动事件时发送此消息给那个窗口，也发送给拥有它的控件  
    WM_HSCROLL = 0x114,
    //当一个窗口标准垂直滚动条产生一个滚动事件时发送此消息给那个窗口也，发送给拥有它的控件  
    WM_VSCROLL = 0x115,
    //当一个菜单将要被激活时发送此消息，它发生在用户菜单条中的某项或按下某个菜单键，它允许程序在显示前更改菜单  
    WM_INITMENU = 0x116,
    //当一个下拉菜单或子菜单将要被激活时发送此消息，它允许程序在它显示前更改菜单，而不要改变全部  
    WM_INITMENUPOPUP = 0x117,
    //当用户选择一条菜单项时发送此消息给菜单的所有者（一般是窗口）  
    WM_MENUSELECT = 0x11F,
    //当菜单已被激活用户按下了某个键（不同于加速键），发送此消息给菜单的所有者  
    WM_MENUCHAR = 0x120,
    //当一个模态对话框或菜单进入空载状态时发送此消息给它的所有者，一个模态对话框或菜单进入空载状态就是在处理完一条或几条先前的消息后没有消息它的列队中等待  
    WM_ENTERIDLE = 0x121,
    //在windows绘制消息框前发送此消息给消息框的所有者窗口，通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置消息框的文本和背景颜色  
    WM_CTLCOLORMSGBOX = 0x132,
    //当一个编辑型控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置编辑框的文本和背景颜色  
    WM_CTLCOLOREDIT = 0x133,

    //当一个列表框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置列表框的文本和背景颜色  
    WM_CTLCOLORLISTBOX = 0x134,
    //当一个按钮控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置按纽的文本和背景颜色  
    WM_CTLCOLORBTN = 0x135,
    //当一个对话框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置对话框的文本背景颜色  
    WM_CTLCOLORDLG = 0x136,
    //当一个滚动条控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置滚动条的背景颜色  
    WM_CTLCOLORSCROLLBAR = 0x137,
    //当一个静态控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以 通过使用给定的相关显示设备的句柄来设置静态控件的文本和背景颜色  
    WM_CTLCOLORSTATIC = 0x138,
    //当鼠标轮子转动时发送此消息个当前有焦点的控件  
    WM_MOUSEWHEEL = 0x20A,
    //双击鼠标中键  
    WM_MBUTTONDBLCLK = 0x209,
    //释放鼠标中键  
    WM_MBUTTONUP = 0x208,
    //移动鼠标时发生，同WM_MOUSEFIRST  
    WM_MOUSEMOVE = 0x200,
    //按下鼠标左键  
    WM_LBUTTONDOWN = 0x201,
    //释放鼠标左键  
    WM_LBUTTONUP = 0x202,
    //双击鼠标左键  
    WM_LBUTTONDBLCLK = 0x203,
    //按下鼠标右键
    WM_RBUTTONDOWN = 0x204,
    //释放鼠标右键  
    WM_RBUTTONUP = 0x205,
    //双击鼠标右键  
    WM_RBUTTONDBLCLK = 0x206,
    //按下鼠标中键  
    WM_MBUTTONDOWN = 0x207,

    WM_SETTINGCHANGE = 0x001A,

    WM_USER = 0x0400,
    MK_LBUTTON = 0x0001,
    MK_RBUTTON = 0x0002,
    MK_SHIFT = 0x0004,
    MK_CONTROL = 0x0008,
    MK_MBUTTON = 0x0010,
    MK_XBUTTON1 = 0x0020,
    MK_XBUTTON2 = 0x0040;


    public const uint
      BITSPIXEL = 12,
      PLANES = 14,
      BI_RGB = 0,
      SIZE_MAXIMIZED = 0x02,
      DIB_RGB_COLORS = 0,
      E_FAIL = unchecked(0x80004005),
      HWND_TOP = 0,
      NIF_MESSAGE = 0x00000001,
      NIF_ICON = 0x00000002,
      NIF_TIP = 0x00000004,
      NIF_INFO = 0x00000010,
      NIM_ADD = 0x00000000,
      NIM_MODIFY = 0x00000001,
      NIM_DELETE = 0x00000002,
      NIIF_NONE = 0x00000000,
      NIIF_INFO = 0x00000001,
      NIIF_WARNING = 0x00000002,
      NIIF_ERROR = 0x00000003,

      WM_NCUAHDRAWCAPTION = 0x00AE,
      WM_NCUAHDRAWFRAME = 0x00AF,
      WM_ENTERSIZEMOVE = 0x0231,
      WM_EXITSIZEMOVE = 0x0232,
      WM_CLIPBOARDUPDATE = 0x031D,
      WS_VISIBLE = 0x10000000,
      MF_BYCOMMAND = 0x00000000,
      MF_BYPOSITION = 0x400,
      MF_GRAYED = 0x00000001,
      MF_SEPARATOR = 0x800,


      NIN_BALLOONUSERCLICK = WM_USER + 5,
      TB_GETBUTTON = WM_USER + 23,
      TB_BUTTONCOUNT = WM_USER + 24,
      TB_GETITEMRECT = WM_USER + 29,


      VERTRES = 10,
      DESKTOPVERTRES = 117,
      LOGPIXELSX = 88,
      LOGPIXELSY = 90,
      SC_CLOSE = 0xF060,
      SC_SIZE = 0xF000,
      SC_MOVE = 0xF010,
      SC_MINIMIZE = 0xF020,
      SC_MAXIMIZE = 0xF030,
      SC_RESTORE = 0xF120,
      SRCCOPY = 0x00CC0020,
      MONITOR_DEFAULTTOPRIMARY = 0x00000001,
      MONITOR_DEFAULTTONEAREST = 0x00000002;


    public const int
          GWL_STYLE = -16,
          GWL_EXSTYLE = -20,
          VK_SHIFT = 0x10,
          VK_CONTROL = 0x11,
          VK_MENU = 0x12,
          WH_CALLWNDPROCRET = 12,
          WH_KEYBOARD_LL = 13,
          WH_MOUSE = 7;

    public const int WS_EX_NOACTIVATE = 0x08000000;

    public const uint
        WS_CHILD = 0x40000000,
        WS_CLIPCHILDREN = 0x02000000,

        SWP_FRAMECHANGED = 0x0020,
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOACTIVATE = 0x10,
        SWP_NOCOPYBITS = 0x0100,
        SWP_ASYNCWINDOWPOS = 16384,
        SWP_HIDEWINDOW = 0x0080,
        SWP_SHOWWINDOW = 0x0040,
        SWP_NOREPOSITION = 0x0200,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOSENDCHANGING = 0x0400,
        WM_KEYFIRST = 0x0100,
        WM_KEYLAST = 0x0108,
        WM_IME_STARTCOMPOSITION = 0x010D,
        WM_IME_ENDCOMPOSITION = 0x010E,
        WM_IME_COMPOSITION = 0x010F,
        WM_IME_KEYLAST = 0x010F,
        WS_OVERLAPPED = 0x00000000,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4;



    public const int
    SWP_NOREDRAW = 0x0008,
    GWL_HWNDPARENT = -8,
    SW_HIDE = 0,
    SW_SHOWNA = 8,
    SW_SHOWNOACTIVATE = 0x0004;




}


public static class ExternDll
{
    public const string
        User32 = "user32.dll",
        Gdi32 = "gdi32.dll",
        GdiPlus = "gdiplus.dll",
        Kernel32 = "kernel32.dll",
        Shell32 = "shell32.dll",
        MsImg = "msimg32.dll",
        NTdll = "ntdll.dll",
        DwmApi = "dwmapi.dll",
        SHCore = "SHCore.dll",
        WinInet = "winInet.dll",
        IPhlpapi = "iphlpapi.dll",
        Urlmon = "urlmon.dll";

}

/// <summary>
/// The CallWndProc hook procedure is an application-defined or library-defined callback 
/// function used with the SetWindowsHookEx function. The HOOKPROC type defines a pointer 
/// to this callback function. CallWndProc is a placeholder for the application-defined 
/// or library-defined function name.
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
/// <remarks>
/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/callwndproc.asp
/// </remarks>
public delegate int HookProc(int nCode, int wParam, IntPtr lParam);



public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

/// <summary>
/// Provide the win32 window stype when call <see cref="CreateWindowEx"/>
/// <para>See following link: <a href="https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles" /> </para>
/// </summary>
[Flags]
public enum ExtendedWindow32Styles : int
{
    /// <summary>
    /// The window should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
    /// The window appears transparent because the bits of underlying sibling windows have already been painted.
    /// </summary>
    WS_EX_TRANSPARENT = 0x00000020
}

/// <summary>
/// Provide the win32 window stype when call <see cref="CreateWindowEx"/>
/// <para>See following link: <a href="https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles" /> </para>
/// </summary>
[Flags]
public enum Window32Styles : int
{
    /// <summary>
    /// The window is a child window. A window with this style cannot have a menu bar.
    /// </summary>
    WS_CHILD = 0x40000000,

    /// <summary>
    /// The window is initially visible.
    /// </summary>
    WS_VISIBLE = 0x10000000
}



public struct WINCOMPATTRDATA
{
    public WINDOWCOMPOSITIONATTRIB Attribute;
    public IntPtr Data;
    public int DataSize;
}

public enum WINDOWCOMPOSITIONATTRIB
{
    WCA_ACCENT_POLICY = 19
}

[StructLayout(LayoutKind.Sequential)]
public struct RTL_OSVERSIONINFOEX
{
    public uint dwOSVersionInfoSize;
    public uint dwMajorVersion;
    public uint dwMinorVersion;
    public uint dwBuildNumber;
    public uint dwPlatformId;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string szCSDVersion;
}

public class StreamConsts
{
    public const int LOCK_WRITE = 0x1;
    public const int LOCK_EXCLUSIVE = 0x2;
    public const int LOCK_ONLYONCE = 0x4;
    public const int STATFLAG_DEFAULT = 0x0;
    public const int STATFLAG_NONAME = 0x1;
    public const int STATFLAG_NOOPEN = 0x2;
    public const int STGC_DEFAULT = 0x0;
    public const int STGC_OVERWRITE = 0x1;
    public const int STGC_ONLYIFCURRENT = 0x2;
    public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 0x4;
    public const int STREAM_SEEK_SET = 0x0;
    public const int STREAM_SEEK_CUR = 0x1;
    public const int STREAM_SEEK_END = 0x2;
}


[ComImport, Guid("0000000C-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IStream
{
    int Read([In] IntPtr buf, [In] int len);

    int Write([In] IntPtr buf, [In] int len);

    [return: MarshalAs(UnmanagedType.I8)]
    long Seek([In, MarshalAs(UnmanagedType.I8)] long dlibMove, [In] int dwOrigin);

    void SetSize([In, MarshalAs(UnmanagedType.I8)] long libNewSize);

    [return: MarshalAs(UnmanagedType.I8)]
    long CopyTo([In, MarshalAs(UnmanagedType.Interface)] IStream pstm, [In, MarshalAs(UnmanagedType.I8)] long cb, [Out, MarshalAs(UnmanagedType.LPArray)] long[] pcbRead);

    void Commit([In] int grfCommitFlags);

    void Revert();

    void LockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In] int dwLockType);

    void UnlockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In] int dwLockType);

    void Stat([In] IntPtr pStatstg, [In] int grfStatFlag);

    [return: MarshalAs(UnmanagedType.Interface)]
    IStream Clone();
}



public struct MONITORINFO
{
    public uint cbSize;
    public RECT rcMonitor;
    public RECT rcWork;
    public uint dwFlags;
}


[StructLayout(LayoutKind.Sequential)]
public struct WINDOWPLACEMENT
{
    public int length;
    public int flags;
    public SW showCmd;
    public POINT ptMinPosition;
    public POINT ptMaxPosition;
    public RECT rcNormalPosition;

    /// <summary>
    /// Gets the default (empty) value.
    /// </summary>
    public static WINDOWPLACEMENT Default
    {
        get
        {
            WINDOWPLACEMENT result = new WINDOWPLACEMENT();
            result.length = Marshal.SizeOf(result);
            return result;
        }
    }
}

/// <summary>
/// ShowWindow options
/// </summary>
public enum SW
{
    HIDE = 0,
    SHOWNORMAL = 1,
    SHOWMINIMIZED = 2,

    SHOWMAXIMIZED = 3,
    MAXIMIZE = 3,

    SHOWNOACTIVATE = 4,
    SHOW = 5,
    MINIMIZE = 6,
    SHOWMINNOACTIVE = 7,
    SHOWNA = 8,
    RESTORE = 9,
    SHOWDEFAULT = 10,
    FORCEMINIMIZE = 11,
}


/// <summary>
/// ShowWindow options
/// //https://docs.microsoft.com/zh-cn/windows/win32/winmsg/wm-showwindow
/// </summary>
public enum WM_SHOWWINDOW
{
    SW_PARENTCLOSING = 1,
    SW_OTHERZOOM = 2,
    SW_PARENTOPENING = 3,
    SW_OTHERUNZOOM = 4,
}

[StructLayout(LayoutKind.Sequential)]
public struct WindowPosition
{
    public IntPtr Hwnd;
    public IntPtr HwndZOrderInsertAfter;
    public int X;
    public int Y;
    public int Width;
    public int Height;
    public uint Flags;
}


[Flags]
public enum WindowStyles
{
    /// <summary>
    ///     The window is initially maximized.
    /// </summary>
    WS_MAXIMIZE = 0x01000000,

    /// <summary>
    ///     The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must
    ///     also be specified.
    /// </summary>
    WS_MAXIMIZEBOX = 0x00010000,

    /// <summary>
    ///     The window is initially minimized. Same as the WS_ICONIC style.
    /// </summary>
    WS_MINIMIZE = 0x20000000,

    /// <summary>
    ///     The window has a sizing border. Same as the WS_SIZEBOX style.
    /// </summary>
    WS_THICKFRAME = 0x00040000,
}


[Flags]
public enum DwmWindowAttribute : uint
{
    DWMWA_NCRENDERING_ENABLED = 1,
    DWMWA_NCRENDERING_POLICY,
    DWMWA_TRANSITIONS_FORCEDISABLED,
    DWMWA_ALLOW_NCPAINT,
    DWMWA_CAPTION_BUTTON_BOUNDS,
    DWMWA_NONCLIENT_RTL_LAYOUT,
    DWMWA_FORCE_ICONIC_REPRESENTATION,
    DWMWA_FLIP3D_POLICY,
    DWMWA_EXTENDED_FRAME_BOUNDS,
    DWMWA_HAS_ICONIC_BITMAP,
    DWMWA_DISALLOW_PEEK,
    DWMWA_EXCLUDED_FROM_PEEK,
    DWMWA_LAST
}

[Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public RECT(Rect rect)
    {
        Left = (int)rect.Left;
        Top = (int)rect.Top;
        Right = (int)rect.Right;
        Bottom = (int)rect.Bottom;
    }

    public Point Position => new(Left, Top);
    public Size Size => new(Width, Height);

    public int Height
    {
        get => Bottom - Top;
        set => Bottom = Top + value;
    }

    public int Width
    {
        get => Right - Left;
        set => Right = Left + value;
    }

    public bool Equals(RECT other)
    {
        return Left == other.Left && Right == other.Right && Top == other.Top && Bottom == other.Bottom;
    }

    public override bool Equals(object obj)
    {
        return obj is RECT rectangle && Equals(rectangle);
    }

    public static bool operator ==(RECT left, RECT right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RECT left, RECT right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Left;
            hashCode = (hashCode * 397) ^ Top;
            hashCode = (hashCode * 397) ^ Right;
            hashCode = (hashCode * 397) ^ Bottom;
            return hashCode;
        }
    }
}

[Flags]
public enum MonitorFromWindowFlags
{
    MONITOR_DEFAULTTONULL = 0x00000000,
    MONITOR_DEFAULTTOPRIMARY = 0x00000001,
    MONITOR_DEFAULTTONEAREST = 0x00000002,
}


public enum MonitorDpiTypes
{
    EffectiveDPI = 0,
    AngularDPI = 1,
    RawDPI = 2,
}

public enum PROCESS_DPI_AWARENESS
{
    PROCESS_DPI_UNAWARE = 0,
    PROCESS_SYSTEM_DPI_AWARE = 1,
    PROCESS_PER_MONITOR_DPI_AWARE = 2,
}

public enum DeviceCaps
{
    /// <summary>
    /// Logical pixels inch in X
    /// </summary>
    LOGPIXELSX = 88,

    /// <summary>
    /// Horizontal width in pixels
    /// </summary>
    HORZRES = 8,

    /// <summary>
    /// Horizontal width of entire desktop in pixels
    /// </summary>
    DESKTOPHORZRES = 118
}



#region 系统托盘
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class NOTIFYICONDATA
{
    public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
    public IntPtr hWnd;
    public int uID;
    public uint uFlags;
    public int uCallbackMessage;
    public IntPtr hIcon;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string szTip = string.Empty;
    public int dwState = 0x01;
    public int dwStateMask = 0x01;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string szInfo = string.Empty;
    public int uTimeoutOrVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string szInfoTitle = string.Empty;
    public uint dwInfoFlags;
}


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class WNDCLASS4ICON
{
    public int style;
    public WndProc lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public IntPtr hInstance;
    public IntPtr hIcon;
    public IntPtr hCursor;
    public IntPtr hbrBackground;
    public string lpszMenuName;
    public string lpszClassName;
}


[Flags]
public enum ProcessAccess
{
    AllAccess = CreateThread | DuplicateHandle | QueryInformation | SetInformation | Terminate | VMOperation | VMRead | VMWrite | Synchronize,
    CreateThread = 0x2,
    DuplicateHandle = 0x40,
    QueryInformation = 0x400,
    SetInformation = 0x200,
    Terminate = 0x1,
    VMOperation = 0x8,
    VMRead = 0x10,
    VMWrite = 0x20,
    Synchronize = 0x100000
}


[Flags]
public enum AllocationType
{
    Commit = 0x1000,
    Reserve = 0x2000,
    Decommit = 0x4000,
    Release = 0x8000,
    Reset = 0x80000,
    Physical = 0x400000,
    TopDown = 0x100000,
    WriteWatch = 0x200000,
    LargePages = 0x20000000
}

[Flags]
public enum MemoryProtection
{
    Execute = 0x10,
    ExecuteRead = 0x20,
    ExecuteReadWrite = 0x40,
    ExecuteWriteCopy = 0x80,
    NoAccess = 0x01,
    ReadOnly = 0x02,
    ReadWrite = 0x04,
    WriteCopy = 0x08,
    GuardModifierflag = 0x100,
    NoCacheModifierflag = 0x200,
    WriteCombineModifierflag = 0x400
}





[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TBBUTTON
{
    public int iBitmap;
    public int idCommand;
    public IntPtr fsStateStylePadding;
    public IntPtr dwData;
    public IntPtr iString;
}


[StructLayout(LayoutKind.Sequential)]
public struct TRAYDATA
{
    public IntPtr hwnd;
    public uint uID;
    public uint uCallbackMessage;
    public uint bReserved0;
    public uint bReserved1;
    public IntPtr hIcon;
}


[Flags]
public enum FreeType
{
    Decommit = 0x4000,
    Release = 0x8000,
}
#endregion

#region 任务栏Dll 数据结构

[StructLayout(LayoutKind.Sequential)]
public struct APPBARDATA
{
    public int cbSize;
    public IntPtr hWnd;
    public int uCallbackMessage;
    public int uEdge;
    public RECT rc;
    public bool lParam;
}






public enum ABEdge
{
    ABE_LEFT = 0,
    ABE_TOP = 1,
    ABE_RIGHT = 2,
    ABE_BOTTOM = 3
}

public enum ABMsg
{
    ABM_NEW = 0,
    ABM_REMOVE = 1,
    ABM_QUERYPOS = 2,
    ABM_SETPOS = 3,
    ABM_GETSTATE = 4,
    ABM_GETTASKBARPOS = 5,
    ABM_ACTIVATE = 6,
    ABM_GETAUTOHIDEBAR = 7,
    ABM_SETAUTOHIDEBAR = 8,
    ABM_WINDOWPOSCHANGED = 9,
    ABM_SETSTATE = 10
}



[StructLayout(LayoutKind.Sequential)]
public struct MINMAXINFO
{
    public POINT ptReserved;
    public POINT ptMaxSize;
    public POINT ptMaxPosition;
    public POINT ptMinTrackSize;
    public POINT ptMaxTrackSize;
};



[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;

    public POINT(int x, int y)
    {
        X = x;
        Y = y;
    }
}
#endregion

#region 系统休眠Dll 数据结构

[Flags]
public enum ExecutionState : uint
{
    /// <summary>
    /// Forces the system to be in the working state by resetting the system idle timer.
    /// </summary>
    SystemRequired = 0x01,

    /// <summary>
    /// Forces the display to be on by resetting the display idle timer.
    /// </summary>
    DisplayRequired = 0x02,

    /// <summary>
    /// This value is not supported. If <see cref="UserPresent"/> is combined with other esFlags values, the call will fail and none of the specified states will be set.
    /// </summary>
    [Obsolete("This value is not supported.")]
    UserPresent = 0x04,

    /// <summary>
    /// Enables away mode. This value must be specified with <see cref="Continuous"/>.
    /// <para />
    /// Away mode should be used only by media-recording and media-distribution applications that must perform critical background processing on desktop computers while the computer appears to be sleeping.
    /// </summary>
    AwaymodeRequired = 0x40,

    /// <summary>
    /// Informs the system that the state being set should remain in effect until the next call that uses <see cref="Continuous"/> and one of the other state flags is cleared.
    /// </summary>
    Continuous = 0x80000000,
}
#endregion 系统休眠Dll 数据结构

#region 刷新资源管理Dll 数据结构

[Flags]
public enum SHChangeNotifyEvents : uint
{
    RenameItem = 0x00000001,
    Create = 0x00000002,
    Delete = 0x00000004,
    MkDir = 0x00000008,
    RmDir = 0x00000010,
    MediaInserted = 0x00000020,
    MediaRemoved = 0x00000040,
    DriveRemoved = 0x00000080,
    DriveAdd = 0x00000100,
    NetShare = 0x00000200,
    NetUnshare = 0x00000400,
    Attributes = 0x00000800,
    UpdateDir = 0x00001000,
    UpdateItem = 0x00002000,
    ServerDisconnect = 0x00004000,
    UpdateImage = 0x00008000,
    DriveAddGui = 0x00010000,
    RenameFolder = 0x00020000,
    FreeSpace = 0x00040000,
    ExtendedEvent = 0x04000000,
    AssocChanged = 0x08000000,
    DiskEvents = 0x0002381F,
    GlobalEvents = 0x0C0581E0,
    AllEvents = 0x7FFFFFFF,
    Interrupt = 0x80000000
}

public enum SHChangeNotifyFlags : uint
{
    IdList = 0x0000,
    PathA = 0x0001,
    PrinterA = 0x0002,
    Dword = 0x0003,
    PathW = 0x0005,
    PrinterW = 0x0006,
    Type = 0x00FF,
    Flush = 0x1000,
    FlushNoWait = 0x3000,
    NotifyRecursive = 0x10000
}
#endregion


#region 启动一个外部程序Dll 数据结构


/// <summary>
/// 设置窗体显示形式
/// </summary>
[Flags]
public enum WinExecFlag : uint
{
    SW_HIDE = 0,//隐藏窗口，活动状态给令一个窗口
    SW_SHOWNORMAL = 1,//用原来的大小和位置显示一个窗口，同时令其进入活动状态
    SW_NORMAL = 1,
    SW_SHOWMINIMIZED = 2,
    SW_SHOWMAXIMIZED = 3,
    SW_MAXIMIZE = 3,
    SW_SHOWNOACTIVATE = 4, //用最近的大小和位置显示一个窗口，同时不改变活动窗口
    SW_SHOW = 5, //用当前的大小和位置显示一个窗口，同时令其进入活动状态
    SW_MINIMIZE = 6, //最小化窗口，活动状态给令一个窗口
    SW_SHOWMINNOACTIVE = 7,//最小化一个窗口，同时不改变活动窗口
    SW_SHOWNA = 8, //用当前的大小和位置显示一个窗口，不改变活动窗口
    SW_RESTORE = 9, //与 SW_SHOWNORMAL  1 相同
    SW_SHOWDEFAULT = 10,
    SW_FORCEMINIMIZE = 11,
    SW_MAX = 11,
}
#endregion

#region 网络Dll 数据结构
// Enum to define the set of values used to indicate the type of table returned by 
// calls made to the function 'GetExtendedTcpTable'.
public enum TcpTableClass
{
    TCP_TABLE_BASIC_LISTENER,
    TCP_TABLE_BASIC_CONNECTIONS,
    TCP_TABLE_BASIC_ALL,
    TCP_TABLE_OWNER_PID_LISTENER,
    TCP_TABLE_OWNER_PID_CONNECTIONS,
    TCP_TABLE_OWNER_PID_ALL,
    TCP_TABLE_OWNER_MODULE_LISTENER,
    TCP_TABLE_OWNER_MODULE_CONNECTIONS,
    TCP_TABLE_OWNER_MODULE_ALL
}


// Enum to define the set of values used to indicate the type of table returned by calls
// made to the function GetExtendedUdpTable.
public enum UdpTableClass
{
    UDP_TABLE_BASIC,
    UDP_TABLE_OWNER_PID,
    UDP_TABLE_OWNER_MODULE
}
#endregion


#region 壁纸引擎DLL 数据结构



public enum AccentState
{
    ACCENT_DISABLED,
    ACCENT_ENABLE_GRADIENT,
    ACCENT_ENABLE_TRANSPARENTGRADIENT,
    ACCENT_ENABLE_BLURBEHIND,
    ACCENT_INVALID_STATE,
}

[StructLayout(LayoutKind.Sequential)]
public struct AccentPolicy
{
    public AccentState AccentState;
    public int AccentFlags;
    public int GradientColor;
    public int AnimationId;
}

[StructLayout(LayoutKind.Sequential)]
public struct WindowCompositionAttributeData
{
    public WindowCompositionAttribute Attribute;
    public IntPtr Data;
    public int SizeOfData;
}

public enum WindowCompositionAttribute
{
    // 省略其他未使用的字段
    WCA_ACCENT_POLICY = 19,
    // 省略其他未使用的字段
}



#endregion



[StructLayout(LayoutKind.Sequential)]
public struct WINDOWPOS
{
    public IntPtr hwnd;
    public IntPtr hwndInsertAfter;
    public int x;
    public int y;
    public int cx;
    public int cy;
    public uint flags;
}

