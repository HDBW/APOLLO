// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Graph.Apollo.Cloud.Common.Models.Assessment;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public abstract class AbstractDataBaseRepositoryTest<TU> : AbstractRepositoryTest<TU>
        where TU : IEntity, new()
    {
        private IDatabaseRepository<TU> GetDataBaseRepository()
        {
            return GetRepository() as IDatabaseRepository<TU>;
        }

        [Fact]
        public async Task TestDatabaseRepositoryWithCanceledAndDisposedTokenAsync()
        {
            var repository = GetDataBaseRepository();
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddOrUpdateItemsAsync(null, cts.Token).ConfigureAwait(false); });
                await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddOrUpdateItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.UpdateItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.UpdateItemsAsync(null, cts.Token).ConfigureAwait(false); });
                cts.Dispose();
                await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddOrUpdateItemsAsync(null, cts.Token).ConfigureAwait(false); });
                await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddOrUpdateItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.UpdateItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.UpdateItemsAsync(null, cts.Token).ConfigureAwait(false); });
            }
        }

        [Fact]
        public async Task TestDatabaseRepositoryAsync()
        {
            var repository = GetDataBaseRepository();

            // TODO: Add tests for
            // AddOrUpdateItemsAsync(IEnumerable<TU> items, CancellationToken token);
            // AddOrUpdateItemAsync(TU item, CancellationToken token);
            // UpdateItemAsync(TU item, CancellationToken token);
            // UpdateItemsAsync(IEnumerable<TU> items, CancellationToken token);
        }
    }
}
