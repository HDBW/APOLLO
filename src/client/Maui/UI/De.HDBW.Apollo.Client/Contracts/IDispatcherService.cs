namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IDispatcherService
    {
        Task ExecuteOnMainThreadAsync(Func<Task> action, CancellationToken token);

        Task<TU> ExecuteOnMainThreadAsync<TU>(Func<Task<TU>> action, CancellationToken token);
    }
}
