using System.Net;
using BlazorXamarin.Application.Contracts;
using Java.Lang;

namespace BlazorXamarin.Droid.Services
{
    public class WebProxyFactory : IWebProxyFactory
    {
        public WebProxy GetWebProxy()
        {
            string proxyHost = JavaSystem.GetProperty("http.proxyHost");
            string proxyPortString = JavaSystem.GetProperty("http.proxyPort");

            if (!string.IsNullOrWhiteSpace(proxyHost) &&
                !string.IsNullOrWhiteSpace(proxyPortString) &&
                int.TryParse(proxyPortString, out int proxyPort) &&
                proxyPort != 0)
            {
                return new WebProxy(proxyHost, proxyPort);
            }

            return null;
        }
    }
}