using System;
using System.Diagnostics;
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
            Uri aboutUri = new Uri("http://blazor.net");

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.WPF)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = $"/c start {aboutUri.AbsoluteUri}",
                });
            }
            else
            {
                Xamarin.Forms.Device.OpenUri(aboutUri);
            }
        }
    }
}
