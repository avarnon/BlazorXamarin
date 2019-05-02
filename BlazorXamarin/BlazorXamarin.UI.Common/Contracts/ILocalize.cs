using System.Globalization;

namespace BlazorXamarin.UI.Common.Contracts
{
    public interface ILocalize
    {
        CultureInfo CultureInfo { get; set; }
    }
}
