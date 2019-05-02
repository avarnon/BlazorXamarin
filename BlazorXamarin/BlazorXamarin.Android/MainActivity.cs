using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Android.App;
using Android.Content.PM;
using Android.OS;
using BlazorXamarin.Application.Models;
using BlazorXamarin.Droid.Services;
using BlazorXamarin.UI.Common.Contracts;
using Java.Lang;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;

namespace BlazorXamarin.Droid
{
    [Activity(Label = "BlazorXamarin", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new BlazorXamarin.UI.Common.App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(GetConfiguration());
            containerRegistry.Register<ILocalize, Localize>();

            HttpClient httpClient = new HttpClient();
            string proxyHost = JavaSystem.GetProperty("http.proxyHost");
            string proxyPortString = JavaSystem.GetProperty("http.proxyPort");

            if (!string.IsNullOrWhiteSpace(proxyHost) &&
                !string.IsNullOrWhiteSpace(proxyPortString) &&
                int.TryParse(proxyPortString, out int proxyPort) &&
                proxyPort != 0)
            {
                WebProxy webProxy = new WebProxy(proxyHost, proxyPort);
                HttpClientHandler httpClientHandler = new HttpClientHandler
                {
                    Proxy = webProxy,
                };

                httpClient = new HttpClient(httpClientHandler);
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

