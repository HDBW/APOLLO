// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface ISessionService
    {
        public UseCase? UseCase { get; }

        bool HasRegisteredUser { get; }

        AccountId? RegisteredUserHomeAccountId { get; }

        bool ChangedUseCase { get; }

        void AddFavorite(long id, Type type);

        void RemoveFavorite(long id, Type type);

        void ClearFavorites();

        void ConfirmedUseCaseChanged();

        IEnumerable<(long Id, Type Type)> GetFavorites();

        void UpdateRegisteredUser(AccountId? registeredUserHomeAccountId);

        void UpdateUseCase(UseCase? useCase);
    }
}
