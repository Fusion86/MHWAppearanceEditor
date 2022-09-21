using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MHWAppearanceEditor.ValueConverters
{
    public class DateTimeOffsetValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset obj)
                return obj.ToString();
            return null!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return DateTimeOffset.Parse(str);
            return null!;
        }
    }
}
