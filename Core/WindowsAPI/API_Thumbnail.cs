namespace Core.WindowsAPI;

/// <summary>
/// 缩略图 API
/// </summary>
public static class API_Thumbnail
{

    private const string IShellItem2Guid = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int SHCreateItemFromParsingName(
        [MarshalAs(UnmanagedType.LPWStr)] string path,
        // The following parameter is not used - binding context.
        IntPtr pbc,
        ref Guid riid,
        [MarshalAs(UnmanagedType.Interface)] out IShellItem shellItem);

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeleteObject(IntPtr hObject);

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    internal interface IShellItem
    {
        void BindToHandler(IntPtr pbc,
            [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IntPtr ppv);

        void GetParent(out IShellItem ppsi);
        void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
        void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
        void Compare(IShellItem psi, uint hint, out int piOrder);
    };

    internal enum SIGDN : uint
    {
        NORMALDISPLAY = 0,
        PARENTRELATIVEPARSING = 0x80018001,
        PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
        DESKTOPABSOLUTEPARSING = 0x80028000,
        PARENTRELATIVEEDITING = 0x80031001,
        DESKTOPABSOLUTEEDITING = 0x8004c000,
        FILESYSPATH = 0x80058000,
        URL = 0x80068000
    }

    internal enum HResult
    {
        Ok = 0x0000,
        False = 0x0001,
        InvalidArguments = unchecked((int)0x80070057),
        OutOfMemory = unchecked((int)0x8007000E),
        NoInterface = unchecked((int)0x80004002),
        Fail = unchecked((int)0x80004005),
        ElementNotFound = unchecked((int)0x80070490),
        TypeElementNotFound = unchecked((int)0x8002802B),
        NoObject = unchecked((int)0x800401E5),
        Win32ErrorCanceled = 1223,
        Canceled = unchecked((int)0x800704C7),
        ResourceInUse = unchecked((int)0x800700AA),
        AccessDenied = unchecked((int)0x80030005)
    }

    [ComImportAttribute()]
    [GuidAttribute("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItemImageFactory
    {
        [PreserveSig]
        HResult GetImage(
        [In, MarshalAs(UnmanagedType.Struct)] NativeSize size,
        [In] ThumbnailOptions flags,
        [Out] out IntPtr phbm);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeSize
    {
        private int width;
        private int height;

        public int Width { set { width = value; } }
        public int Height { set { height = value; } }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static BitmapSource GetThumbnailBitmapSource(string file)
    {
        var bitmapImage = new BitmapImage
        {
            DecodePixelWidth = 50
        };
        bitmapImage.BeginInit();
        bitmapImage.UriSource = new Uri(file, UriKind.RelativeOrAbsolute);
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static BitmapSource GetThumbnailBitmapSource(string fileName, int width, int height, ThumbnailOptions options)
    {
        IntPtr hBitmap = IntPtr.Zero;
        try
        {
            hBitmap = GetHBitmap(Path.GetFullPath(fileName), width, height, options);
            var bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
                                                                                IntPtr.Zero,
                                                                                System.Windows.Int32Rect.Empty,
                                                                                BitmapSizeOptions.FromWidthAndHeight(width, height));
            bs.Freeze();

            return bs;
        }
        catch (Exception)
        {
            return default;
        }
        finally
        {
            // delete HBitmap to avoid memory leaks
            DeleteObject(hBitmap);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Bitmap GetThumbnailBitmap(string fileName, int width, int height, ThumbnailOptions options)
    {
        IntPtr hBitmap = IntPtr.Zero;

        try
        {
            hBitmap = GetHBitmap(Path.GetFullPath(fileName), width, height, options);
            return GetBitmapFromHBitmap(hBitmap);
        }
        catch (Exception)
        {
            return default;
        }
        finally
        {
            // delete HBitmap to avoid memory leaks
            DeleteObject(hBitmap);
        }
    }

    private static Bitmap GetBitmapFromHBitmap(IntPtr nativeHBitmap)
    {
        Bitmap bmp = Bitmap.FromHbitmap(nativeHBitmap);

        if (Bitmap.GetPixelFormatSize(bmp.PixelFormat) < 32) return bmp;

        return CreateAlphaBitmap(bmp, PixelFormat.Format32bppArgb);
    }

    private static Bitmap CreateAlphaBitmap(Bitmap srcBitmap, PixelFormat targetPixelFormat)
    {
        Bitmap result = new Bitmap(srcBitmap.Width, srcBitmap.Height, targetPixelFormat);

        Rectangle bmpBounds = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);

        BitmapData srcData = srcBitmap.LockBits(bmpBounds, ImageLockMode.ReadOnly, srcBitmap.PixelFormat);

        bool isAlplaBitmap = false;

        try
        {
            for (int y = 0; y <= srcData.Height - 1; y++)
            {
                for (int x = 0; x <= srcData.Width - 1; x++)
                {
                    Color pixelColor = Color.FromArgb(
                        Marshal.ReadInt32(srcData.Scan0, (srcData.Stride * y) + (4 * x)));

                    if (pixelColor.A > 0 & pixelColor.A < 255)
                    {
                        isAlplaBitmap = true;
                    }

                    result.SetPixel(x, y, pixelColor);
                }
            }
        }
        finally
        {
            srcBitmap.UnlockBits(srcData);
        }

        if (isAlplaBitmap)
        {
            return result;
        }
        else
        {
            return srcBitmap;
        }
    }

    private static IntPtr GetHBitmap(string fileName, int width, int height, ThumbnailOptions options)
    {
        Guid shellItem2Guid = new(IShellItem2Guid);
        int retCode = SHCreateItemFromParsingName(fileName, IntPtr.Zero, ref shellItem2Guid, out IShellItem nativeShellItem);

        if (retCode != 0)
            throw Marshal.GetExceptionForHR(retCode);

        NativeSize nativeSize = new NativeSize
        {
            Width = width,
            Height = height
        };

        HResult hr = ((IShellItemImageFactory)nativeShellItem).GetImage(nativeSize, options, out IntPtr hBitmap);

        Marshal.ReleaseComObject(nativeShellItem);

        if (hr == HResult.Ok) return hBitmap;

        throw Marshal.GetExceptionForHR((int)hr);
    }

}

[Flags]
public enum ThumbnailOptions
{
    None = 0x00,
    BiggerSizeOk = 0x01,
    InMemoryOnly = 0x02,
    IconOnly = 0x04,
    ThumbnailOnly = 0x08,
    InCacheOnly = 0x10,
}