namespace BlazorXamarin.Blazor.Contracts
{
    public interface IPromiseCallbackHandler
    {
        void SetResult(string json);

        void SetError(string error);
    }
}