namespace De.HDBW.Apollo.Client.Services
{
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using De.HDBW.Apollo.Client.ViewModels;
    using Microsoft.Extensions.Logging;

    public class NavigationService : INavigationService
    {
        private readonly WeakEventManager weakEventManager = new WeakEventManager();

        public NavigationService(IDispatcherService dispatcherService, ILogger<NavigationService> logger)
        {
            this.Logger = logger;
            this.DispatcherService = dispatcherService;
        }

        private ILogger Logger { get; }

        private IDispatcherService DispatcherService { get; }

        public async Task<bool> NavigateAsnc(string route, CancellationToken token, NavigationParameters parameters = null)
        {
            token.ThrowIfCancellationRequested();
            var result = false;
            try
            {
                await this.DispatcherService.ExecuteOnMainThreadAsync(() => this.NavigateOnUIThreadAsnc(route, token, parameters), token);
                result = true;
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled NavigateAsync in {this.GetType()}.");
                throw;
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled NavigateAsync in {this.GetType()}.");
                throw;
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error while NavigateAsync in {this.GetType()}.");
            }

            return result;
        }

        private Task NavigateOnUIThreadAsnc(string route, CancellationToken token, NavigationParameters parameters)
        {
            if (parameters == null)
            {
                return Shell.Current?.GoToAsync(route, true);
            }
            else
            {
                return Shell.Current?.GoToAsync(route, true, parameters.ToQueryDictionary());
            }
        }
    }
}
