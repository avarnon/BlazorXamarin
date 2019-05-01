using BlazorXamarin.Application.Contracts;
using BlazorXamarin.Application.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorXamarin.Application.Services
{
    public class WeatherForecastService : ApiService, IWeatherForecastService
    {
        public WeatherForecastService(Configuration configuration, HttpClient httpClient)
            : base(configuration, httpClient)
        {
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync()
        {
            return await GetAsync<IEnumerable<WeatherForecast>>("api/SampleData/WeatherForecasts");
        }
    }
}
