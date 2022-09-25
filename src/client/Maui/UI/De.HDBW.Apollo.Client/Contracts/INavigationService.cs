namespace De.HDBW.Apollo.Client.Contracts
{
    using De.HDBW.Apollo.Client.Models;

    public interface INavigationService
    {
        Task<bool> PushToRootAsnc(string route, CancellationToken token, NavigationParameters parameters = null);

        Task<bool> NavigateAsnc(string route, CancellationToken token, NavigationParameters parameters = null);
    }
}
