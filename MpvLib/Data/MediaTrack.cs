namespace MpvLib.Data;

/// <summary>
/// 音轨
/// </summary>
public class MediaTrack
{
    public int ID { get; set; }
    public bool External { get; set; }
    public string Text { get; set; } = "";
    public string Type { get; set; } = "";
}
