namespace Controls.Control;


/// <summary>
/// 
/// </summary>
public class GrowlInfo
{
    public Window ParentWin { get; set; }
  
    public string Message { get; set; }

    public bool ShowDateTime { get; set; } = true;

    public int WaitTime { get; set; } = 2;

    public string CancelStr { get; set; } = "取消";

    public string ConfirmStr { get; set; } = "确定";

    public Func<bool, bool> ActionBeforeClose { get; set; }

    public bool StaysOpen { get; set; }

    public bool IsCustom { get; set; }

    public Geometry Icon { get; set; }

    public GrowlInfoType Type { get; set; }

    public System.Windows.Media.Brush IconBrush { get; set; }


    public string IconKey { get; set; }

    public string IconBrushKey { get; set; }

    public bool ShowCloseButton { get; set; } = true;

    public string Token { get; set; }

    public FlowDirection FlowDirection { get; set; }
}


/// <summary>
/// 
/// </summary>
public enum GrowlInfoType
{
    Success = 0,

    Info,

    Warning,

    Error,

    Fatal,

    Ask
}
