using Avalonia.Data.Converters;
using Avalonia.Media;
using MHWAppearanceEditor.Extensions;
using ReactiveUI;
using System;
using System.Globalization;
using MediaColor = Avalonia.Media.Color;
using SDColor = System.Drawing.Color;

namespace MHWAppearanceEditor.ValueConverters
{
    // TODO: This class is a clusterfuck with a lot of duplicated code
    public class ColorValueConverter : IValueConverter, IBindingTypeConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(IBrush))
            {
                if (value is MediaColor color)
                    return new SolidColorBrush(color);
                else if (value is SDColor sdcolor)
                    return new SolidColorBrush(sdcolor.ToMediaColor());
            }
            else if (targetType == typeof(MediaColor))
            {
                if (value is SDColor sdcolor)
                    return sdcolor.ToMediaColor();
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if (fromType == typeof(MediaColor) || fromType == typeof(SDColor))
                return 100;
            return 0;
        }

        public bool TryConvert(object? from, Type toType, object? conversionHint, out object? result)
        {
            if (from is SDColor sdcolor)
            {
                if (toType == typeof(MediaColor))
                {
                    result = sdcolor.ToMediaColor();
                    return true;
                }
            }
            else if (from is MediaColor color)
            {
                if (toType == typeof(SDColor))
                {
                    result = color.ToSDColor();
                    return true;
                }
            }

            result = null!;
            return false;
        }
    }
}
