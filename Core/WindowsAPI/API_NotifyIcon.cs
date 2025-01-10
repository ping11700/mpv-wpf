namespace Core.WindowsAPI;

/// <summary>
/// 托盘API
/// </summary>
public static class API_NotifyIcon
{


    /// <summary>
    /// 鼠标是否进入托盘
    /// </summary>
    /// <returns></returns>

    public static bool CheckMouseIsEnter()
    {
        var isTrue = FindNotifyIcon(out var rectNotifyList);
        if (!isTrue) return false;

        NativeAPI.GetCursorPos(out var point);
        return rectNotifyList.Any(rectNotify => point.X >= rectNotify.Left &&
                                                point.X <= rectNotify.Right &&
                                                point.Y >= rectNotify.Top &&
                                                point.Y <= rectNotify.Bottom);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="rectList"></param>
    /// <returns></returns>
    private static bool FindNotifyIcon(out List<RECT> rectList)
    {
        var rectNotifyList = new List<RECT>();
        var hTrayWnd = FindTrayToolbarWindow();
        var isTrue = FindNotifyIcon(hTrayWnd, ref rectNotifyList);
        if (!isTrue)
        {
            hTrayWnd = FindTrayToolbarOverFlowWindow();
            isTrue = FindNotifyIcon(hTrayWnd, ref rectNotifyList);
        }
        rectList = rectNotifyList;
        return isTrue;
    }

    /// <summary>
    /// 查找到托盘图标所在的窗口
    /// </summary>
    /// <returns></returns>
    //referenced from http://www.cnblogs.com/sczmzx/p/5158127.html
    private static IntPtr FindTrayToolbarWindow()
    {
        var hWnd = NativeAPI.FindWindow("Shell_TrayWnd", null);
        if (hWnd != IntPtr.Zero)
        {
            hWnd = NativeAPI.FindWindowEx(hWnd, IntPtr.Zero, "TrayNotifyWnd", null);
            if (hWnd != IntPtr.Zero)
            {
                hWnd = NativeAPI.FindWindowEx(hWnd, IntPtr.Zero, "SysPager", null);
                if (hWnd != IntPtr.Zero)
                {
                    hWnd = NativeAPI.FindWindowEx(hWnd, IntPtr.Zero, "ToolbarWindow32", null);
                }
            }
        }

        return hWnd;
    }



    /// <summary>
    /// 如果没有找到，那么需要用相同的方法在托盘溢出区域内查找
    /// </summary>
    /// <returns></returns>
    //referenced from http://www.cnblogs.com/sczmzx/p/5158127.html
    private static IntPtr FindTrayToolbarOverFlowWindow()
    {
        var hWnd = NativeAPI.FindWindow("NotifyIconOverflowWindow", null);
        if (hWnd != IntPtr.Zero)
        {
            hWnd = NativeAPI.FindWindowEx(hWnd, IntPtr.Zero, "ToolbarWindow32", null);
        }
        return hWnd;
    }









    //referenced from http://www.cnblogs.com/sczmzx/p/5158127.html
    private static bool FindNotifyIcon(IntPtr hTrayWnd, ref List<RECT> rectNotifyList)
    {
        NativeAPI.GetWindowRect(hTrayWnd, out var rectTray);
        var count = (int)NativeAPI.SendMessage(hTrayWnd, InteropValues.TB_BUTTONCOUNT, 0, IntPtr.Zero);

        var isFind = false;
        if (count > 0)
        {
            NativeAPI.GetWindowThreadProcessId(hTrayWnd, out var trayPid);
            var hProcess = NativeAPI.OpenProcess(ProcessAccess.VMOperation | ProcessAccess.VMRead | ProcessAccess.VMWrite, false, trayPid);
            var address = NativeAPI.VirtualAllocEx(hProcess, IntPtr.Zero, 1024, AllocationType.Commit, MemoryProtection.ReadWrite);

            var btnData = new TBBUTTON();
            var trayData = new TRAYDATA();
            var handle = Process.GetCurrentProcess().Id;

            for (uint i = 0; i < count; i++)
            {
                NativeAPI.SendMessage(hTrayWnd, InteropValues.TB_GETBUTTON, i, address);
                var isTrue = NativeAPI.ReadProcessMemory(hProcess, address, out btnData, Marshal.SizeOf(btnData), out _);
                if (!isTrue) continue;

                if (btnData.dwData == IntPtr.Zero)
                {
                    btnData.dwData = btnData.iString;
                }

                NativeAPI.ReadProcessMemory(hProcess, btnData.dwData, out trayData, Marshal.SizeOf(trayData), out _);
                NativeAPI.GetWindowThreadProcessId(trayData.hwnd, out var dwProcessId);

                if (dwProcessId == (uint)handle)
                {
                    var rect = new RECT();
                    var lngRect = NativeAPI.VirtualAllocEx(hProcess, IntPtr.Zero, Marshal.SizeOf(typeof(Rect)), AllocationType.Commit, MemoryProtection.ReadWrite);

                    NativeAPI.SendMessage(hTrayWnd, InteropValues.TB_GETITEMRECT, i, lngRect);
                    NativeAPI.ReadProcessMemory(hProcess, lngRect, out rect, Marshal.SizeOf(rect), out _);

                    NativeAPI.VirtualFreeEx(hProcess, lngRect, Marshal.SizeOf(rect), FreeType.Decommit);
                    NativeAPI.VirtualFreeEx(hProcess, lngRect, 0, FreeType.Release);

                    var left = rectTray.Left + rect.Left;
                    var top = rectTray.Top + rect.Top;
                    var botton = rectTray.Top + rect.Bottom;
                    var right = rectTray.Left + rect.Right;
                    rectNotifyList.Add(new RECT
                    {
                        Left = left,
                        Right = right,
                        Top = top,
                        Bottom = botton
                    });
                    isFind = true;
                }
            }
            NativeAPI.VirtualFreeEx(hProcess, address, 0x4096, FreeType.Decommit);
            NativeAPI.VirtualFreeEx(hProcess, address, 0, FreeType.Release);
            NativeAPI.CloseHandle(hProcess);
        }
        return isFind;
    }


}