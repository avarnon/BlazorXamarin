using Prism.Navigation;

namespace BlazorXamarin.UI.Common.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Hello, world!";
        }
    }
}
