using System.Globalization;
using System.Threading;
using BlazorXamarin.UI.Common.Contracts;
using BlazorXamarin.UI.Common.Models;
using BlazorXamarin.UI.Common.Services;
using Foundation;

namespace BlazorXamarin.iOS.Services
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

                NSUserDefaults.StandardUserDefaults.SetValueForKey(NSArray.FromStrings(value.TwoLetterISOLanguageName), new NSString("AppleLanguages"));
                NSUserDefaults.StandardUserDefaults.Synchronize();

                base.CultureInfo = value;
            }
        }

        private static CultureInfo GetOsCultureInfo()
        {
            string netLanguage = DefaultCulture;
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                string pref = NSLocale.PreferredLanguages[0];
                netLanguage = IOsToDotnetLanguage(pref);
            }

            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException)
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                try
                {
                    string fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    cultureInfo = new CultureInfo(fallback);
                }
                catch (CultureNotFoundException)
                {
                    // iOS language not valid .NET culture, falling back to English
                    cultureInfo = new CultureInfo(DefaultCulture);
                }
            }

            return cultureInfo;
        }

        /// <summary>
        /// Convert an iOS language string to its .NET equivalent.
        /// </summary>
        /// <param name="iOSLanguage">The iOS language string.</param>
        /// <returns>the .NET language string.</returns>
        private static string IOsToDotnetLanguage(string iOSLanguage)
        {
            string netLanguage = iOSLanguage;
            //certain languages need to be converted to CultureInfo equivalent
            switch (iOSLanguage)
            {
                case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                    netLanguage = "ms"; // closest supported
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
        /// Convert a <see cref="Aldara.Mobile.Models.PlatformCulture"/> instance to a fallback .NET language string.
        /// </summary>
        /// <param name="platCulture">The <see cref="Aldara.Mobile.Models.PlatformCulture"/> instance.</param>
        /// <returns>the .NET fallback language if it exists; otherwise, the <see cref="Aldara.Mobile.Models.PlatformCulture.LanguageCode"/>.</returns>
        private static string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            string netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually);
            switch (platCulture.LanguageCode)
            {
                case "pt":
                    netLanguage = "pt-PT"; // fallback to Portuguese (Portugal)
                    break;
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