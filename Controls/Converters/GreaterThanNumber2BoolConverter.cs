namespace Controls.Converters;


public class GreaterThanNumber2BoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var param = System.Convert.ToDouble(parameter);
            var val = System.Convert.ToDouble(value);
            if (double.IsNaN(param) || double.IsNaN(val))
            {
                return false;
            }
            if (val - param > 0)
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
