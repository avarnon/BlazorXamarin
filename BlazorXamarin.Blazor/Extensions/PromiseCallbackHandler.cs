using BlazorXamarin.Blazor.Contracts;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BlazorXamarin.Blazor.Extensions
{
    public class PromiseCallbackHandler<TResult> : IPromiseCallbackHandler
    {
        private readonly TaskCompletionSource<TResult> _taskCompletionSource;

        public PromiseCallbackHandler(TaskCompletionSource<TResult> taskCompletionSource)
        {
            if (taskCompletionSource == null) throw new ArgumentNullException(nameof(taskCompletionSource));

            this._taskCompletionSource = taskCompletionSource;
        }

        public void SetResult(string json)
        {
            TResult result = string.IsNullOrWhiteSpace(json) ||
                string.Equals("null", json, StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals("undefined", json, StringComparison.InvariantCultureIgnoreCase)
                ? default(TResult)
                : JsonConvert.DeserializeObject<TResult>(json);

            _taskCompletionSource.SetResult(result);
        }

        public void SetError(string error)
        {
            var exception = string.IsNullOrWhiteSpace(error)
                ? new Exception("Unknown Error")
                : new Exception(error);

            _taskCompletionSource.SetException(exception);
        }
    }
}
