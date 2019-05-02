using System.Globalization;
using System.Threading;
using BlazorXamarin.UI.Common.Contracts;
using BlazorXamarin.UI.Common.Models;
using BlazorXamarin.UI.Common.Services;
using Java.Util;
using Xamarin.Forms;

namespace BlazorXamarin.Droid.Services
{
    public class Localize : BaseLocalize, ILocalize
    {
        public Localize()
        {
            this.OriginalCultureInfo = GetOsCultureInfo();
        }

        protected override CultureInfo OriginalCultureInfo { get; }

        public override CultureInfo CultureInfo
        {
            get => base.CultureInfo;
            set
            {
                Thread.CurrentThread.CurrentCulture = value;
                Thread.CurrentThread.CurrentUICulture = value;

                // We need to change the default Android locale for the app so that images are localized after toggling the language from within the app.
                Java.Util.Locale.Default = new Java.Util.Locale(value.Name.Replace("-", "_"));

                Forms.Context.Resources.Configuration.Locale = Java.Util.Locale.Default;
                Forms.Context.Resources.UpdateConfiguration(Forms.Context.Resources.Configuration, Forms.Context.Resources.DisplayMetrics);

                base.CultureInfo = value;
            }
        }

        private static CultureInfo GetOsCultureInfo()
        {
            Locale androidLocale = Java.Util.Locale.Default;
            string netLanguage = AndroidToDotnetLanguage(androidLocale.ToString().Replace("_", "-"));
            CultureInfo cultureInfo = null;

            try
            {
                cultureInfo = new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException)
            {
                // Android locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                try
                {
                    string fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    cultureInfo = new CultureInfo(fallback);
                }
                catch (CultureNotFoundException)
                {
                    // Android language not valid .NET culture, falling back to English
                    cultureInfo = new CultureInfo(DefaultCulture);
                }
            }

            return cultureInfo;
        }

        /// <summary>
        /// Convert an Android language string to its .NET equivalent.
        /// </summary>
        /// <param name="androidLanguage">The Android language string.</param>
        /// <returns>the .NET language string.</returns>
        private static string AndroidToDotnetLanguage(string androidLanguage)
        {
            string netLanguage = androidLanguage;
            //certain languages need to be converted to CultureInfo equivalent
            switch (androidLanguage)
            {
                case "ms-BN":   // "Malaysian (Brunei)" not supported .NET culture
                case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                    netLanguage = "ms"; // closest supported
                    break;
                case "in-ID":  // "Indonesian (Indonesia)" has different code in  .NET
                    netLanguage = "id-ID"; // correct code for .NET
                    break;
                case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
                    netLanguage = "de-CH"; // closest supported
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }
            return netLanguage;
        }

        /// <summary>
        /// Convert a <see cref="Aldara.Mobile.PlatformCulture"/> instance to a fallback .NET language string.
        /// </summary>
        /// <param name="platCulture">The <see cref="Aldara.Mobile.Models.PlatformCulture"/> instance.</param>
        /// <returns>the .NET fallback language if it exists; otherwise, the <see cref="Aldara.Mobile.Models.PlatformCulture.LanguageCode"/>.</returns>
        private static string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            string netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually);
            switch (platCulture.LanguageCode)
            {
                case "gsw":
                    netLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }
            return netLanguage;
        }
    }
}