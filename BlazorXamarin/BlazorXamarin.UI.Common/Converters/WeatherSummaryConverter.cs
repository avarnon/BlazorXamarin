using System;
using BlazorXamarin.Application.Contracts;
using Xamarin.Forms;

namespace BlazorXamarin.UI.Common.Converters
{
    public class WeatherSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new InvalidOperationException("The target must be a String");
            if (value == null || value.GetType() != typeof(string)) throw new InvalidOperationException("The target must be a string");

            ITranslationService translationService = (ITranslationService)App.Current.Container.Resolve(typeof(ITranslationService));

            return translationService[$"FetchData__Summary_{value}"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
