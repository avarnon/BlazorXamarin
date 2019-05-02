using System;

namespace BlazorXamarin.UI.Common.Models
{
    public class PlatformCulture
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="platformCultureString">The platform culture string.</param>
        public PlatformCulture(string platformCultureString)
        {
            if (string.IsNullOrWhiteSpace(platformCultureString))
            {
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));
            }

            this.PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if (dashIndex > 0)
            {
                var parts = this.PlatformString.Split('-');
                this.LanguageCode = parts[0];
                this.LocaleCode = parts[1];
            }
            else
            {
                this.LanguageCode = this.PlatformString;
                this.LocaleCode = string.Empty;
            }
        }

        /// <summary>
        /// The language code.
        /// </summary>
        public string LanguageCode { get; }

        /// <summary>
        /// The locale code.
        /// </summary>
        public string LocaleCode { get; }

        /// <summary>
        /// The platform culture string.
        /// </summary>
        public string PlatformString { get; }

        /// <summary>
        /// Returns the string representation of this <see cref="PlatformCulture"/> instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.PlatformString;
        }
    }
}