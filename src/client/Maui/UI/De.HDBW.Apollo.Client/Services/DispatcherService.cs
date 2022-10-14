// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Threading.Tasks;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Services
{
    public class DispatcherService : IDispatcherService
    {
        public DispatcherService(ILogger<DispatcherService> logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; set; }

        public void BeginInvokeOnMainThread(Action action)
        {
            if (MainThread.IsMainThread)
            {
                action.Invoke();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(action);
            }
        }

        public Task ExecuteOnMainThreadAsync(Func<Task> action, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (MainThread.IsMainThread)
            {
                return action.Invoke();
            }
            else
            {
                return MainThread.InvokeOnMainThreadAsync(() => action);
            }
        }

        public Task<TU> ExecuteOnMainThreadAsync<TU>(Func<Task<TU>> action, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (MainThread.IsMainThread)
            {
                return action.Invoke();
            }
            else
            {
                return MainThread.InvokeOnMainThreadAsync(() => { return action.Invoke(); });
            }
        }
    }
}
