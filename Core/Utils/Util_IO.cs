namespace Core.Utils;

public class Util_IO
{
    public static readonly string SplashVideo = Path.Combine(AppContext.BaseDirectory, "SplashVideo.mp4");

    public const string CommonExtensions = ".txt;.rtf;.pdf;.doc;.docm;.docx;.dotx;.csv;.xlam;.xls;.xlsx;.xml;.xps;.ppt;.potx;.exe;.apk;.lnk;";


    public const string VideoFilters = "*.mp4;*.mov;*.ts;*.dat;*.avi;*.mts;*.flv;*.swf;*.mpg;*.3gp;*.wmv;*.mkv;*.rmvb;" +
                                       "*.vob;*.f4v;*.rm;*.m2ts;*.m4v;*.mpeg;*.asf;*.wmz;*.webM;*.wm;*.pmp;*.mpga;*.264,;" +
                                       "*.265,;*.avc,;*.h264,;*.h265,*.hevc,*.m2v,*.mpv,*.vpy,*.y4m,";


    public const string ImageFilters = "*.gif;*.bmp;*.jpg;*.png;*.webp;*.tif;*.tiff;*.jpeg;*.heic;*.avif;";



    public const string AudioFilters = "*.mp3;*.flac;*.m4a;*.mka;*.mp2;*.ogg;*.opus;*.aac;*.ac3;*.dts;*.dtshd;*.dtshr;*.dtsma;" +
                                       "*.eac3;*.mpa;*.mpc;*.thd;*.w64;*.wav;*.wma;*.alac;*.aiff;";


    public const string SubtitleFilters = "*.srt;*.ass;*.idx;*.sub;*.sup;*.ttxt;*.txt;*.ssa;*.smi;*.mks;";
 


    /// <summary>
    /// 磁盘不足
    /// </summary>
    /// <param name="actualNeededSize"></param>
    /// <param name="directorie"></param>
    /// <exception cref="IOException"></exception>
    public static void ThrowIfNotEnoughDisk(long actualNeededSize, string directorie)
    {
        try
        {
            var drive = new DriveInfo(directorie);

            var space = drive.IsReady ? drive.AvailableFreeSpace : 0L;

            if (space > 0 && space < actualNeededSize)
                throw new IOException($"There is not enough space on the disk `{directorie}` with {space} bytes");
        }
        catch (Exception)
        {

        }

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public static bool DetecCommonFile(string file) => CommonExtensions.Contains(Path.GetExtension(file));



    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static bool DetecVideoFile(string file) => VideoFilters.Contains(Path.GetExtension(file));



    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static bool DetecAudioFile(string file) => AudioFilters.Contains(Path.GetExtension(file));



    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static bool DetecSubtitleFile(string file) => SubtitleFilters.Contains(Path.GetExtension(file));

}