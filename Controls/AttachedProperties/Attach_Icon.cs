namespace Controls.AttachedProperties;

/// <summary>
/// Icon附加属性
/// </summary>
public class Attach_Icon
{
    /// <summary>
    /// 图标 Path
    /// </summary>
    public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached(
        "Geometry", typeof(Geometry), typeof(Attach_Icon), new PropertyMetadata(default(Geometry)));

    public static void SetGeometry(DependencyObject element, Geometry value) => element.SetValue(GeometryProperty, value);
    public static Geometry GetGeometry(DependencyObject element) => (Geometry)element.GetValue(GeometryProperty);


    /// <summary>
    /// 图标 Path1
    /// </summary>
    public static readonly DependencyProperty Geometry1Property = DependencyProperty.RegisterAttached(
        "Geometry1", typeof(Geometry), typeof(Attach_Icon), new PropertyMetadata(default(Geometry)));

    public static void SetGeometry1(DependencyObject element, Geometry value) => element.SetValue(Geometry1Property, value);
    public static Geometry GetGeometry1(DependencyObject element) => (Geometry)element.GetValue(Geometry1Property);

    /// <summary>
    /// 图标 颜色
    /// </summary>
    public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached(
        "Color", typeof(System.Windows.Media.Brush), typeof(Attach_Icon), new PropertyMetadata(default(System.Windows.Media.Brush)));

    public static void SetColor(DependencyObject element, System.Windows.Media.Brush value) => element.SetValue(ColorProperty, value);
    public static System.Windows.Media.Brush GetColor(DependencyObject element) => (System.Windows.Media.Brush)element.GetValue(ColorProperty);

    /// <summary>
    /// 图标 颜色
    /// </summary>
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.RegisterAttached(
        "Stroke", typeof(System.Windows.Media.Brush), typeof(Attach_Icon), new PropertyMetadata(default(System.Windows.Media.Brush)));

    public static void SetStroke(DependencyObject element, System.Windows.Media.Brush value) => element.SetValue(StrokeProperty, value);
    public static System.Windows.Media.Brush GetStroke(DependencyObject element) => (System.Windows.Media.Brush)element.GetValue(StrokeProperty);

    /// <summary>
    /// 图标 颜色
    /// </summary>
    public static readonly DependencyProperty FillProperty = DependencyProperty.RegisterAttached(
        "Fill", typeof(System.Windows.Media.Brush), typeof(Attach_Icon), new PropertyMetadata(default(System.Windows.Media.Brush)));

    public static void SetFill(DependencyObject element, System.Windows.Media.Brush value) => element.SetValue(FillProperty, value);
    public static System.Windows.Media.Brush GetFill(DependencyObject element) => (System.Windows.Media.Brush)element.GetValue(FillProperty);



    /// <summary>
    /// 图标 宽度
    /// </summary>
    public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
        "Width", typeof(double), typeof(Attach_Icon), new PropertyMetadata(ValueBoxes.Double0Box));
    public static void SetWidth(DependencyObject element, double value) => element.SetValue(WidthProperty, value);
    public static double GetWidth(DependencyObject element) => (double)element.GetValue(WidthProperty);

    /// <summary>
    /// 图标 高度
    /// </summary>
    public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
        "Height", typeof(double), typeof(Attach_Icon), new PropertyMetadata(ValueBoxes.Double0Box));

    public static void SetHeight(DependencyObject element, double value) => element.SetValue(HeightProperty, value);
    public static double GetHeight(DependencyObject element) => (double)element.GetValue(HeightProperty);


    /// <summary>
    /// 图标 Padding
    /// </summary>
    public static readonly DependencyProperty PaddingProperty = DependencyProperty.RegisterAttached(
        "Padding", typeof(Thickness), typeof(Attach_Icon), new PropertyMetadata(ValueBoxes.Thickness0));

    public static void SetPadding(DependencyObject element, Thickness value) => element.SetValue(PaddingProperty, value);
    public static Thickness GetPadding(DependencyObject element) => (Thickness)element.GetValue(PaddingProperty);

}