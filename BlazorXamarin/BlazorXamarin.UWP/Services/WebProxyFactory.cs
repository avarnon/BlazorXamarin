using System;
using System.Net;
using System.Threading.Tasks;
using BlazorXamarin.Application.Contracts;
using BlazorXamarin.Application.Models;
using Windows.Networking.Connectivity;

namespace BlazorXamarin.UWP.Services
{
    public class WebProxyFactory : IWebProxyFactory
    {
        private readonly Configuration _configuration;

        public WebProxyFactory(Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            this._configuration = configuration;
        }

        public WebProxy GetWebProxy()
        {
            if (!string.IsNullOrWhiteSpace(this._configuration.ApiServerUrl) &&
                Uri.TryCreate(this._configuration.ApiServerUrl, UriKind.Absolute, out Uri apiServerUri))
            {
                ProxyConfiguration proxyConfiguration = Task.Run(async () => await NetworkInformation.GetProxyConfigurationAsync(apiServerUri)).Result;
                if (proxyConfiguration.ProxyUris.Count > 0)
                {
                    Uri proxyUri = proxyConfiguration.ProxyUris[0];
                    string proxyHost = proxyUri.Host;
                    int proxyPort = proxyUri.Port;

                    if (!string.IsNullOrWhiteSpace(proxyHost) &&
                        proxyPort != 0)
                    {
                        return new WebProxy(proxyHost, proxyPort);
                    }
                }
            }

            return null;
        }
    }
}
