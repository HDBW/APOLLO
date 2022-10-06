using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.Services
{
    public abstract class BaseAuthService : IAuthService
    {
        private readonly IPublicClientApplication _authenticationClient;

        protected BaseAuthService(IPublicClientApplication authenticationClient)
        {
            _authenticationClient = authenticationClient;
        }

        public Task<AuthenticationResult?> SignInInteractively(CancellationToken cancellationToken)
        {
            return _authenticationClient
                    .AcquireTokenInteractive(Constants.Scopes)
#if WINDOWS
                    .WithUseEmbeddedWebView(false)
#endif
                    .ExecuteAsync(cancellationToken);
        }

        public async Task<AuthenticationResult?> AcquireTokenSilent(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _authenticationClient.GetAccountsAsync(Constants.SignInPolicy);
                var firstAccount = accounts.FirstOrDefault();
                if (firstAccount is null)
                {
                    return null;
                }

                return await _authenticationClient.AcquireTokenSilent(Constants.Scopes, firstAccount).ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException)
            {
                return null;
            }
        }

        public async Task LogoutAsync(CancellationToken cancellationToken)
        {
            var accounts = await _authenticationClient.GetAccountsAsync();
            foreach (var account in accounts)
            {
                await _authenticationClient.RemoveAsync(account);
            }
        }
    }
}
