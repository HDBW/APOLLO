// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public abstract class AbstractRepositoryTest<TU>
        where TU : IEntity, new()
    {

        [Fact]
        public async Task TestRepositoryWithCanceledAndDisposedTokenAsync()
        {
            using (var context = new DatabaseTestContext(Path.GetTempFileName()))
            {
                var repository = GetRepository(context);
                using (var cts = new CancellationTokenSource())
                {
                    cts.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.GetItemsAsync(cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.GetItemByIdAsync(0, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.GetItemsByIdsAsync(null, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddItemsAsync(null, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.RemoveItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.RemoveItemsAsync(null, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.RemoveItemByIdAsync(0, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.ResetItemsAsync(null, cts.Token).ConfigureAwait(false); });
                    cts.Dispose();
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.GetItemsAsync(cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.GetItemByIdAsync(0, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.GetItemsByIdsAsync(null, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddItemsAsync(null, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.RemoveItemAsync(default(TU), cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.RemoveItemsAsync(null, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.RemoveItemByIdAsync(0, cts.Token).ConfigureAwait(false); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.ResetItemsAsync(null, cts.Token).ConfigureAwait(false); });
                }
            }
        }

        [Fact]
        public async Task TestRepositoryAsync()
        {
            using (var context = new DatabaseTestContext(Path.GetTempFileName()))
            {
                var repository = GetRepository(context);
                var result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.False(result.Any(), $"Repository {repository.GetType().Name} is not empty.");
                var item = Activator.CreateInstance<TU>();
                item.Id = 1;
                var boolResult = await repository.AddItemAsync(item, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult, $"Repository {repository.GetType().Name} is did not add item.");
                result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.Single(result, (i) => i.Id == item.Id);
                var itemResult = await repository.GetItemByIdAsync(0, CancellationToken.None).ConfigureAwait(false);
                Assert.Null(itemResult);
                itemResult = await repository.GetItemByIdAsync(1, CancellationToken.None).ConfigureAwait(false);
                Assert.NotNull(itemResult);
                Assert.Equal(item?.Id, itemResult?.Id);
                var itemResults = await repository.GetItemsByIdsAsync(null, CancellationToken.None).ConfigureAwait(false);
                Assert.NotNull(itemResults);
                Assert.Empty(itemResults);
                itemResults = await repository.GetItemsByIdsAsync(new List<long>() { 10, 11 }, CancellationToken.None).ConfigureAwait(false);
                Assert.NotNull(itemResults);
                Assert.Empty(itemResults);
                itemResults = await repository.GetItemsByIdsAsync(new List<long>() { 1, 10, 11 }, CancellationToken.None).ConfigureAwait(false);
                Assert.NotNull(itemResults);
                Assert.Single(itemResults);
                Assert.Equal(item?.Id, itemResults.First().Id);

                var item1 = Activator.CreateInstance<TU>();
                item1.Id = 1;
                boolResult = await repository.AddItemAsync(item1, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult, $"Repository {repository.GetType().Name} is did not add item.");
                result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.Single(result);
                Assert.Equal(item1.Id, result.First().Id);

                // We are removing by id. Not by instance.
                boolResult = await repository.RemoveItemAsync(item, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.Empty(result);

                // We are removing by id. Not by instance.
                await repository.AddItemAsync(item, CancellationToken.None).ConfigureAwait(false);
                boolResult = await repository.RemoveItemAsync(item1, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.Empty(result);

                // We should get a single item. because we are using id.
                boolResult = await repository.ResetItemsAsync(new List<TU> { item, item1 }, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.Single(result);

                boolResult = await repository.ResetItemsAsync(null, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.Empty(result);

                boolResult = await repository.AddItemsAsync(null, CancellationToken.None).ConfigureAwait(false);
                Assert.False(boolResult);

                boolResult = await repository.AddItemsAsync(new List<TU> { item, item1 }, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult);

                result = await repository.GetItemsAsync(CancellationToken.None).ConfigureAwait(false);
                Assert.Single(result);

                boolResult = await repository.RemoveItemsAsync(null, CancellationToken.None).ConfigureAwait(false);
                Assert.True(boolResult);
            }
        }

        protected abstract IRepository<TU> GetRepository(DatabaseTestContext context);
    }
}
