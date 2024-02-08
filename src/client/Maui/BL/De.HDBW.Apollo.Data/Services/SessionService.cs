// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Data.Services
{
    public class SessionService : ISessionService
    {
        private readonly List<(string Id, Type Type)> _favorites = new List<(string Id, Type Type)>();

        public SessionService(bool hasRegisteredUser)
        {
            HasRegisteredUser = hasRegisteredUser;
        }

        public bool HasRegisteredUser { get; private set; }

        public UseCase? UseCase { get; private set; }

        public bool ChangedUseCase { get; private set; }

        public void AddFavorite(string id, Type type)
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

        public IEnumerable<(string Id, Type Type)> GetFavorites()
        {
            return _favorites;
        }

        public void RemoveFavorite(string id, Type type)
        {
            var items = _favorites.Where(f => f.Id == id && f.Type == type).ToList();
            foreach (var item in items)
            {
                _favorites.Remove(item);
            }
        }

        public void UpdateRegisteredUser(bool hasRegisteredUser)
        {
            HasRegisteredUser = hasRegisteredUser;
        }

        public void UpdateUseCase(UseCase? useCase)
        {
            UseCase = useCase;
            ChangedUseCase = true;
        }
    }
}
