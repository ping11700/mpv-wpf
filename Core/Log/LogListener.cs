namespace Core.Log;


public class LogListener : TraceListener
{
    // level trace 日志 输出
    public override void Write(string message)
    {

#if DEBUG 

        File.AppendAllText(Logger.FilePath, DateTime.Now.ToString("HH:mm:ss") + message + Environment.NewLine);
#else
        if (GlobalVars.IsAdmin == true) 
        {
            File.AppendAllText(Logger.FilePath, DateTime.Now.ToString("HH:mm:ss") + message + Environment.NewLine);
        }
#endif

    }

    public override void WriteLine(string message)
    {
        File.AppendAllText(Logger.FilePath, DateTime.Now.ToString("HH:mm:ss") + message + Environment.NewLine);
    }


    public override void Write(object sender, string category)
    {
        string msg = "";
        if (string.IsNullOrWhiteSpace(category) == false) //category参数不为空
        {
            msg = category + ": ";
        }
        if (sender is Exception ex) //如果参数o是异常类,输出异常消息+堆栈,否则输出o.ToString()
        {
            msg += ex.Message + Environment.NewLine;
            msg += ex.StackTrace;
        }
        else if (sender != null)
        {
            msg += sender.ToString();
        }
        WriteLine(msg);
    }

}
