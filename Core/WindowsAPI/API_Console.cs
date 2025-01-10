namespace Core.WindowsAPI;

/// <summary>
/// Console api
/// </summary>
//参考: https://www.cnblogs.com/Chary/p/No0000B8.html
[SuppressUnmanagedCodeSecurity]
public class API_Console
{


    //当关闭Console时，系统会发送下面的消息
    private const int CtrlCEvent = 0; //无论是从键盘输入或由GenerateConsoleCtrlEvent功能信号产生的一个CTRL + C接收信号

    private const int CtrlBreakEvent = 1; //无论是从键盘输入或由GenerateConsoleCtrlEvent信号产生的一个CTRL + BREAK信号接收。
    private const int CtrlCloseEvent = 2; //信号系统，当用户关闭控制台（通过单击控制台窗口菜单上的关闭按钮，或通过从任务管理器结束任务）
    private const int CtrlLogoffEvent = 5; //用户注销时系统发送到所有控制台进程的信号。此信号不指示哪个用户正在注销，因此不能进行任何假设。请注意，此信号仅由服务接收。交互式应用程序在注销时终止，因此当系统发送此信号时，它们不存在。
    private const int CtrlShutdownEvent = 6; //系统关闭时系统发送的信号。在系统发送此信号时，交互式应用程序不存在，因此在这种情况下它只能被服务接收。服务还有自己的关闭事件的通知机制。这个信号还可以通过使用应用程序生成的GenerateConsoleCtrlEvent。

    private static readonly ConsoleCtrlDelegate ConsoleCtrlDelegateHandlerRoutine = new(HandlerRoutine);

    /// <summary>
    /// 是否已经开启控制台
    /// </summary>
    public static bool HasConsole => NativeAPI.GetConsoleWindow() != IntPtr.Zero;

    private static event CtrlCPressedHandler? CtrlCPressed;

    private static void OnCtrlCPressed(object? sender, ConsoleCancelEventArgs? e)
    {
        CtrlCPressed?.Invoke(sender, e);
    }

    public static void Toggle()
    {
        if (HasConsole)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    /// <summary>
    /// Creates a new console instance if the process is not attached to a console already.
    /// </summary>
    public static void Show()
    {
        if (!HasConsole)
        {
            NativeAPI.AllocConsole();

            InvalidateOutAndError();

            RemoveCloseButton();

            NativeAPI.AttachConsole(-1);

            if (NativeAPI.SetConsoleCtrlHandler(ConsoleCtrlDelegateHandlerRoutine, true))
                Console.WriteLine("************控制台启动************ \n");
        }
    }

    /// <summary>
    /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
    /// </summary>
    public static void Hide()
    {
        if (HasConsole)
        {
            SetOutAndErrorNull();
            NativeAPI.FreeConsole();
        }
    }



    /// <summary>
    ///     禁用关闭按钮
    /// </summary>
    private static void RemoveCloseButton()
    {
        //与控制台标题名一样的路径,根据控制台标题找控制台
        var windowHandler = NativeAPI.FindWindow(null, Process.GetCurrentProcess().MainModule?.FileName);

        //找关闭按钮
        var closeMenu = NativeAPI.GetSystemMenu(windowHandler, IntPtr.Zero);
        var scClose = 0xF060;
        //关闭按钮禁用
        NativeAPI.RemoveMenu(closeMenu, scClose, 0x0);
    }

    private static void InvalidateOutAndError()
    {
        Type type = typeof(Console);

        FieldInfo? _out = type.GetField("_out", BindingFlags.Static | BindingFlags.NonPublic);

        FieldInfo? _error = type.GetField("_error", BindingFlags.Static | BindingFlags.NonPublic);

        MethodInfo? _InitializeStdOutError = type.GetMethod("InitializeStdOutError", BindingFlags.Static | BindingFlags.NonPublic);

        _out?.SetValue(null, null);
        _error?.SetValue(null, null);
        _InitializeStdOutError?.Invoke(null, new object[] { true });
    }

    private static void SetOutAndErrorNull()
    {
        Console.SetOut(TextWriter.Null);
        Console.SetError(TextWriter.Null);
    }

    /// <summary>
    /// 处理程序例程，在这里编写对指定事件的处理程序代码
    /// </summary>
    /// <param name="ctrlType"></param>
    /// <returns></returns>
    private static bool HandlerRoutine(int ctrlType)
    {
        switch (ctrlType)
        {
            case CtrlCEvent:
                OnCtrlCPressed(null, null);
                Console.WriteLine("Ctrl+C按下，阻止");
                return true; //这里返回true，表示阻止响应系统对该程序的操作成功
            case CtrlBreakEvent:
                Console.WriteLine("Ctrl+BREAK按下，阻止");
                return true;

            case CtrlCloseEvent:
                Console.WriteLine("CLOSE");
                break;

            case CtrlLogoffEvent:
                Console.WriteLine("LOGOFF");
                break;

            case CtrlShutdownEvent:
                Console.WriteLine("SHUTDOWN");
                break;
        }
        return true; //true 表示阻止响应系统对该程序的操作 //false 忽略处理，让系统进行默认操作
    }
}

public delegate void CtrlCPressedHandler(object? sender, ConsoleCancelEventArgs? e);

//定义处理程序委托
public delegate bool ConsoleCtrlDelegate(int dwCtrlType);