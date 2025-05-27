using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

namespace Controls.Control;


public class PlayerSlider : Slider
{
    #region 字段

    private BaseAdorner? _adorner;

    private const string TrackKey = "PART_Track";

    private const string ThumbKey = "PART_Thumb";

    private FrameworkElement? _previewContent = null;

    private FrameworkElement? _thumb;

    private TranslateTransform? _transform;

    private Track? _track;


    #endregion 字段

    #region 属性

    public ControlTemplate ThumbTemplate
    {
        get => (ControlTemplate)GetValue(ThumbTemplateProperty);
        set => SetValue(ThumbTemplateProperty, value);
    }

    // Using a DependencyProperty as the backing store for ThumbTemplate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ThumbTemplateProperty =
        DependencyProperty.Register("ThumbTemplate", typeof(ControlTemplate), typeof(PlayerSlider), new PropertyMetadata(null, OnThumbTemplateChanged));

    private static void OnThumbTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }

    /// <summary>
    ///     预览内容
    /// </summary>
    public static readonly DependencyProperty PreviewContentProperty = DependencyProperty.Register(
        "PreviewContent", typeof(object), typeof(PlayerSlider), new PropertyMetadata(default(object)));

    /// <summary>
    ///     预览内容
    /// </summary>
    public object PreviewContent
    {
        get => GetValue(PreviewContentProperty);
        set => SetValue(PreviewContentProperty, value);
    }

    public static readonly DependencyProperty PreviewContentOffsetProperty = DependencyProperty.Register(
        "PreviewContentOffset", typeof(double), typeof(PlayerSlider), new PropertyMetadata(9.0));

    public double PreviewContentOffset
    {
        get => (double)GetValue(PreviewContentOffsetProperty);
        set => SetValue(PreviewContentOffsetProperty, value);
    }

    public static readonly DependencyProperty PreviewPositionProperty = DependencyProperty.RegisterAttached(
        "PreviewPosition", typeof(double), typeof(PlayerSlider), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetPreviewPosition(DependencyObject? element, double value)
    {
        element?.SetValue(PreviewPositionProperty, value);
    }

    public static double GetPreviewPosition(DependencyObject? element)
    {
        if (element != null)
        {
            return (double)element.GetValue(PreviewPositionProperty);
        }
        else
        {
            return Double.NaN;
        }
    }

    public double PreviewPosition
    {
        get => GetPreviewPosition(_previewContent);
        set
        {
            SetPreviewPosition(_previewContent, value);
            if (ThumbIsPressed)
            {
                Value = value;
            }
        }
    }

    public long CacheValue
    {
        get => (long)GetValue(CacheValueProperty);
        set => SetValue(CacheValueProperty, value);
    }

    // Using a DependencyProperty as the backing store for CacheValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CacheValueProperty =
        DependencyProperty.Register("CacheValue", typeof(long), typeof(PlayerSlider), new PropertyMetadata(ValueBoxes.Long0Box));

    public Brush CacheBackground
    {
        get => (Brush)GetValue(CacheBackgroundProperty);
        set => SetValue(CacheBackgroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for CacheBackground.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CacheBackgroundProperty =
        DependencyProperty.Register("CacheBackground", typeof(Brush), typeof(PlayerSlider), new PropertyMetadata(Brushes.Transparent));

    public Brush PastBackground
    {
        get => (Brush)GetValue(PastBackgroundProperty);
        set => SetValue(PastBackgroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for PastBackground.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PastBackgroundProperty =
        DependencyProperty.Register("PastBackground", typeof(Brush), typeof(PlayerSlider), new PropertyMetadata(Brushes.Transparent));

    public Brush NotyetBackground
    {
        get => (Brush)GetValue(NotyetBackgroundProperty);
        set => SetValue(NotyetBackgroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for NotyetBackground.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NotyetBackgroundProperty =
        DependencyProperty.Register("NotyetBackground", typeof(Brush), typeof(PlayerSlider), new PropertyMetadata(Brushes.Transparent));

    public double TrackHeight
    {
        get => (double)GetValue(TrackHeightProperty);
        set => SetValue(TrackHeightProperty, value);
    }

    // Using a DependencyProperty as the backing store for TrackHeight.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TrackHeightProperty =
        DependencyProperty.Register("TrackHeight", typeof(double), typeof(PlayerSlider), new PropertyMetadata(4.0d));

    public long WantValue
    {
        get => (long)GetValue(WantValueProperty);
        set => SetValue(WantValueProperty, value);
    }

    // Using a DependencyProperty as the backing store for WantValue.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty WantValueProperty =
        DependencyProperty.Register("WantValue", typeof(long), typeof(PlayerSlider), new PropertyMetadata(ValueBoxes.Long0Box, OnWantValueChanged));

    private static void OnWantValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PlayerSlider playerSlider)
        {
            if (!playerSlider.ThumbIsPressed && !(playerSlider._thumb?.IsMouseCaptured ?? false))
                playerSlider.Value = (long)e.NewValue;
        }
    }


    public bool ThumbIsPressed
    {
        get => (bool)GetValue(ThumbIsPressedProperty);
        private set
        {
            SetValue(ThumbIsPressedProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for ThumbIsPressed.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ThumbIsPressedProperty =
        DependencyProperty.Register("ThumbIsPressed", typeof(bool), typeof(PlayerSlider), new PropertyMetadata(false));

    public Visibility ThumbVisibility
    {
        get => (Visibility)GetValue(ThumbVisibilityProperty);
        set => SetValue(ThumbVisibilityProperty, value);
    }

    // Using a DependencyProperty as the backing store for ThumbVisiable.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ThumbVisibilityProperty =
        DependencyProperty.Register("ThumbVisibility", typeof(Visibility), typeof(PlayerSlider), new PropertyMetadata(Visibility.Visible));

    #endregion 属性

    #region 事件

    /// <summary>
    ///     Max值改变事件
    /// </summary>
    public static readonly RoutedEvent MaxValueChangedEvent =
        EventManager.RegisterRoutedEvent("MaxValueChanged", RoutingStrategy.Bubble,
            typeof(EventHandler), typeof(PlayerSlider));

    /// <summary>
    ///     Max值改变事件
    /// </summary>
    public event EventHandler MaxValueChanged
    {
        add => AddHandler(MaxValueChangedEvent, value);
        remove => RemoveHandler(MaxValueChangedEvent, value);
    }



    /// <summary>
    ///     值改变事件
    /// </summary>
    public static readonly RoutedEvent PreviewPositionChangedEvent =
        EventManager.RegisterRoutedEvent("PreviewPositionChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<double>), typeof(PlayerSlider));

    /// <summary>
    ///     值改变事件
    /// </summary>
    public event EventHandler<RoutedPropertyChangedEventHandler<double>> PreviewPositionChanged
    {
        add => AddHandler(PreviewPositionChangedEvent, value);
        remove => RemoveHandler(PreviewPositionChangedEvent, value);
    }

    /// <summary>
    ///     值改变事件
    /// </summary>
    public static readonly RoutedEvent DropValueChangedEvent =
        EventManager.RegisterRoutedEvent("DropValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<double>), typeof(PlayerSlider));

    /// <summary>
    ///     值改变事件
    /// </summary>
    public event RoutedPropertyChangedEventHandler<double> DropValueChanged
    {
        add => AddHandler(DropValueChangedEvent, value);
        remove => RemoveHandler(DropValueChangedEvent, value);
    }

    protected virtual void OnDropValueChanged(double oldValue, double newValue)
    {
        var arg = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, DropValueChangedEvent);
        RaiseEvent(arg);
    }

    private double _thumbIsPressedValue = 0;

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonDown(e);
        ThumbIsPressed = true;
        _thumbIsPressedValue = PreviewPosition;
    }

    protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (ThumbIsPressed)
        {
            ThumbIsPressed = false;
            OnDropValueChanged(_thumbIsPressedValue, PreviewPosition);
        }
        base.OnPreviewMouseLeftButtonUp(e);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (ThumbIsPressed)
        {
            ThumbIsPressed = false;
            OnDropValueChanged(_thumbIsPressedValue, PreviewPosition);
        }
        base.OnMouseLeftButtonUp(e);
    }

    protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
    {
        if (ThumbIsPressed)
        {
            ThumbIsPressed = false;
            OnDropValueChanged(_thumbIsPressedValue, PreviewPosition);
        }
        base.OnPreviewMouseUp(e);
    }

    private void Track_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (ThumbIsPressed)
        {
            ThumbIsPressed = false;
            OnDropValueChanged(_thumbIsPressedValue, PreviewPosition);
        }
    }






    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (ThumbIsPressed)
        {
            ThumbIsPressed = false;
            OnDropValueChanged(_thumbIsPressedValue, PreviewPosition);
        }
        base.OnMouseUp(e);
    }

    protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
    {
        if (ThumbIsPressed)
        {
            ThumbIsPressed = false;
            OnDropValueChanged(_thumbIsPressedValue, PreviewPosition);
        }
        base.OnPreviewMouseRightButtonUp(e);
    }

    protected override void OnLostMouseCapture(MouseEventArgs e)
    {
        this.ThumbIsPressed = false;
        base.OnLostMouseCapture(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (_previewContent == null) return;
        if (_thumb == null) return;
        if (_track == null) return;
        if (_transform == null) return;
        var p = e.GetPosition(_adorner);
        var maximum = Maximum;
        var minimum = Minimum;

        if (Orientation == Orientation.Horizontal)
        {
            var pos = !IsDirectionReversed
                ? (e.GetPosition(_track).X) / _track.ActualWidth * (maximum - minimum) + minimum
                : (1 - (e.GetPosition(_track).X) / _track.ActualWidth) * (maximum - minimum) + minimum;
            if (pos > maximum || pos < 0)
            {
                if (_thumb.IsMouseCaptureWithin)
                {
                    PreviewPosition = Value;
                }
                return;
            }

            _transform.X = p.X - _previewContent.ActualWidth * 0.5;
            _transform.Y = _thumb.TranslatePoint(new Point(), _adorner).Y - _previewContent.ActualHeight - PreviewContentOffset;

            PreviewPosition = _thumb.IsMouseCaptureWithin ? Value : pos;
        }
        else
        {
            var pos = !IsDirectionReversed
                ? (1 - (e.GetPosition(_track).Y - _thumb.ActualHeight * 0.5) / _track.ActualHeight) * (maximum - minimum) + minimum
                : (e.GetPosition(_track).Y - _thumb.ActualHeight * 0.5) / _track.ActualHeight * (maximum - minimum) + minimum;
            if (pos > maximum || pos < 0)
            {
                if (_thumb.IsMouseCaptureWithin)
                {
                    PreviewPosition = Value;
                }
                return;
            }

            _transform.X = _thumb.TranslatePoint(new Point(), _adorner).X - _previewContent.ActualWidth - PreviewContentOffset;
            _transform.Y = p.Y - _previewContent.ActualHeight * 0.5;

            PreviewPosition = _thumb.IsMouseCaptureWithin ? Value : pos;
        }

        RaiseEvent(new RoutedPropertyChangedEventArgs<double>(PreviewPosition, PreviewPosition, PreviewPositionChangedEvent));
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        if (_adorner == null)
        {
            var layer = AdornerLayer.GetAdornerLayer(this);
            if (layer == null) return;
            _adorner = new BaseAdorner(layer)
            {
                Child = _previewContent
            };
            layer.Add(_adorner);
        }
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        var layer = AdornerLayer.GetAdornerLayer(this);
        if (layer != null)
        {
            layer.Remove(_adorner);
        }
        else if (_adorner is { Parent: AdornerLayer parent })
        {
            parent.Remove(_adorner);
        }

        if (_adorner != null)
        {
            _adorner.Child = null;
            _adorner = null;
        }

        this.ThumbIsPressed = false;
    }




    protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
        base.OnMaximumChanged(oldMaximum, newMaximum);

        RaiseEvent(new RoutedEventArgs(MaxValueChangedEvent, this));
    }



    #endregion 事件

    #region 重写

    static PlayerSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PlayerSlider), new FrameworkPropertyMetadata(typeof(PlayerSlider)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var contentControl = new System.Windows.Controls.ContentControl
        {
            DataContext = this
        };
        contentControl.SetBinding(System.Windows.Controls.ContentControl.ContentProperty, new Binding(PreviewContentProperty.Name) { Source = this });
        _previewContent = contentControl;

        _track = Template.FindName(TrackKey, this) as Track;
        _thumb = Template.FindName(ThumbKey, this) as FrameworkElement;

        if (_previewContent != null)
        {
            _transform = new TranslateTransform();

            _previewContent.HorizontalAlignment = HorizontalAlignment.Left;
            _previewContent.VerticalAlignment = VerticalAlignment.Top;
            _previewContent.RenderTransform = _transform;
        }

        if (_track != null)
        {
            _track.PreviewMouseLeftButtonUp -= Track_PreviewMouseLeftButtonUp;
            _track.PreviewMouseLeftButtonUp += Track_PreviewMouseLeftButtonUp;
        }
    }


    #endregion 重写
}
