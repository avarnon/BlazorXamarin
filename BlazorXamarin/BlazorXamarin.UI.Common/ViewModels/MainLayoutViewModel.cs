using System;
using Prism.Commands;
using Prism.Navigation;

namespace BlazorXamarin.UI.Common.ViewModels
{
    public class MainLayoutViewModel : ViewModelBase
    {
        public MainLayoutViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.Title = "BlazorXamarin";
            this.AboutCommand = new DelegateCommand(() => this.AboutClick());
        }

        public DelegateCommand AboutCommand { get; }

        private void AboutClick()
        {
            Xamarin.Forms.Device.OpenUri(new Uri("http://blazor.net"));
        }
    }
}
