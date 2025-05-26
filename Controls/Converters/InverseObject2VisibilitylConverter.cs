namespace Controls.Converters;


/// <summary>
///  object 为null or OrEmpty 返回 Visibility.Visible
/// </summary>
public class InverseObject2VisibilitylConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not null)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }

        return Visibility.Visible;


    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
