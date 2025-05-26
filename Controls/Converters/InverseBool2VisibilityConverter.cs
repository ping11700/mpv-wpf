namespace Controls.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class InverseBool2VisibilityConverter : IValueConverter
{
    //parameter ==0 hidden
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter != null)
        {
            if (parameter.ToString() == "0")
            {
                return value is bool && (bool)value ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                return value is bool && (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        else
        {
            return value is bool && (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}