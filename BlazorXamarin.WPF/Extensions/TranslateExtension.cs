using System;
using System.Windows.Markup;
using BlazorXamarin.Application.Contracts;
using BlazorXamarin.Application.Services;

namespace BlazorXamarin.WPF
{
    public class TranslateExtension : MarkupExtension
    {
        public string Source { get; set; }

        public string Path { get; set; }

        /// <summary>
        /// Returns the translation for the specified text.
        /// </summary>
        /// <param name="serviceProvider">The service that provides the value.</param>
        /// <returns>The translation.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Translate();
        }

        protected string Translate()
        {
            if (this.Source == null) return string.Empty;

            ITranslationService translationService = new TranslationService(); // This is being called by WPF before Xamarin Forms is finished setting up.
            string translation = translationService[this.Source];

            return translation;
        }
    }
}
