using System;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace BlazorXamarin.UI.Common.ViewModels
{
    public class CounterViewModel : ViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;

        private bool _isActive;
        private int _currentCount;

        public CounterViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.Title = "Counter";
            this.IncrementCounterCommand = new DelegateCommand(() => this.IncrementCounter());
        }

        public int CurrentCount
        {
            get { return _currentCount; }
            set { SetProperty(ref _currentCount, value); }
        }

        public DelegateCommand IncrementCounterCommand { get; }

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        protected virtual void RaiseIsActiveChanged()
        {
            this.IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        private void IncrementCounter()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                int currentCount = this.CurrentCount + 1;
                this.CurrentCount = currentCount;
            });
        }
    }
}
