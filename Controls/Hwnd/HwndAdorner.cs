using POINT = Core.WindowsAPI.POINT;

namespace Controls.Hwnd;
/// <summary>
/// A class for managing an adornment above all other content (including non-WPF child windows (hwnd), unlike the WPF Adorner classes)
/// </summary>
public sealed class HwndAdorner : IDisposable
{
    // See the HwndAdornerElement class for a simple usage example.
    // 
    // Another way of using this class is through the HwndExtensions.HwndAdornment attached property,
    // which can attach any UIElement as an Adornment to any FrameworkElement. 
    // This option lacks the logical parenting provided by HwndAdornerElement. 
    // 
    // Event routing should work in any case (through the GetUIParentCore override of the HwndAdornmentRoot class)

    private readonly FrameworkElement _elementAttachedTo;
    private readonly HwndAdornmentRoot _hwndAdornmentRoot;
    private UIElement? _adornment;
    private HwndAdornerGroup? _hwndAdornerGroup;
    private HwndSource? _hwndSource;
    private bool _shown;

    private Rect _parentBoundingBox;
    private Rect _boundingBox;

    private bool _disposed;

    private const uint NO_REPOSITION_FLAGS = InteropValues.SWP_NOMOVE | InteropValues.SWP_NOSIZE | InteropValues.SWP_NOACTIVATE |
                                             InteropValues.SWP_NOZORDER | InteropValues.SWP_NOOWNERZORDER | InteropValues.SWP_NOREPOSITION;

    private const uint SET_ONLY_LOCATION = InteropValues.SWP_NOACTIVATE | InteropValues.SWP_NOZORDER | InteropValues.SWP_NOOWNERZORDER;

    private const int HTTRANSPARENT = -1;

    private const uint ResizeBorderThickness = 6;

    public HwndAdorner(FrameworkElement attachedTo)
    {
        _elementAttachedTo = attachedTo;
        _parentBoundingBox = _boundingBox = new Rect(new Point(), new Size());

        _hwndAdornmentRoot = new HwndAdornmentRoot()
        {
            UIParentCore = _elementAttachedTo,
        };

        _elementAttachedTo.Loaded += OnLoaded;
        _elementAttachedTo.Unloaded += OnUnloaded;
        _elementAttachedTo.IsVisibleChanged += OnIsVisibleChanged;
        _elementAttachedTo.LayoutUpdated += OnLayoutUpdated;
    }


    internal IntPtr Handle => _hwndSource?.Handle ?? IntPtr.Zero;

    internal void InvalidateAppearance()
    {
        if (_hwndSource == null) return;

        if (NeedsToAppear)
        {
            if (!_shown)
            {
                NativeAPI.SetWindowPos(_hwndSource.Handle, IntPtr.Zero, 0, 0, 0, 0, NO_REPOSITION_FLAGS | InteropValues.SWP_SHOWWINDOW);
                _shown = true;
            }
        }
        else
        {
            if (_shown)
            {
                NativeAPI.SetWindowPos(_hwndSource.Handle, IntPtr.Zero, 0, 0, 0, 0, NO_REPOSITION_FLAGS | InteropValues.SWP_HIDEWINDOW);
                _shown = false;
            }
        }
    }

    internal void UpdateOwnerPosition(Rect rect)
    {
        if (!_parentBoundingBox.Equals(rect))
        {
            _parentBoundingBox = rect;
            SetAbsolutePosition();
        }
    }


    public FrameworkElement Root => _hwndAdornmentRoot;

    public UIElement? Adornment
    {
        get => _adornment;
        set
        {
            if (_disposed)
                throw new ObjectDisposedException("HwndAdorner");

            _adornment = value;
            if (_elementAttachedTo.IsLoaded)
                _hwndAdornmentRoot.SetValue(ContentControl.ContentProperty, _adornment);
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        InitHwndSource();
        _hwndAdornmentRoot.SetCurrentValue(ContentControl.ContentProperty, _adornment);
        ConnectToGroup();
    }

    private void OnUnloaded(object sender, RoutedEventArgs args)
    {
        DisconnectFromGroup();
        _hwndAdornmentRoot.SetCurrentValue(ContentControl.ContentProperty, null);
        DisposeHwndSource();
    }

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        InvalidateAppearance();
    }

    private void OnLayoutUpdated(object? sender, EventArgs eventArgs)
    {
        var source = PresentationSource.FromVisual(_elementAttachedTo);
        var ct = source?.CompositionTarget;

        if (ct?.RootVisual != null)
        {
            UpdateBoundingBox(CalculateAssignedRC(source!));
        }
    }


    private void UpdateBoundingBox(Rect boundingBox)
    {
        if (!_boundingBox.Equals(boundingBox))
        {
            _boundingBox = boundingBox;
            SetAbsolutePosition();
        }
    }

    private Rect CalculateAssignedRC(PresentationSource source)
    {
        var rectElement = new Rect(_elementAttachedTo.RenderSize);
        var rectRoot = Util_Visual.ElementToRoot(rectElement, _elementAttachedTo, source);
        return Util_Visual.RootToClient(rectRoot, source);
    }

    private bool Owned => _hwndAdornerGroup?.Owned ?? false;

    private bool NeedsToAppear => Owned && _elementAttachedTo.IsVisible;

    private void ConnectToGroup()
    {
        DisconnectFromGroup();

        _hwndAdornerGroup = new HwndAdornerGroup(_elementAttachedTo);
        _hwndAdornerGroup.AddAdorner(this);
    }

    private void DisconnectFromGroup()
    {
        if (_hwndAdornerGroup == null) return;

        _hwndAdornerGroup.RemoveAdorner(this);
        _hwndAdornerGroup = null;
    }

    private void SetAbsolutePosition()
    {
        if (_hwndSource == null) return;

        NativeAPI.SetWindowPos(_hwndSource.Handle, IntPtr.Zero,
            (int)(_parentBoundingBox.X + _boundingBox.X),
            (int)(_parentBoundingBox.Y + _boundingBox.Y),
            (int)(Math.Min(_boundingBox.Width, _parentBoundingBox.Width - _boundingBox.X)),
            (int)(Math.Min(_boundingBox.Height, _parentBoundingBox.Height - _boundingBox.Y)),
            SET_ONLY_LOCATION | InteropValues.SWP_ASYNCWINDOWPOS);
    }

    private void InitHwndSource()
    {
        if (_hwndSource != null) return;

        int classStyle = 0;
        int style = 0;
        int styleEx = (int)InteropValues.WS_EX_NOACTIVATE;

        var parameters = new HwndSourceParameters()
        {
            UsesPerPixelOpacity = true,
            WindowClassStyle = classStyle,
            WindowStyle = style,
            ExtendedWindowStyle = styleEx,
            PositionX = (int)(_parentBoundingBox.X + _boundingBox.X),
            PositionY = (int)(_parentBoundingBox.Y + _boundingBox.Y),
            Width = (int)(_boundingBox.Width),
            Height = (int)(_boundingBox.Height)
        };

        _hwndSource = new HwndSource(parameters)
        {
            RootVisual = _hwndAdornmentRoot
        };

        _hwndSource.AddHook(WndProc);

        _shown = false;
    }

    private void DisposeHwndSource()
    {
        if (_hwndSource == null) return;

        _hwndSource.RemoveHook(WndProc);
        _hwndSource.Dispose();
        _hwndSource = null;

        _shown = false;
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case InteropValues.WM_NCHITTEST:
                int x = lParam.ToInt32() << 16 >> 16, y = lParam.ToInt32() >> 16;
                Point pos = _elementAttachedTo.PointFromScreen(new Point(x, y));

                //bottom,left, top, Right, 
                if (pos.Y > _elementAttachedTo.ActualHeight - ResizeBorderThickness ||
                    pos.X > _elementAttachedTo.ActualWidth - ResizeBorderThickness ||
                    pos.Y <= ResizeBorderThickness ||
                    pos.X <= ResizeBorderThickness)
                {
                    // 这里，我强行让所有区域返回 HTTRANSPARENT，于是整个子窗口都交给父窗口处理消息。
                    // 正常，你应该在这里计算窗口边缘。
                    handled = true;
                    return new IntPtr(HTTRANSPARENT);
                }
                break;

            case InteropValues.WM_LBUTTONDOWN:
            case InteropValues.WM_RBUTTONDOWN:
                NativeAPI.SetForegroundWindow(hwnd);
                break;

            case InteropValues.WM_ACTIVATE:
                _hwndAdornerGroup?.ActivateInGroupLimits(this);
                break;

            case InteropValues.WM_GETMINMAXINFO:
                unsafe
                {
                    MINMAXINFO* minMaxInfo = (MINMAXINFO*)lParam;
                    minMaxInfo->ptMinTrackSize = new POINT() { X = 0, Y = 0 };
                }

                //var minMaxInfo = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof (MINMAXINFO));
                //minMaxInfo.ptMinTrackSize = new POINT();
                //Marshal.StructureToPtr(minMaxInfo, lParam, true);

                break;
            default:
                break;
        }

        return IntPtr.Zero;
    }


    public void Dispose()
    {
        if (_disposed) return;

        DisconnectFromGroup();
        _hwndAdornmentRoot.SetCurrentValue(ContentControl.ContentProperty, null);
        DisposeHwndSource();

        _elementAttachedTo.Loaded -= OnLoaded;
        _elementAttachedTo.Unloaded -= OnUnloaded;
        _elementAttachedTo.IsVisibleChanged -= OnIsVisibleChanged;
        _elementAttachedTo.LayoutUpdated -= OnLayoutUpdated;

        _disposed = true;
    }
}
