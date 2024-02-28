// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class FavoriteRepository : AbstractFileRepository<IEnumerable<Favorite?>>, IFavoriteRepository
    {
        public FavoriteRepository(
            string basePath,
            ILogger<FavoriteRepository>? logger)
            : base(basePath, logger)
        {
        }

        public async Task<bool> DeleteFavoriteAsync(string apiId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                if (string.IsNullOrWhiteSpace(apiId))
                {
                    return false;
                }

                var list = (await LoadAsync(token) ?? new List<Favorite?>()).ToList();
                var item = list.FirstOrDefault(x => x?.ApiId?.Equals(apiId) == true);
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

        public Task<IEnumerable<Favorite?>?> GetItemsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return LoadAsync(token);
        }

        public async Task<Favorite?> GetItemByApiIdAsync(string apiId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(apiId))
            {
                return default;
            }

            var list = (await LoadAsync(token) ?? new List<Favorite?>()).ToList();
            return list.FirstOrDefault(x => x?.ApiId?.Equals(apiId) == true);
        }

        public async Task<bool> SaveAsync(Favorite favorite, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(favorite?.ApiId))
            {
                return false;
            }

            var list = (await LoadAsync(token) ?? new List<Favorite?>()).ToList();
            var item = list.FirstOrDefault(x => x?.ApiId?.Equals(favorite.ApiId) == true);
            if (item != null)
            {
                list.Remove(item);
            }

            list.Add(favorite);
            return await SaveAsync(list, token);
        }
    }
}
