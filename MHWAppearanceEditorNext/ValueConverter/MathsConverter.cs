using System;
using System.Globalization;
using System.Windows.Data;

namespace MHWAppearanceEditorNext.ValueConverter
{
    public class ValueGreaterThanConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if value is larger than paramter, otherwise returns false
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>(bool)(value > parameter)</returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal a = Convert.ToInt32(value);
            decimal b = Convert.ToInt32(parameter);

            return a > b;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ValueSmallerThanConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if value is smaller than paramter, otherwise returns false
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>(bool)(value < parameter)</returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal a = Convert.ToInt32(value);
            decimal b = Convert.ToInt32(parameter);

            return a < b;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
