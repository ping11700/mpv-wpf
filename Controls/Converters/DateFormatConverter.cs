namespace Controls.Converters;

/// <summary>
/// 格式化日期
/// </summary>
[ValueConversion(typeof(object), typeof(string))]
public class DateFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double seconds;
        if (value is long ltimestamp)
        {
            seconds = ltimestamp;
        }
        else if (value is double dtimestamp)
        {
            seconds = dtimestamp;
        }
        else if (value is int itimestamp)
        {
            seconds = itimestamp;
        }
        else if (value is string strtimestamp && double.TryParse(strtimestamp, out double stimestamp))
        {
            seconds = stimestamp;
        }
        else
        {
            return Binding.DoNothing;
        }


        return parameter?.ToString() switch
        {

            "Milliseconds" => TimeSpan.FromMilliseconds(seconds),

            "Seconds" => TimeSpan.FromSeconds(seconds),

            _ => TimeSpan.FromSeconds(seconds),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;

    }
}