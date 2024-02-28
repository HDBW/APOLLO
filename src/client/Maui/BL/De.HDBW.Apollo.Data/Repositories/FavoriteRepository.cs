// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class FavoriteRepository : AbstractFileRepository<IEnumerable<Favorite>>, IFavoriteRepository
    {
        public FavoriteRepository(
            string basePath,
            ILogger<FavoriteRepository>? logger)
            : base(basePath, logger)
        {
        }

        public async Task<bool> DeleteFavoriteAsync(string id, string type, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(type))
                {
                    return false;
                }

                var list = (await LoadAsync(token) ?? new List<Favorite>()).ToList();
                var item = list.FirstOrDefault(x => string.Equals(id, x.Id) && string.Equals(x.Type, type));
                if (item != null)
                {
                    list.Remove(item);
                }

                return await SaveAsync(list, token);
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(DeleteFavoriteAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(DeleteFavoriteAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(DeleteFavoriteAsync)} in {GetType().Name}.");
            }

            return false;
        }

        public async Task<IEnumerable<Favorite>?> GetItemsByTypeAsync(string type, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                if (string.IsNullOrWhiteSpace(type))
                {
                    return null;
                }

                var items = await LoadAsync(token).ConfigureAwait(false);
                return items?.Where(x => x.Type == type).ToList();
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(GetItemsByTypeAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(GetItemsByTypeAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(GetItemsByTypeAsync)} in {GetType().Name}.");
            }

            return null;
        }

        public async Task<Favorite?> GetItemByIdAsync(string id, string type, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(type))
                {
                    return null;
                }

                var list = await LoadAsync(token).ConfigureAwait(false);
                return list?.FirstOrDefault(x => string.Equals(id, x.Id) && string.Equals(x.Type, type));
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(GetItemByIdAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(GetItemByIdAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(GetItemByIdAsync)} in {GetType().Name}.");
            }

            return null;
        }

        public async Task<bool> SaveAsync(Favorite favorite, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                if (string.IsNullOrWhiteSpace(favorite?.Id) || string.IsNullOrWhiteSpace(favorite?.Type))
                {
                    return false;
                }

                var list = (await LoadAsync(token).ConfigureAwait(false) ?? new List<Favorite>()).ToList();
                var item = list.FirstOrDefault(x => string.Equals(x.Id, favorite.Id) && string.Equals(x.Type, favorite.Type));
                if (item != null)
                {
                    list.Remove(item);
                }

                list.Add(favorite);
                return await SaveAsync(list, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(SaveAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(SaveAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(SaveAsync)} in {GetType().Name}.");
            }

            return false;
        }
    }
}
