using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorXamarin.Application.Models;

namespace BlazorXamarin.Application.Contracts
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync();
    }
}
