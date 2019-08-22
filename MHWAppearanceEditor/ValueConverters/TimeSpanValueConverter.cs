using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MHWAppearanceEditor.ValueConverters
{
    public class TimeSpanValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timespan)
                return (int)timespan.TotalHours + ":" + timespan.ToString(@"mm\:ss"); // Yeah this is the easiest way

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
