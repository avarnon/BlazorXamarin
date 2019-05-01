using BlazorXamarin.Application.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorXamarin.Application.Services
{
    public abstract class ApiService
    {
        private readonly string _apiServerUrl;
        private readonly HttpClient _httpClient;

        protected ApiService(Configuration configuration, HttpClient httpClient)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(configuration.ApiServerUrl)) throw new ArgumentNullException($"{nameof(configuration)}.{nameof(Configuration.ApiServerUrl)}");
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

            this._apiServerUrl = configuration.ApiServerUrl.TrimEnd('/');
            this._httpClient = httpClient;
        }

        public async Task<TResult> GetAsync<TResult>(string apiRelativeUrl)
        {
            if (string.IsNullOrWhiteSpace(apiRelativeUrl)) throw new ArgumentNullException(nameof(apiRelativeUrl));

            string absoluteUrl = $"{this._apiServerUrl}/{apiRelativeUrl.TrimStart('/')}";
            string jsonString = await this._httpClient.GetStringAsync(absoluteUrl);

            return JsonConvert.DeserializeObject<TResult>(jsonString);
        }
    }
}
