using Prism.Commands;
using Prism.Navigation;
using System;

namespace BlazorXamarin.UI.Common.ViewModels
{
    public class NavMenuViewModel : ViewModelBase
    {
        public NavMenuViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.Title = "BlazorXamarin";
        }
    }
}
