namespace Controls.AttachedProperties;

/// <summary>
/// window附加属性
/// </summary>
public class Attach_Window
{
    /// <summary>
    /// 只显示关闭
    /// </summary>
    public static readonly DependencyProperty OnlyCloseProperty = DependencyProperty.RegisterAttached(
        "OnlyClose", typeof(bool), typeof(Attach_Window), new FrameworkPropertyMetadata(ValueBoxes.FalseBox));

    public static bool GetOnlyClose(DependencyObject obj) => (bool)obj.GetValue(OnlyCloseProperty);
    public static void SetOnlyClose(DependencyObject obj, bool value) => obj.SetValue(OnlyCloseProperty, ValueBoxes.BooleanBox(value));


    /// <summary>
    /// 是否Resize
    /// </summary>
    public static readonly DependencyProperty IsCanResizeProperty = DependencyProperty.RegisterAttached(
        "IsCanResize", typeof(bool), typeof(Attach_Window), new FrameworkPropertyMetadata(ValueBoxes.FalseBox));

    public static bool GetIsCanResize(DependencyObject obj) => (bool)obj.GetValue(IsCanResizeProperty);
    public static void SetIsCanResize(DependencyObject obj, bool value) => obj.SetValue(IsCanResizeProperty, ValueBoxes.BooleanBox(value));



    /// <summary>
    ///  隐藏窗口操作 最大/正常/最小/关闭
    /// </summary>
    public static readonly DependencyProperty HideOperaProperty = DependencyProperty.RegisterAttached(
        "HideOpera", typeof(bool), typeof(Attach_Window), new FrameworkPropertyMetadata(ValueBoxes.FalseBox));

    public static bool GetHideOpera(DependencyObject obj) => (bool)obj.GetValue(HideOperaProperty);
    public static void SetHideOpera(DependencyObject obj, bool value) => obj.SetValue(HideOperaProperty, ValueBoxes.BooleanBox(value));


    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty IgnoreAltF4Property = DependencyProperty.RegisterAttached(
       "IgnoreAltF4", typeof(bool), typeof(Attach_Window), new PropertyMetadata(ValueBoxes.FalseBox, OnIgnoreAltF4Changed));
    public static void SetIgnoreAltF4(DependencyObject element, bool value) => element.SetValue(IgnoreAltF4Property, ValueBoxes.BooleanBox(value));
    public static bool GetIgnoreAltF4(DependencyObject element) => (bool)element.GetValue(IgnoreAltF4Property);

    private static void OnIgnoreAltF4Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is System.Windows.Window window)
        {
            if ((bool)e.NewValue)
            {
                window.PreviewKeyDown += Window_PreviewKeyDown;
            }
            else
            {
                window.PreviewKeyDown -= Window_PreviewKeyDown;
            }
        }
    }

    private static void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.System && e.SystemKey == Key.F4)
        {
            e.Handled = true;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty ShowInTaskManagerProperty = DependencyProperty.RegisterAttached(
        "ShowInTaskManager", typeof(bool), typeof(Attach_Window), new PropertyMetadata(ValueBoxes.TrueBox, OnShowInTaskManagerChanged));
    public static void SetShowInTaskManager(DependencyObject element, bool value) => element.SetValue(ShowInTaskManagerProperty, ValueBoxes.BooleanBox(value));
    public static bool GetShowInTaskManager(DependencyObject element) => (bool)element.GetValue(ShowInTaskManagerProperty);

    private static void OnShowInTaskManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is System.Windows.Window window)
        {
            var v = (bool)e.NewValue;
            window.SetCurrentValue(System.Windows.Window.ShowInTaskbarProperty, v);

            if (v)
            {
                window.SourceInitialized -= Window_SourceInitialized;
            }
            else
            {
                window.SourceInitialized += Window_SourceInitialized;
            }
        }
    }

    private static void Window_SourceInitialized(object? sender, EventArgs e)
    {
        if (sender is System.Windows.Window window)
        {
            var _ = new WindowInteropHelper(window)
            {
                Owner = API_Window.GetDesktopWindow()
            };
        }
    }

}