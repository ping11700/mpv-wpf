namespace MpvLib.Utils;

public class Util
{
    public static void Run(Action action)
    {
        Task.Run(() =>
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                //Terminal.WriteError(e);
            }
        });
    }



    [SupportedOSPlatform("windows")]
    public static string GetShortcutTarget(string path)
    {
        Type? t = Type.GetTypeFromProgID("WScript.Shell");
        dynamic? sh = Activator.CreateInstance(t!);
        return sh?.CreateShortcut(path).TargetPath!;
    }


    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr LoadLibrary(string path);
}