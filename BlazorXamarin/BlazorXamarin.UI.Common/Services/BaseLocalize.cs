using System.Globalization;
using BlazorXamarin.UI.Common.Contracts;
using Xamarin.Forms;

namespace BlazorXamarin.UI.Common.Services
{
    public abstract class BaseLocalize : ILocalize
    {
        protected static readonly string DefaultCulture = "en";

        private CultureInfo _currentCultureInfo;
        private bool _languageChanged;

        public virtual CultureInfo CultureInfo
        {
            get
            {
                if (this._languageChanged == false || this._currentCultureInfo == null)
                {
                    this._currentCultureInfo = this.OriginalCultureInfo;
                }

                return this._currentCultureInfo;
            }
            set
            {
                this._currentCultureInfo = value;

                if (this._languageChanged == false)
                {
                    this._languageChanged = value.Equals(this.OriginalCultureInfo) == false;
                }

                MessagingCenter.Send<ILocalize, CultureInfo>(this, nameof(CultureInfo), value);
            }
        }

        protected abstract CultureInfo OriginalCultureInfo { get; }
    }
}