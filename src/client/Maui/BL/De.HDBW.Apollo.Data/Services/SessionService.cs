// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Data.Services
{
    public class SessionService : ISessionService
    {
        public bool HasRegisteredUser { get; private set; }
    }
}
