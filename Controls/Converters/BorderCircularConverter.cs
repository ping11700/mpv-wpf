namespace Controls.Converters;

[ValueConversion(typeof(double[]), typeof(CornerRadius))]
public class BorderCircularConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(CornerRadius))
            throw new InvalidOperationException("The target must be a CornerRadius");

        if (values.Length == 2 && values[0] is double width && values[1] is double height)
        {
            if (width < double.Epsilon || height < double.Epsilon)
            {
                return new CornerRadius();
            }

            var min = Math.Min(width, height);
            return new CornerRadius(min / 2);
        }

        return DependencyProperty.UnsetValue;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}