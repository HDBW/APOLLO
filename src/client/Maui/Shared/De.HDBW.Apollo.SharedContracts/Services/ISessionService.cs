// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface ISessionService
    {
        public UseCase? UseCase { get; }

        bool HasRegisteredUser { get; }

        void AddFavorite(long id, Type type);

        void RemoveFavorite(long id, Type type);

        void ClearFavorites();

        IEnumerable<(long Id, Type Type)> GetFavorites();

        void UpdateRegisteredUser(bool hasRegisteredUser);

        void UpdateUseCase(UseCase? useCase);
    }
}
