namespace BlazorXamarin.Application.Contracts
{
    public interface ITranslationService
    {
        string this[string index] { get; }
    }
}
