using System;
using Xamarin.Forms;

namespace BlazorXamarin.UI.Common.Converters
{
    public class ShortDateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new InvalidOperationException("The target must be a String");
            if (value == null || value.GetType() != typeof(DateTime)) throw new InvalidOperationException("The target must be a DateTime");

            return ((DateTime)value).ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
