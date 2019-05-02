using BlazorXamarin.Application.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BlazorXamarin.Blazor.Services
{
    public class TranslationService : ITranslationService
    {
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> __translations;

        static TranslationService()
        {
            Assembly assembly = typeof(TranslationService).Assembly;
            IEnumerable<string> manifestResourceNames = assembly.GetManifestResourceNames();
            Regex regex = new Regex(@"^.*\.Strings(_(?<Locale>.{2,5}))?\.json$", RegexOptions.Compiled);

            __translations = manifestResourceNames.Select(_ => new
            {
                ManifestResourceName = _,
                Match = regex.Match(_),
            })
            .Where(_ => _.Match.Success)
            .Select(_ =>
            {
                string locale = _.Match.Groups["Locale"].Value;

                return new
                {
                    ManifestResourceName = _.ManifestResourceName,
                    Locale = string.IsNullOrWhiteSpace(locale)
                    ? string.Empty
                    : locale,
                };
            })
            .ToDictionary(_ => _.Locale, _ => ReadTranslations(assembly, _.ManifestResourceName));
        }

        public string this[string index]
        {
            get
            {
                IReadOnlyDictionary<string, string> currentTranslations = GetCurrentTranslations();

                if (currentTranslations.TryGetValue(index, out string translation) == false ||
                    translation == null)
                {
                    translation = index; // Return the translation key if there is no entry.
                }

                return translation;
            }
        }

        private static IReadOnlyDictionary<string, string> GetCurrentTranslations()
        {
            string locale = CultureInfo.DefaultThreadCurrentUICulture.Name;

            if (string.IsNullOrWhiteSpace(locale)) return new Dictionary<string, string>(); // Short circuit if we don't have a locale.

            if (__translations.TryGetValue(locale, out IReadOnlyDictionary<string, string> currentTranslations) == false)
            {
                // We don't have translations for the full locale.
                locale = locale.Split('-')[0];

                if (__translations.TryGetValue(locale, out currentTranslations) == false)
                {
                    // We don't have translations for the language root of the locale.
                    locale = string.Empty;

                    if (__translations.TryGetValue(locale, out currentTranslations) == false)
                    {
                        // We don't have default translations.
                        currentTranslations = new Dictionary<string, string>();
                    }
                }
            }

            return currentTranslations;
        }

        private static IReadOnlyDictionary<string, string> ReadTranslations(Assembly assembly, string manifestResourceName)
        {
            using (Stream manifestResourceStream = assembly.GetManifestResourceStream(manifestResourceName))
            {
                using (StreamReader streamReader = new StreamReader(manifestResourceStream))
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
                }
            }
        }
    }
}
