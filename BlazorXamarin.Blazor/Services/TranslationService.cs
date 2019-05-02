using BlazorXamarin.Application.Contracts;
using Newtonsoft.Json;
using System;
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

            Console.WriteLine(string.Join("; ", manifestResourceNames.ToArray()));

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

        public string this[string index] {
            get
            {
                IReadOnlyDictionary<string, string> currentTranslations = GetCurrentTranslations();

                if (currentTranslations.TryGetValue(index, out string translation) == false ||
                    translation == null)
                {
                    translation = index;
                }

                Console.WriteLine($"TranslationService: {index}: {translation}");

                return translation;
            }
        }

        private static IReadOnlyDictionary<string, string> GetCurrentTranslations()
        {
            string locale = CultureInfo.DefaultThreadCurrentUICulture.Name;

            if (string.IsNullOrWhiteSpace(locale)) return new Dictionary<string, string>();

            if (__translations.TryGetValue(locale, out IReadOnlyDictionary<string, string> currentTranslations) == false)
            {
                locale = locale.Split('-')[0];

                if (__translations.TryGetValue(locale, out currentTranslations) == false)
                {
                    locale = string.Empty;

                    if (__translations.TryGetValue(locale, out currentTranslations) == false)
                    {
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
