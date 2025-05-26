using mpv_wpf._View;

namespace mpv_wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// 检测多个进程同时运行的互斥体
    /// </summary>
    private Mutex? _procInstanceMutex;

    public App()
    {
        this.ConfigApplication();
    }



    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

#if DEBUG
        _procInstanceMutex = new(true, "mpv_wpf_App_Mutex_DEBUG", out bool createdNew);
#else
        _procInstanceMutex = new(true, "mpv_wpf_App_Mutex", out bool createdNew);
#endif
        if (!createdNew)
        {
            Logger.Info($"mpv_wpf_App Already Running");

            API_Window.CallMainWindow(Consts.APPShellName, System.Text.Json.JsonSerializer.Serialize(e.Args));

            Shutdown();

            return;
        }

        new PlayerShell(new PlayerMpv()).Show();
    }



    /// <summary>
    ///   
    /// </summary>
    internal static new void Shutdown()
    {
        Application.Current.ShutdownApplication();
    }
}
