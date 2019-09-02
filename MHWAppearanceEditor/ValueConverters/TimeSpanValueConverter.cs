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
            if (value is string str)
            {
                var split = str.Split(':');
                if (split.Length >= 3
                    && int.TryParse(split[0], out var hours)
                    && int.TryParse(split[1], out var min)
                    && int.TryParse(split[2], out var sec))
                {
                    return new TimeSpan(hours, min, sec);
                }
            }

            throw new NotImplementedException();
        }
    }
}
