using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MHWAppearanceEditor.ValueConverters
{
    public class IsNotNullToTrueValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null!;
        }
    }
}
