using System.Globalization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.JSInterop;

namespace BlazorXamarin.Blazor
{
    public static class LocalizationExtensions
    {
        public static void UseBrowserLocalization(this IComponentsApplicationBuilder app)
        {
            string locale = null;
            IJSInProcessRuntime jsinProcessRuntime = app.Services.GetService(typeof(IJSRuntime)) as IJSInProcessRuntime;

            if (jsinProcessRuntime != null)
            {
                locale = jsinProcessRuntime.Invoke<string>("BlazorXamarin.Blazor.TypeScript.Localization.getLocale");
            }

            if (string.IsNullOrWhiteSpace(locale))
            {
                locale = "en";
            }

            CultureInfo culture = new CultureInfo(locale);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}
