using System;
using BlazorXamarin.Application.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlazorXamarin.UI.Common
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension<string>
    {
        /// <summary>
        /// The text to be translated.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Returns the translation for the specified text.
        /// </summary>
        /// <param name="serviceProvider">The service that provides the value.</param>
        /// <returns>The translation.</returns>
        public virtual string ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Translate();
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Translate();
        }

        protected string Translate()
        {
            if (this.Text == null) return string.Empty;

            ITranslationService translationService = (ITranslationService)App.Current.Container.Resolve(typeof(ITranslationService));

            return translationService[this.Text];
        }
    }
}
