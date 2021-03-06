﻿using System.Globalization;
using System.Net.Http;
using BlazorXamarin.Application.Contracts;
using BlazorXamarin.Application.Services;
using BlazorXamarin.UI.Common.Contracts;
using BlazorXamarin.UI.Common.ViewModels;
using BlazorXamarin.UI.Common.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using Unity.Injection;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BlazorXamarin.UI.Common
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App()
            : this(null)
        {
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            ILocalize localize = this.Container.Resolve<ILocalize>();
            CultureInfo cultureInfo = localize.CultureInfo;
            localize.CultureInfo = cultureInfo;

            await NavigationService.NavigateAsync("MainLayout/NavMenu");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Counter, CounterViewModel>();
            containerRegistry.RegisterForNavigation<FetchData, FetchDataViewModel>();
            containerRegistry.RegisterForNavigation<Index, IndexViewModel>();
            containerRegistry.RegisterForNavigation<MainLayout, MainLayoutViewModel>();
            containerRegistry.RegisterForNavigation<NavMenu, NavMenuViewModel>();

            containerRegistry.Register<ITranslationService, TranslationService>();
            containerRegistry.Register<IWeatherForecastService, WeatherForecastService>();

            containerRegistry.RegisterSingleton<HttpMessageHandler, BXHttpClientHandler>();
            containerRegistry.GetContainer().RegisterSingleton<HttpClient>(new InjectionConstructor(typeof(HttpMessageHandler)));
        }
    }
}
