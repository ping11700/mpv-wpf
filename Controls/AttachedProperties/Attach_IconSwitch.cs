namespace Controls.AttachedProperties;

/// <summary>
/// Icon附加属性 可切换icon
/// </summary>
public class Attach_IconSwitch : Attach_Icon
{
    public static readonly DependencyProperty SelectedGeometryProperty = DependencyProperty.RegisterAttached(
       "SelectedGeometry", typeof(Geometry), typeof(Attach_IconSwitch), new PropertyMetadata(default(Geometry)));

    public static void SetSelectedGeometry(DependencyObject element, Geometry value) => element.SetValue(SelectedGeometryProperty, value);
    public static Geometry GetSelectedGeometry(DependencyObject element) => (Geometry)element.GetValue(SelectedGeometryProperty);

    public static readonly DependencyProperty SelectedContentProperty = DependencyProperty.RegisterAttached(
       "SelectedContent", typeof(object), typeof(Attach_IconSwitch), new PropertyMetadata(default));

    public static void SetSelectedContent(DependencyObject element, object value) => element.SetValue(SelectedContentProperty, value);
    public static object GetSelectedContent(DependencyObject element) => (object)element.GetValue(SelectedContentProperty);
}