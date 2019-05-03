using System.Globalization;
using System.Threading;
using BlazorXamarin.UI.Common.Contracts;
using BlazorXamarin.UI.Common.Services;

namespace BlazorXamarin.WPF.Services
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
            CultureInfo cultureInfo = CultureInfo.InstalledUICulture;

            if (cultureInfo == null)
            {
                cultureInfo = new CultureInfo(DefaultCulture);
            }

            return cultureInfo;
        }
    }
}