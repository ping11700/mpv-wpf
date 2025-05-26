using Brush = System.Windows.Media.Brush;
using Size = System.Windows.Size;

namespace Controls.Hwnd;
/// <summary>
/// A custom control for managing an HwndHost child and presenting an Adornment over it. 
/// Inherited classes must control the access and life cycle of the HwndHost child
/// </summary>
public class HwndHostPresenter : FrameworkElement, IDisposable
{
    private readonly HwndAdorner _hwndAdorner;
    private UIElement? _child;
    private HwndHost? _hwndHost;
    private UIElement? _adornment;
    private bool _mouseIsOverHwnd;

    #region Mouse Enter / Leave

    public static readonly RoutedEvent HwndMouseEnterEvent = EventManager.RegisterRoutedEvent(
        "HwndMouseEnter", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(HwndHostPresenter));

    public static readonly RoutedEvent HwndMouseLeaveEvent = EventManager.RegisterRoutedEvent(
        "HwndMouseLeave", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(HwndHostPresenter));

    #endregion

    static HwndHostPresenter()
    {
        EventManager.RegisterClassHandler(typeof(HwndHostPresenter), MouseEnterEvent, new RoutedEventHandler(OnMouseEnterOrLeave));
        EventManager.RegisterClassHandler(typeof(HwndHostPresenter), MouseLeaveEvent, new RoutedEventHandler(OnMouseEnterOrLeave));

        EventManager.RegisterClassHandler(typeof(HwndHostPresenter), HwndHostPresenter.HwndMouseEnterEvent, new RoutedEventHandler(OnHwndMouseEnterOrLeave));
        EventManager.RegisterClassHandler(typeof(HwndHostPresenter), HwndHostPresenter.HwndMouseLeaveEvent, new RoutedEventHandler(OnHwndMouseEnterOrLeave));
    }

    public HwndHostPresenter()
    {
        _hwndAdorner = new HwndAdorner(this);
        AddLogicalChild(_hwndAdorner.Root);
    }

    /// <summary>
    /// The only visual child
    /// </summary>
    private UIElement? Child
    {
        get => _child;
        set
        {
            if (_child == value) return;

            RemoveVisualChild(_child);

            _child = value;

            AddVisualChild(value);
            InvalidateMeasure();
        }
    }

    public HwndHost? HwndHost
    {
        get => _hwndHost;
        set
        {
            if (_hwndHost == value) return;

            RemoveLogicalChild(_hwndHost);

            _hwndHost = value;

            AddLogicalChild(value);
            if (Hosting)
            {
                Child = value;
            }
        }
    }

    public UIElement? Adornment
    {
        get => _adornment;
        set
        {
            if (_adornment == value) return;

            _adornment = value;
            _hwndAdorner.Adornment = _adornment;
        }
    }

    /// <summary> 
    /// The Adorner Root is always a logical child
    /// so is The HwndHost if exists
    /// </summary>
    protected override IEnumerator LogicalChildren
    {
        get
        {
            if (_hwndHost != null)
            {
                yield return _hwndHost;
            }
            yield return _hwndAdorner.Root;
        }
    }

    /// <summary>
    /// Returns the Visual children count.
    /// </summary>
    protected override int VisualChildrenCount => _child is null ? 0 : 1;

    /// <summary>
    /// Returns the child at the specified index.
    /// </summary>
    protected override Visual GetVisualChild(int index)
    {
        if ((_child == null) || (index != 0))
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "presenter has one child at the most");
        }

        return _child;
    }

    protected override Size MeasureOverride(Size constraint)
    {
        if (Child is not null)
        {
            Child.Measure(constraint);
            return Child.DesiredSize;
        }
        return Size.Empty;
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        Child?.Arrange(new Rect(arrangeSize));
        return arrangeSize;
    }

    /// <summary>
    ///     Fills in the background based on the Background property.
    /// </summary>
    protected override void OnRender(DrawingContext dc)
    {
        if (Background is not null)
        {
            // Using the Background brush, draw a rectangle that fills the
            // render bounds of the panel.
            dc.DrawRectangle(Background, null, new Rect(RenderSize));
        }

        base.OnRender(dc);
    }

    /// <summary>
    /// DependencyProperty for <see cref="Background" /> property.
    /// </summary>
    public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background), typeof(Brush), typeof(HwndHostPresenter), new FrameworkPropertyMetadata(null,
                            FrameworkPropertyMetadataOptions.AffectsRender |
                            FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

    /// <summary>
    /// The Background property defines the brush used to fill the area between borders.
    /// </summary>
    public Brush? Background
    {
        get => (Brush?)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    // A property maintaining the Mouse Over state for all content - including an 
    // HwndHost with a Message Loop on another thread
    // HwndHost childs should raise the HwndExtensions.HwndMouseXXX routed events

    private readonly static DependencyPropertyKey IsMouseOverOverridePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsMouseOverOverride), typeof(bool), typeof(HwndHostPresenter), new PropertyMetadata(false));

    /// <summary>Identifies the <see cref="IsMouseOverOverride"/> dependency property.</summary>
    public readonly static DependencyProperty IsMouseOverOverrideProperty = IsMouseOverOverridePropertyKey.DependencyProperty;

    public bool IsMouseOverOverride => (bool)GetValue(IsMouseOverOverrideProperty);

    private static void OnMouseEnterOrLeave(object sender, RoutedEventArgs e)
    {
        var presenter = (HwndHostPresenter)sender;
        presenter.InvalidateMouseOver();
    }

    private static void OnHwndMouseEnterOrLeave(object sender, RoutedEventArgs e)
    {
        var presenter = (HwndHostPresenter)sender;

        // Handling this routed event only if its coming from our direct child
        if (e.OriginalSource == presenter._hwndHost)
        {
            presenter._mouseIsOverHwnd = e.RoutedEvent == HwndHostPresenter.HwndMouseEnterEvent;
            presenter.InvalidateMouseOver();
        }
    }

    private void InvalidateMouseOver()
    {
        bool over = IsMouseOver || (_hwndHost != null && _mouseIsOverHwnd);

        SetValue(IsMouseOverOverridePropertyKey, over);
    }

    public static readonly DependencyProperty HostingProperty = DependencyProperty.Register(
        nameof(Hosting), typeof(bool), typeof(HwndHostPresenter), new UIPropertyMetadata(true, OnHostingChanged));

    public bool Hosting
    {
        get => (bool)GetValue(HostingProperty);
        set => SetValue(HostingProperty, value);
    }

    private static void OnHostingChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
    {
        var presenter = (HwndHostPresenter)d;
        presenter.OnHostingChanged((bool)args.NewValue);
    }

    private void OnHostingChanged(bool hosting) => Child = hosting ? _hwndHost : null;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    /// <summary>
    /// Inherited classes should decide whether to dispose the HwndHost child
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            HwndHost?.Dispose();
        }
    }
}
