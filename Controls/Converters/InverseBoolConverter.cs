namespace Controls.Converters;

[ValueConversion(typeof(bool), typeof(bool))]
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            if (targetType != typeof(bool?))
            {
                return Binding.DoNothing;
            }
        }

        if (value != null)
        {
            return !(bool)value;
        }
        else
        {
            return false;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            if (targetType != typeof(bool?))
            {
                return Binding.DoNothing;
            }
        }
        if (value is bool b)
        {
            return !b;
        }
        return System.Windows.Data.Binding.DoNothing;
    }
}