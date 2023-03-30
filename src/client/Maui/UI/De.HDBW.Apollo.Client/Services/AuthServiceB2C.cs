// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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
