namespace Controls.Control;

/// <summary>
/// BusyBox.xaml 的交互逻辑
/// </summary>
public partial class BusyBox : System.Windows.Controls.ProgressBar
{
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
        "IsActive", typeof(bool), typeof(BusyBox), new PropertyMetadata(ValueBoxes.FalseBox));



    static BusyBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyBox), new FrameworkPropertyMetadata(typeof(BusyBox)));
    }
}