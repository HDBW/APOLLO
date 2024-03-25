// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class UserRepository : AbstractFileRepository<User>, IUserRepository
    {
        public UserRepository(
            string basePath,
            ILogger<UserRepository>? logger)
            : base(basePath, logger)
        {
        }

        public Task<bool> DeleteUserAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                if (File.Exists(BasePath))
                {
                    File.Delete(BasePath);
                }

                return Task.FromResult(true);
            }
            catch (OperationCanceledException)
            {
                Logger.LogDebug($"Canceled {nameof(DeleteUserAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger.LogDebug($"Canceled {nameof(DeleteUserAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unknown error in {nameof(DeleteUserAsync)} in {GetType().Name}.");
            }

            return Task.FromResult(false);
        }

        public Task<User?> GetItemAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return LoadAsync(token);
        }
    }
}
