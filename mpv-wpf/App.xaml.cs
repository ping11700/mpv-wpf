namespace mpv_wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public T? GetRequiredService<T>() where T : class => _host?.Services.GetRequiredService<T>();

    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder(e.Args).ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(AppContext.BaseDirectory);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(e.Args);
                })
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureAppConfiguration(appConfig =>
                {
                    //var configPath = SavePaths.ConfigDirectory;
                    // 如果不存在 AccountConfig 则尝试加载 AppConfig
                    //appConfig.AddJsonFile(Path.Combine(configPath, "AppConfig"), optional: true, reloadOnChange: true);
                })
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    //logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
                    //logging.AddConsole();
                    //logging.AddDebug();
                    //logging.AddNLog();
                })
                .Build();

        await _host.StartAsync();
     }


    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        
    }


    /// <summary>
    ///   
    /// </summary>
    internal static new void Shutdown()
    {
        var app = (App)Application.Current;

        try
        {
            if (app._host != null)
            {
                app._host.StopAsync();
                app._host.Dispose();
                app._host = null;
            }

            Application.Current.Shutdown();
        }
        finally
        {
            Environment.Exit(0);
        }
    }
}
