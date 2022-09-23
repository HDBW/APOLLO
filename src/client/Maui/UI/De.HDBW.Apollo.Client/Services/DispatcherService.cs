namespace De.HDBW.Apollo.Client.Services
{
    using System.Threading.Tasks;
    using De.HDBW.Apollo.Client.Contracts;
    using Microsoft.Extensions.Logging;

    public class DispatcherService : IDispatcherService
    {
        public DispatcherService(ILogger<DispatcherService> logger)
        {
            this.Logger = logger;
        }

        private ILogger Logger { get; set; }

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
