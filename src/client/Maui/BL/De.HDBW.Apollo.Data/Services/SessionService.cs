// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Data.Services
{
    public class SessionService : ISessionService
    {
        public SessionService(bool hasRegisteredUser)
        {
            HasRegisteredUser = hasRegisteredUser;
        }

        public bool HasRegisteredUser { get; private set; }

        public UseCase? UseCase { get; private set; }

        public void UpdateRegisteredUser(bool hasRegisteredUser)
        {
            HasRegisteredUser = hasRegisteredUser;
        }

        public void UpdateUseCase(UseCase? useCase)
        {
            UseCase = useCase;
        }
    }
}
