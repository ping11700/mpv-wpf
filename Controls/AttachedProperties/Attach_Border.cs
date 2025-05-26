namespace Controls.AttachedProperties;

/// <summary>
///  附加属性 Border
/// </summary>
public class Attach_Border
{
    /// <summary>
    /// Border 圆角
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(Attach_Border), new FrameworkPropertyMetadata(default(CornerRadius)));
    public static void SetCornerRadius(DependencyObject element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);
    public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius)element.GetValue(CornerRadiusProperty);



    /// <summary>
    /// Border 是否圆形
    /// </summary>
    public static readonly DependencyProperty CircularProperty = DependencyProperty.RegisterAttached(
           "Circular", typeof(bool), typeof(Attach_Border), new PropertyMetadata(ValueBoxes.FalseBox, OnCircularChanged));
    public static void SetCircular(DependencyObject element, bool value) => element.SetValue(CircularProperty, ValueBoxes.BooleanBox(value));
    public static bool GetCircular(DependencyObject element) => (bool)element.GetValue(CircularProperty);
    private static void OnCircularChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Border border)
        {
            if ((bool)e.NewValue)
            {
                var binding = new MultiBinding
                {
                    Converter = new BorderCircularConverter()
                };
                binding.Bindings.Add(new System.Windows.Data.Binding(FrameworkElement.ActualWidthProperty.Name) { Source = border });
                binding.Bindings.Add(new System.Windows.Data.Binding(FrameworkElement.ActualHeightProperty.Name) { Source = border });
                border.SetBinding(Border.CornerRadiusProperty, binding);
            }
            else
            {
                BindingOperations.ClearBinding(border, FrameworkElement.ActualWidthProperty);
                BindingOperations.ClearBinding(border, FrameworkElement.ActualHeightProperty);
                BindingOperations.ClearBinding(border, Border.CornerRadiusProperty);
            }
        }
    }
}