using System.Net;

namespace BlazorXamarin.Application.Contracts
{
    public interface IWebProxyFactory
    {
        WebProxy GetWebProxy();
    }
}
