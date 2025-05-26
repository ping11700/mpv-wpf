
namespace Controls.Converters;


public class Bool2VisibilityConverter : IValueConverter
{
    //parameter ==0 hidden
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter != null)
        {
            if (parameter.ToString() == "0")
            {
                return value is bool boolean && boolean ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                return value is bool boolean && boolean ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        else
        {
            return value is bool boolean && boolean ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}