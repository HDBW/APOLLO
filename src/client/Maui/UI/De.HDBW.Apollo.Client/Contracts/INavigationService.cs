using De.HDBW.Apollo.Client.Models;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface INavigationService
    {
        Task<bool> PushToRootAsnc(string route, CancellationToken token, NavigationParameters? parameters = null);

        Task<bool> NavigateAsnc(string route, CancellationToken token, NavigationParameters? parameters = null);
    }
}
