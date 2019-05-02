using System.Net;
using BlazorXamarin.Application.Contracts;
using CoreFoundation;

namespace BlazorXamarin.iOS.Services
{
    public class WebProxyFactory : IWebProxyFactory
    {
        public WebProxy GetWebProxy()
        {
            CFProxySettings systemProxySettings = CFNetwork.GetSystemProxySettings();
            string proxyHost = systemProxySettings.HTTPProxy;
            int proxyPort = systemProxySettings.HTTPPort;

            if (!string.IsNullOrWhiteSpace(proxyHost) &&
                proxyPort != 0)
            {
                return new WebProxy(proxyHost, proxyPort);
            }

            return null;
        }
    }
}