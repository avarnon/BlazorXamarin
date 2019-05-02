using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using BlazorXamarin.Application.Models;
using BlazorXamarin.iOS.Services;
using BlazorXamarin.UI.Common.Contracts;
using CoreFoundation;
using Foundation;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;
using UIKit;

namespace BlazorXamarin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new BlazorXamarin.UI.Common.App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(GetConfiguration());
            containerRegistry.Register<ILocalize, Localize>();

            HttpClient httpClient = new HttpClient();
            CFProxySettings systemProxySettings = CFNetwork.GetSystemProxySettings();
            string proxyHost = systemProxySettings.HTTPProxy;
            int proxyPort = systemProxySettings.HTTPPort;

            if (!string.IsNullOrWhiteSpace(proxyHost) &&
                proxyPort != 0)
            {
                WebProxy webProxy = new WebProxy(proxyHost, proxyPort);
                HttpClientHandler httpClientHandler = new HttpClientHandler
                {
                    Proxy = webProxy,
                };

                httpClient = new HttpClient(httpClientHandler);
                containerRegistry.RegisterInstance(httpClient);
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
