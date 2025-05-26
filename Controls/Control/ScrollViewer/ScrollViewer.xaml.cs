namespace Controls.Control;

/// <summary>
/// 动画 ScrollViewer
/// </summary>
public class ScrollViewer : System.Windows.Controls.ScrollViewer
{

    /// <summary>
    ///     滚动方向
    /// </summary>
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static void SetOrientation(DependencyObject element, Orientation value) => element.SetValue(OrientationProperty, value);
    public static Orientation GetOrientation(DependencyObject element) => (Orientation)element.GetValue(OrientationProperty);
    /// <summary>
    ///     滚动方向
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached(
        "Orientation", typeof(Orientation), typeof(ScrollViewer), new PropertyMetadata(Orientation.Vertical));



    /// <summary>
    ///   是否自动隐藏
    /// </summary>
    public bool AutoHide
    {
        get => (bool)GetValue(AutoHideProperty);
        set => SetValue(AutoHideProperty, ValueBoxes.BooleanBox(value));
    }

    public static void SetAutoHide(DependencyObject element, bool value) => element.SetValue(AutoHideProperty, ValueBoxes.BooleanBox(value));
    public static bool GetAutoHide(DependencyObject element) => (bool)element.GetValue(AutoHideProperty);
    /// <summary>
    /// 是否自动隐藏
    /// </summary>
    public static readonly DependencyProperty AutoHideProperty = DependencyProperty.RegisterAttached(
            "AutoHide", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.TrueBox));





    /// <summary>
    ///     是否响应鼠标滚轮操作
    /// </summary>
    public bool CanMouseWheel
    {
        get => (bool)GetValue(CanMouseWheelProperty);
        set => SetValue(CanMouseWheelProperty, ValueBoxes.BooleanBox(value));
    }
    public static void SetCanMouseWheel(DependencyObject element, bool value) => element.SetValue(CanMouseWheelProperty, ValueBoxes.BooleanBox(value));
    public static bool GetCanMouseWheel(DependencyObject element) => (bool)element.GetValue(CanMouseWheelProperty);
    /// <summary>
    ///     是否响应鼠标滚轮操作
    /// </summary>
    public static readonly DependencyProperty CanMouseWheelProperty = DependencyProperty.RegisterAttached(
        "CanMouseWheel", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.TrueBox));




    /// <summary>
    ///     是否显示上一页/下一页
    /// </summary>
    public bool ShowPageButton
    {
        get => (bool)GetValue(ShowPageButtonProperty);
        set => SetValue(ShowPageButtonProperty, ValueBoxes.BooleanBox(value));
    }

    public static void SetShowPageButton(DependencyObject element, bool value) => element.SetValue(ShowPageButtonProperty, ValueBoxes.BooleanBox(value));
    public static bool GetShowPageButton(DependencyObject element) => (bool)element.GetValue(ShowPageButtonProperty);

    /// <summary>
    ///    是否显示上一页/下一页
    /// </summary>
    public static readonly DependencyProperty ShowPageButtonProperty = DependencyProperty.RegisterAttached(
        "ShowPageButton", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.FalseBox));



    /// <summary>
    ///     是否显示上一页/下一页
    /// </summary>
    public bool OnlyHasithPageButton
    {
        get => (bool)GetValue(OnlyHasithPageButtonProperty);
        set => SetValue(OnlyHasithPageButtonProperty, ValueBoxes.BooleanBox(value));
    }

    public static void SetOnlyHasithPageButton(DependencyObject element, bool value) => element.SetValue(OnlyHasithPageButtonProperty, ValueBoxes.BooleanBox(value));
    public static bool GetOnlyHasithPageButtonn(DependencyObject element) => (bool)element.GetValue(OnlyHasithPageButtonProperty);

    /// <summary>
    ///    上一页/下一页
    /// </summary>
    public static readonly DependencyProperty OnlyHasithPageButtonProperty = DependencyProperty.RegisterAttached(
        "OnlyHasithPageButton", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.FalseBox));






    /// <summary>
    ///     是否支持惯性
    /// </summary>
    public bool IsInertiaEnabled
    {
        get => (bool)GetValue(IsInertiaEnabledProperty);
        set => SetValue(IsInertiaEnabledProperty, ValueBoxes.BooleanBox(value));
    }


    public static void SetIsInertiaEnabled(DependencyObject element, bool value) => element.SetValue(IsInertiaEnabledProperty, ValueBoxes.BooleanBox(value));
    public static bool GetIsInertiaEnabled(DependencyObject element) => (bool)element.GetValue(IsInertiaEnabledProperty);


    /// <summary>
    ///     是否支持惯性
    /// </summary>
    public static readonly DependencyProperty IsInertiaEnabledProperty = DependencyProperty.RegisterAttached(
        "IsInertiaEnabled", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.TrueBox));






    /// <summary>
    ///     控件是否可以穿透点击
    /// </summary>
    public bool IsPenetrating
    {
        get => (bool)GetValue(IsPenetratingProperty);
        set => SetValue(IsPenetratingProperty, ValueBoxes.BooleanBox(value));
    }

    public static void SetIsPenetrating(DependencyObject element, bool value) => element.SetValue(IsPenetratingProperty, ValueBoxes.BooleanBox(value));
    public static bool GetIsPenetrating(DependencyObject element) => (bool)element.GetValue(IsPenetratingProperty);

    /// <summary>
    ///     控件是否可以穿透点击
    /// </summary>
    public static readonly DependencyProperty IsPenetratingProperty = DependencyProperty.RegisterAttached(
        "IsPenetrating", typeof(bool), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.FalseBox));



    /// <summary>
    ///  滚动
    /// </summary>
    public ICommand ScrollCmd
    {
        get => (ICommand)GetValue(ScrollCmdProperty);
        set => SetValue(ScrollCmdProperty, value);
    }

    public static void SetScrollCmd(DependencyObject element, ICommand value) => element.SetValue(ScrollCmdProperty, value);
    public static ICommand GetScrollCmd(DependencyObject element) => (ICommand)element.GetValue(ScrollCmdProperty);

    // Using a DependencyProperty as the backing store for ScrollCmd.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ScrollCmdProperty = DependencyProperty.RegisterAttached(
        "ScrollCmd", typeof(ICommand), typeof(ScrollViewer), new PropertyMetadata(default(ICommand)));

    /// <summary>
    ///  调用 加载数据 Cmd 的长度差 
    /// </summary>
    public double CallScrollCmdLength
    {
        get => (double)GetValue(CallScrollCmdLengthProperty);
        set => SetValue(CallScrollCmdLengthProperty, value);
    }

    public static void SetCallScrollCmdLength(DependencyObject element, double value) => element.SetValue(CallScrollCmdLengthProperty, value);
    public static double GetCallScrollCmdLength(DependencyObject element) => (double)element.GetValue(CallScrollCmdLengthProperty);

    // Using a DependencyProperty as the backing store for ScrollCmd.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CallScrollCmdLengthProperty = DependencyProperty.RegisterAttached(
        nameof(CallScrollCmdLength), typeof(double), typeof(ScrollViewer), new PropertyMetadata(360d));




    /// <summary>
    ///     当前垂直滚动偏移
    /// </summary>
    internal double CurrentVerticalOffset
    {
        // ReSharper disable once UnusedMember.Local
        get => (double)GetValue(CurrentVerticalOffsetProperty);
        set => SetValue(CurrentVerticalOffsetProperty, value);
    }

    /// <summary>
    ///     当前垂直滚动偏移
    /// </summary>
    internal static readonly DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register(
        "CurrentVerticalOffset", typeof(double), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.Double1Box, OnCurrentVerticalOffsetChanged));

    private static void OnCurrentVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewer ctl && e.NewValue is double v)
        {
            ctl.ScrollToVerticalOffset(v);
        }
    }


    /// <summary>
    ///     当前水平滚动偏移
    /// </summary>
    internal double CurrentHorizontalOffset
    {
        get => (double)GetValue(CurrentHorizontalOffsetProperty);
        set => SetValue(CurrentHorizontalOffsetProperty, value);
    }

    /// <summary>
    ///     当前水平滚动偏移
    /// </summary>
    internal static readonly DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register(
        "CurrentHorizontalOffset", typeof(double), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.Double0Box, OnCurrentHorizontalOffsetChanged));

    private static void OnCurrentHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewer ctl && e.NewValue is double v)
        {
            ctl.ScrollToHorizontalOffset(v);
        }
    }


    /// <summary>
    ///    滚动速率
    /// </summary>
    internal double SpeedRate
    {
        // ReSharper disable once UnusedMember.Local
        get => (double)GetValue(SpeedRateProperty);
        set => SetValue(SpeedRateProperty, value);
    }
    public static void SetSpeedRate(DependencyObject element, double value) => element.SetValue(SpeedRateProperty, value);
    public static double GetSpeedRate(DependencyObject element) => (double)element.GetValue(SpeedRateProperty);
    /// <summary>
    ///     当前垂直滚动偏移
    /// </summary>
    internal static readonly DependencyProperty SpeedRateProperty = DependencyProperty.RegisterAttached(
        "SpeedRate", typeof(double), typeof(ScrollViewer), new PropertyMetadata(ValueBoxes.Double1Box));



    protected override HitTestResult? HitTestCore(PointHitTestParameters hitTestParameters) => IsPenetrating ? null : base.HitTestCore(hitTestParameters);


  
 

    //记录上一次的滚动位置
    private double _lastLocation = 0;

     
 
    #region Touch
    private double _touchVerticalOffset;
    private TouchPoint? lastTouchDownPoint;
    protected override void OnPreviewTouchDown(TouchEventArgs e)
    {
        base.OnPreviewTouchDown(e);
        if (e.OriginalSource is DependencyObject dependencyObject)
        {
            var scrollViewer = Util_Visual.FindParent<ScrollViewer>(dependencyObject);
            if (scrollViewer != null && scrollViewer != this)
            {
                lastTouchDownPoint = e.GetTouchPoint(this);
                _touchVerticalOffset = VerticalOffset;
                CurrentVerticalOffset = VerticalOffset;
            }
        }
    }
    protected override void OnPreviewTouchUp(TouchEventArgs e)
    {
        base.OnPreviewTouchUp(e);
        if (e.OriginalSource is ScrollViewer scrollViewer)
        {
            if (scrollViewer != this)
            {
                lastTouchDownPoint = null;
            }
        }
    }

    protected override void OnPreviewTouchMove(TouchEventArgs e)
    {
        base.OnPreviewTouchMove(e);
        if (e.OriginalSource is ScrollViewer scrollViewer)
        {
            if (scrollViewer != this)
            {
                var currentTouchPoint = e.GetTouchPoint(this);
                if (lastTouchDownPoint != null)
                {
                    var yOffset = currentTouchPoint.Position.Y - lastTouchDownPoint.Position.Y;
                    var xOffset = currentTouchPoint.Position.X - lastTouchDownPoint.Position.X;

                    if (Math.Abs(yOffset) > Math.Abs(xOffset))
                    {
                        e.Handled = true;

                        _touchVerticalOffset = Math.Min(Math.Max(0, _touchVerticalOffset -= yOffset), ScrollableHeight);
                        AnimateScroll(_touchVerticalOffset);

                    }
                }
                lastTouchDownPoint = currentTouchPoint;
            }
        }
        //向下滚动加载
        CheckLoadMoreData();
    }

    #endregion

    private void CheckLoadMoreData()
    {
        //向下滚动加载
        if (ScrollableHeight - VerticalOffset <= CallScrollCmdLength)
        {
            if (ScrollCmd != null && ScrollCmd.CanExecute(null))
            {
                ScrollCmd.Execute(null);
            }
        }
    }

    /// <summary>
    ///  鼠标滚动事件
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        double WheelChange = e.Delta;
        //可以更改一次滚动的距离倍数 (WheelChange可能为正负数!)
        double newOffset = _lastLocation - (WheelChange * 2);
        //Animation并不会改变真正的VerticalOffset(只是它的依赖属性) 所以将VOffset设置到上一次的滚动位置 (相当于衔接上一个动画)
        ScrollToVerticalOffset(_lastLocation);
        //碰到底部和顶部时的处理
        if (newOffset < 0)
            newOffset = 0;
        if (newOffset > ScrollableHeight)
            newOffset = ScrollableHeight;

        AnimateScroll(newOffset);
        _lastLocation = newOffset;
        //告诉ScrollViewer我们已经完成了滚动
        e.Handled = true;

    }


    private void AnimateScroll(double ToValue)
    {
        //为了避免重复，先结束掉上一个动画
        BeginAnimation(CurrentVerticalOffsetProperty, null);
        var Animation = new DoubleAnimation
        {
            EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
            From = VerticalOffset,
            To = ToValue,
            //动画速度
            Duration = TimeSpan.FromMilliseconds(800)
        };
        //考虑到性能，可以降低动画帧数
        //Timeline.SetDesiredFrameRate(Animation, 40);
        BeginAnimation(CurrentVerticalOffsetProperty, Animation);
    }

}
