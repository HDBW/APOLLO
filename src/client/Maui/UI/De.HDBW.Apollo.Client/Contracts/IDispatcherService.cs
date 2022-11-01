// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IDispatcherService
    {
        void BeginInvokeOnMainThread(Action action);

        Task BeginInvokeOnMainThreadAsync(Action method, CancellationToken token);

        Task ExecuteOnMainThreadAsync(Func<Task> action, CancellationToken token);

        Task<TU> ExecuteOnMainThreadAsync<TU>(Func<Task<TU>> action, CancellationToken token);
    }
}
