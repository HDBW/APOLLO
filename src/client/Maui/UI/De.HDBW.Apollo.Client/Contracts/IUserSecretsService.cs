// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IUserSecretsService
    {
        string? this[string name]
        {
            get;
        }

        bool LoadSecrets();
    }
}
