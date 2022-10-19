// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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
            MainThread.BeginInvokeOnMainThread(action);
        }

        public Task BeginInvokeOnMainThreadAsync(Action method, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return MainThread.InvokeOnMainThreadAsync(method);
        }

        public Task ExecuteOnMainThreadAsync(Func<Task> action, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return MainThread.InvokeOnMainThreadAsync(action);
        }

        public Task<TU> ExecuteOnMainThreadAsync<TU>(Func<Task<TU>> action, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return MainThread.InvokeOnMainThreadAsync(action);
        }
    }
}
