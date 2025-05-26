namespace Core.Extensions;

public static class ApplicationExtension
{

    public static void ConfigApplication(this System.Windows.Application application)
    {
        try
        {
            // 设置 线程池数量
            ThreadPool.GetMinThreads(out var workers, out var ports);
            ThreadPool.SetMinThreads(workers + 6, ports + 6);

            // 尽量提升程序的优先级
            var process = Process.GetCurrentProcess();
            process.PriorityClass = ProcessPriorityClass.RealTime;

            //限制动画帧率为30帧
            //System.Windows.Media.Animation.Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(System.Windows.Media.Animation.Timeline), new FrameworkPropertyMetadata { DefaultValue = 30 });
        }
        catch { }

        application.Startup += (s, e) =>
        {
            //捕获主线程（UI线程的异常）
            application.DispatcherUnhandledException -= App_DispatcherUnhandledException;
            application.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //捕获非UI线程的异常（线程池的异常不能捕获）
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //捕获Task异常
            TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;


        };
        application.Exit += (s, e) =>
        {
            //捕获主线程（UI线程的异常）
            application.DispatcherUnhandledException -= App_DispatcherUnhandledException;
            //捕获非UI线程的异常（线程池的异常不能捕获）
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
            //捕获Task异常
            TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;


        };


        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            OnException(e.Exception);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OnException(e.ExceptionObject as Exception);
        }

        void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            OnException(e.Exception.GetBaseException());
        }

        /// <summary>
        /// 系统异常 重启
        /// </summary>
        /// <param name="e"></param>
        void OnException(Exception? ex)
        {
            RecordUnhandledException(ex);
        }

        /// <summary>
        /// 记录不能处理的系统异常
        /// </summary>
        /// <param name="e"></param>
        void RecordUnhandledException(Exception? ex)
        {
            Logger.Error($" Message:{ex?.Message}\n{ex?.StackTrace}", ex);
        }
    }


    /// <summary>
    /// Restart App
    /// </summary>
    /// <param name="application"></param>
    public static void RestartApplication(this System.Windows.Application application)
    {
        //程序位置
        string? strAppFileName = Process.GetCurrentProcess().MainModule?.FileName;

        using (Process myProcess = new())
        {
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.CreateNoWindow = true;
            //要启动的应用程序
            myProcess.StartInfo.FileName = strAppFileName;
            // 设置要启动的进程的初始目录
            myProcess.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //启动程序
            myProcess.Start();
        }
        //结束该程序
        Process.GetCurrentProcess().Kill();
        //结束该进程所有线程
        Environment.Exit(0);
    }


    /// <summary>
    /// ShutdownEvent App
    /// </summary>
    /// <param name="application"></param>
    public static void ShutdownApplication(this System.Windows.Application application)
    {
        try
        {
            application.Access(() => Application.Current.Shutdown());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ShutdownEvent Failed:{ex.Message}");

            Environment.Exit(0);
        }
        finally
        {
        }
    }



    /// <summary>
    /// Dispatcher.Access  Action
    /// </summary>
    /// <param name="application"></param>
    /// <param name="action"></param>
    public static void Access(this System.Windows.Application application, Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal)
    {
        if (application == null)
        {
            return;
        }
        if (!application.Dispatcher.CheckAccess())
        {
            application.Dispatcher.Invoke(action, dispatcherPriority);
            return;
        }

        action?.Invoke();
    }

 



    /// <summary>
    /// Dispatcher.AccessAsync  Action
    /// </summary>
    /// <param name="application"></param>
    /// <param name="action"></param>
    public static void AccessAsync(this System.Windows.Application application, Action action)
    {
        if (!application.Dispatcher.CheckAccess())
            application.Dispatcher.InvokeAsync(action);

        Task.Run(action);
    }


    /// <summary>
    /// Dispatcher.AccessAsync Func
    /// </summary> 
    /// <typeparam name="TResult"></typeparam>
    /// <param name="application"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TResult AccessAsync<TResult>(this System.Windows.Application application, Func<TResult> func)
    {
        if (!application.Dispatcher.CheckAccess())
        {
            return application.Dispatcher.InvokeAsync<TResult>(func).Result;
        }

        return Task.Run(func).Result;
    }
}