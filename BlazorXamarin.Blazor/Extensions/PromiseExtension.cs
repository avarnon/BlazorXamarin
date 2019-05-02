using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BlazorXamarin.Blazor.Contracts;
using BlazorXamarin.Blazor.Extensions;
using Microsoft.JSInterop;

namespace BlazorXamarin.Blazor
{
    public static class PromiseExtension
    {
        private static readonly ConcurrentDictionary<string, IPromiseCallbackHandler> __callbackHandlers =
            new ConcurrentDictionary<string, IPromiseCallbackHandler>();

        [JSInvokable]
        public static void PromiseCallback(string callbackId, string javaScriptResult)
        {
            HandlePromiseCallback(callbackId, (IPromiseCallbackHandler promiseCallbackHandler) =>
            {
                promiseCallbackHandler.SetResult(javaScriptResult);
            });
        }

        [JSInvokable]
        public static void PromiseError(string callbackId, string javaScriptError)
        {
            HandlePromiseCallback(callbackId, (IPromiseCallbackHandler promiseCallbackHandler) =>
            {
                promiseCallbackHandler.SetError(javaScriptError);
            });
        }

        /// <summary>
        /// Execute an asynchronous JavaScript function.
        /// </summary>
        /// <typeparam name="TResult">The type of the return object.</typeparam>
        /// <param name="jsruntime">The JavaScript runtime on which the call will be invoked.</param>
        /// <param name="javaScriptFunctionName">The JavaScript function to execute. Note: Must be off of the Window object and return a promise.</param>
        /// <param name="javaScriptFunctionArgs">The arguments to pass into the JavaScript Function.</param>
        /// <returns>the result of the Promise returned by the JavaScript Function.</returns>
        public static async Task<TResult> ExecutePromiseAsync<TResult>(this IJSRuntime jsruntime, string javaScriptFunctionName, params object[] javaScriptFunctionArgs)
        {
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            string callbackId = Guid.NewGuid().ToString();

            if (__callbackHandlers.TryAdd(callbackId, new PromiseCallbackHandler<TResult>(taskCompletionSource)))
            {
                await jsruntime.InvokeAsync<bool>("BlazorXamarin.Blazor.TypeScript.runAsyncFunction", new object[] { callbackId, javaScriptFunctionName, javaScriptFunctionArgs, });

                return await taskCompletionSource.Task;
            }

            throw new Exception("An entry with the same callback id already existed, really should never happen");
        }

        private static void HandlePromiseCallback(string callbackId, Action<IPromiseCallbackHandler> doWork)
        {
            if (__callbackHandlers.TryGetValue(callbackId, out IPromiseCallbackHandler promiseCallbackHandler))
            {
                try
                {
                    doWork(promiseCallbackHandler);
                }
                finally
                {
                    __callbackHandlers.TryRemove(callbackId, out IPromiseCallbackHandler removedPromiseCallbackHandler);
                }
            }
        }
    }
}
