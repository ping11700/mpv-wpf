namespace mpv_wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

         
     }


 


    /// <summary>
    ///   
    /// </summary>
    internal static new void Shutdown()
    {
        var app = (App)Application.Current;

        try
        {
             
            Application.Current.Shutdown();
        }
        finally
        {
            Environment.Exit(0);
        }
    }
}
