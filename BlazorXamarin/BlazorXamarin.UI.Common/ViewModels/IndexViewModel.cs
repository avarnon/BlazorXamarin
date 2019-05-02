using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
