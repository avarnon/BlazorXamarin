using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BlazorXamarin.Application.Contracts;
using BlazorXamarin.Application.Models;
using BlazorXamarin.UI.Common.Contracts;
using BlazorXamarin.WPF.Services;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace BlazorXamarin.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();

            Forms.Init();
            LoadApplication(new BlazorXamarin.UI.Common.App(new WpfInitializer()));
        }
    }

    public class WpfInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(GetConfiguration());
            containerRegistry.Register<ILocalize, Localize>();
            containerRegistry.RegisterSingleton<IWebProxyFactory, WebProxyFactory>();
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
