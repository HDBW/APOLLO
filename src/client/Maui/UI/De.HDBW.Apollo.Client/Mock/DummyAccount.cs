// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.Models
{
#if DEBUG
    public class DummyAccount : IAccount
    {
        public string Username { get; } = "Dummy";

        public string Environment { get; } = "Dummy";

        public AccountId HomeAccountId { get; } = new AccountId("Dummy", "Dummy", "Dummy");
    }
#endif
}
