using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BlazorXamarin.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AppConfigure)
                .UseStartup<Startup>()
                .Build();

        private static void AppConfigure(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddEnvironmentVariables();
        }
    }
}
