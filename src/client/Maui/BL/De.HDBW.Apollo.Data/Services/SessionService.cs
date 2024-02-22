// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Data.Services
{
    public class SessionService : ISessionService
    {
        private readonly List<(long Id, Type Type)> _favorites = new List<(long Id, Type Type)>();

        public SessionService(string accessToken, AccountId? accountId)
        {
            AccountId = accountId;
            AccessToken = accessToken;
        }

        public bool HasRegisteredUser => AccountId != null;

        public AccountId? AccountId { get; private set; }

        public string AccessToken { get; private set; }

        public UseCase? UseCase { get; private set; }

        public bool ChangedUseCase { get; private set; }

        public void AddFavorite(long id, Type type)
        {
            RemoveFavorite(id, type);
            _favorites.Add(new (id, type));
        }

        public void ClearFavorites()
        {
            _favorites.Clear();
        }

        public void ConfirmedUseCaseChanged()
        {
            ChangedUseCase = false;
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

        public void UpdateRegisteredUser(AccountId? accountId)
        {
            AccountId = accountId;
        }

        public void UpdateUseCase(UseCase? useCase)
        {
            UseCase = useCase;
            ChangedUseCase = true;
        }
    }
}
