using BlazorXamarin.Application.Contracts;
using BlazorXamarin.Application.Models;
using BlazorXamarin.Application.Services;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Windows.Networking.Connectivity;

namespace BlazorXamarin.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new BlazorXamarin.UI.Common.App(new UwpInitializer()));
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Configuration configuration = GetConfiguration();
            containerRegistry.RegisterInstance(configuration);

            HttpClient httpClient = new HttpClient();

            if (!string.IsNullOrWhiteSpace(configuration?.ApiServerUrl) &&
                Uri.TryCreate(configuration.ApiServerUrl, UriKind.Absolute, out Uri apiServerUri))
            {
                ProxyConfiguration proxyConfiguration = NetworkInformation.GetProxyConfigurationAsync(apiServerUri).GetResults();
                if (proxyConfiguration.ProxyUris.Count > 0)
                {
                    Uri proxyUri = proxyConfiguration.ProxyUris[0];
                    string proxyHost = proxyUri.Host;
                    int proxyPort = proxyUri.Port;

                    if (!string.IsNullOrWhiteSpace(proxyHost) &&
                        proxyPort != 0)
                    {
                        WebProxy webProxy = new WebProxy(proxyHost, proxyPort);
                        HttpClientHandler httpClientHandler = new HttpClientHandler
                        {
                            Proxy = webProxy,
                        };

                        httpClient = new HttpClient(httpClientHandler);
                    }
                }
            }

            containerRegistry.RegisterInstance(httpClient);
        }

        private Configuration GetConfiguration()
        {
            Configuration defaultConfiguration = GetConfiguration("appsettings.json");
#if DEBUG
            Configuration additionalConfiguration = GetConfiguration("appsettings.debug.json");
#else
            Configuration additionalConfiguration = GetConfiguration("appsettings.release.json");
#endif

            Configuration finalConfiguration = new Configuration()
            {
                ApiServerUrl = additionalConfiguration?.ApiServerUrl ?? defaultConfiguration?.ApiServerUrl,
            };

            return finalConfiguration;
        }

        private Configuration GetConfiguration(string fileName)
        {
            Assembly assembly = this.GetType().Assembly;
            string manifestResourceName = assembly.GetManifestResourceNames().SingleOrDefault(_ => _.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrWhiteSpace(manifestResourceName)) return new Configuration();

            using (Stream stream = assembly.GetManifestResourceStream(manifestResourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return JsonConvert.DeserializeObject<Configuration>(reader.ReadToEnd());
                }
            }
        }
    }
}
