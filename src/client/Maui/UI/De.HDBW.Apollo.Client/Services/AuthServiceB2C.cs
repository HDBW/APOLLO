using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.Services
{
    public class AuthServiceB2C : BaseAuthService
    {
        public AuthServiceB2C(IPublicClientApplication publicClientApplication)
            : base(publicClientApplication)
        {
        }
    }
}
