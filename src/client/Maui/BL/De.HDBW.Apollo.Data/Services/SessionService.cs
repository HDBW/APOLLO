// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Data.Services
{
    public class SessionService : ISessionService
    {
        private readonly List<(long Id, Type Type)> _favorites = new List<(long Id, Type Type)>();

        public SessionService(string? uniqueId, AccountId? accountId)
        {
            AccountId = accountId;
            UniqueId = uniqueId;
        }

        public bool HasRegisteredUser => AccountId != null;

        public AccountId? AccountId { get; private set; }

        public string? UniqueId { get; private set; }

        public void AddFavorite(long id, Type type)
        {
            RemoveFavorite(id, type);
            _favorites.Add(new(id, type));
        }

        public void ClearFavorites()
        {
            _favorites.Clear();
        }

        public IEnumerable<(long Id, Type Type)> GetFavorites()
        {
            return _favorites;
        }

        public void RemoveFavorite(long id, Type type)
        {
            var items = _favorites.Where(f => f.Id == id && f.Type == type).ToList();
            foreach (var item in items)
            {
                _favorites.Remove(item);
            }
        }

        public void UpdateRegisteredUser(string? uniqueId, AccountId? accountId)
        {
            AccountId = accountId;
            UniqueId = uniqueId;
        }
    }
}
