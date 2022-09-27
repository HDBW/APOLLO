namespace De.HDBW.Apollo.Data.Services
{
    using De.HDBW.Apollo.SharedContracts.Services;

    public class SessionService : ISessionService
    {
        public bool HasRegisteredUser { get; private set; }
    }
}
