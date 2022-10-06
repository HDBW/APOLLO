using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Data.Services
{
    public class SessionService : ISessionService
    {
        public bool HasRegisteredUser { get; private set; }
    }
}
