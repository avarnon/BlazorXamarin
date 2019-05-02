using System.Globalization;
using System.Threading;
using BlazorXamarin.UI.Common.Contracts;
using BlazorXamarin.UI.Common.Services;

namespace BlazorXamarin.UWP.Services
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

                base.CultureInfo = value;
            }
        }

        private static CultureInfo GetOsCultureInfo()
        {
            string netLanguage = DefaultCulture;
            if (Windows.System.UserProfile.GlobalizationPreferences.Languages.Count > 0)
            {
                netLanguage = Windows.System.UserProfile.GlobalizationPreferences.Languages[0];
            }

            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException)
            {
                cultureInfo = new CultureInfo(DefaultCulture);
            }

            return cultureInfo;
        }
    }
}