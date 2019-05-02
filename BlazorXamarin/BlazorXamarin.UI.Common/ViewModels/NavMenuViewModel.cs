using Prism.Navigation;

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
