namespace Core.Log;

public class Logger
{
    public static string FilePath { get; set; }

    static Logger()
    {
        var dir = $"{AppDomain.CurrentDomain.BaseDirectory}/logs";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


        System.Diagnostics.Trace.Listeners.Clear();  //清除系统监听器 (就是输出到Console的那个)
        System.Diagnostics.Trace.Listeners.Add(new LogListener()); //添加Logger实例

        FilePath = $"{dir}/app_{DateTime.Now:yyyy-MM-dd}.log";

        //只保留一天日志
        Task.Run(() =>
        {
            var fileNames = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.log*");

            foreach (var name in fileNames)
            {
                var date = name.Substring(name.Length - 14, 10);
                if (DateTime.TryParse(date, out DateTime dateTime) && DateTime.Compare(DateTime.Now.Date, dateTime) == 1)
                    File.Delete(name);
            }
        });
    }


    public static void Fatal(string msg, object? obj = null) => System.Diagnostics.Trace.Write(obj, $"[Fatal]-{msg}");

    public static void Error(string msg, object? obj = null) => System.Diagnostics.Trace.Write(obj, $"[Error]-{msg}");

    public static void Warn(string msg, object? obj = null) => System.Diagnostics.Trace.Write(obj, $"[Warn]-{msg}");

    public static void Info(object msg) => System.Diagnostics.Trace.Write(msg, "[Info]");

    public static void Trace(string msg)
    {
#if DEBUG
     System.Diagnostics.Trace.Write(msg, "[Trace]");
#endif
    }

}
