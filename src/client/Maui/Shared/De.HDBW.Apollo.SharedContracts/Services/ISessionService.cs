// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface ISessionService
    {
        bool HasRegisteredUser { get; }

        AccountId? AccountId { get; }

        string? UniqueId { get; }

        void AddFavorite(long id, Type type);

        void RemoveFavorite(long id, Type type);

        void ClearFavorites();

        IEnumerable<(long Id, Type Type)> GetFavorites();

        void UpdateRegisteredUser(string? uniqueId, AccountId? accountId);
    }
}
