using BlazorXamarin.Application.Contracts;
using BlazorXamarin.Application.Models;
using Prism;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BlazorXamarin.UI.Common.ViewModels
{
    public class FetchDataViewModel : ViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;

        private ObservableCollection<WeatherForecast> _forecasts;
        private bool _isActive;
        private bool _isLoading;
        private IWeatherForecastService _weatherForecastService;

        public FetchDataViewModel(
            INavigationService navigationService,
            IWeatherForecastService weatherForecastService)
            : base(navigationService)
        {
            if (weatherForecastService == null) throw new ArgumentNullException(nameof(weatherForecastService));

            this._forecasts = new ObservableCollection<WeatherForecast>();
            this._weatherForecastService = weatherForecastService;

            Title = "Weather forecast";
        }

        public ObservableCollection<WeatherForecast> Forecasts
        {
            get { return _forecasts; }
            set { SetProperty(ref _forecasts, value); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        protected virtual void RaiseIsActiveChanged()
        {
            this.IsActiveChanged?.Invoke(this, EventArgs.Empty);

            if (this.IsActive)
            {
                Task.Run(LoadForecastsAsync);
            }
        }

        private async Task LoadForecastsAsync()
        {
            this.IsLoading = true;

            try
            {
                IEnumerable< WeatherForecast> result = await this._weatherForecastService.GetWeatherForecastsAsync();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    this.Forecasts.Clear();
                    foreach (WeatherForecast forecast in result)
                    {
                        this.Forecasts.Add(forecast);
                    }
                });
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                this.IsLoading = false;
            }
        }
    }
}
