using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BlazorXamarin.Application.Contracts;

namespace BlazorXamarin.Application.Services
{
    public class BXHttpClientHandler : HttpClientHandler
    {
        public BXHttpClientHandler(IWebProxyFactory webProxyFactory)
            : this()
        {
            WebProxy webProxy = webProxyFactory.GetWebProxy();
            this.Proxy = webProxy;
            this.UseProxy = webProxy != null;
        }

        protected BXHttpClientHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentUICulture.Name));
            if (string.Equals("en", CultureInfo.CurrentUICulture.Name, StringComparison.InvariantCultureIgnoreCase) == false)
            {
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
