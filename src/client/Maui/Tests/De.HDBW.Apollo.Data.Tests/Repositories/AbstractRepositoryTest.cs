﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public abstract class AbstractRepositoryTest<TU> : AbstractTest
        where TU : IEntity, new()
    {
        protected AbstractRepositoryTest(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task TestRepositoryWithCanceledAndDisposedTokenAsync()
        {
            using (var context = new DatabaseTestContext(Path.GetTempFileName(), Logger))
            {
                var repository = GetRepository(context);
                using (var cts = new CancellationTokenSource())
                {
                    cts.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.GetItemsAsync(cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.GetItemByIdAsync(0, cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.GetItemsByIdsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddItemsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.RemoveItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.RemoveItemsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.RemoveItemByIdAsync(0, cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.ResetItemsAsync(null, cts.Token); });
                    cts.Dispose();
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.GetItemsAsync(cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.GetItemByIdAsync(0, cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.GetItemsByIdsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddItemsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.RemoveItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.RemoveItemsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.RemoveItemByIdAsync(0, cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.ResetItemsAsync(null, cts.Token); });
                }
            }
        }

        [Fact]
        public async Task TestRepositoryAsync()
        {
            using (var context = new DatabaseTestContext(Path.GetTempFileName(), Logger))
            {
                var repository = GetRepository(context);
                var result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.False(result.Any(), $"Repository {repository.GetType().Name} is not empty.");
                var item = Activator.CreateInstance<TU>();
                item.Id = 1;
                var boolResult = await repository.AddItemAsync(item, CancellationToken.None);
                Assert.True(boolResult, $"Repository {repository.GetType().Name} is did not add item.");
                result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.Single(result, (i) => i.Id == item.Id);
                var itemResult = await repository.GetItemByIdAsync(0, CancellationToken.None);
                Assert.Null(itemResult);
                itemResult = await repository.GetItemByIdAsync(item.Id, CancellationToken.None);
                Assert.NotNull(itemResult);
                Assert.Equal(item.Id, itemResult?.Id);
                var itemResults = await repository.GetItemsByIdsAsync(null, CancellationToken.None);
                Assert.NotNull(itemResults);
                Assert.Empty(itemResults);
                itemResults = await repository.GetItemsByIdsAsync(new List<long>() { 10, 11 }, CancellationToken.None);
                Assert.NotNull(itemResults);
                Assert.Empty(itemResults);
                itemResults = await repository.GetItemsByIdsAsync(new List<long>() { item.Id, 10, 11 }, CancellationToken.None);
                Assert.NotNull(itemResults);
                Assert.Single(itemResults);
                Assert.Equal(item?.Id, itemResults.First().Id);
                Assert.True(await repository.ResetItemsAsync(new List<TU>(), CancellationToken.None));

                var item1 = Activator.CreateInstance<TU>();
                item1.Id = 1;
                boolResult = await repository.AddItemAsync(item1, CancellationToken.None);
                Assert.True(boolResult, $"Repository {repository.GetType().Name} is did not add item.");
                result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.Single(result);
                Assert.Equal(item1.Id, result.First().Id);

                // We are removing by id. Not by instance.
                boolResult = await repository.RemoveItemAsync(item, CancellationToken.None);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.Empty(result);

                // We are removing by id. Not by instance.
                await repository.AddItemAsync(item, CancellationToken.None);
                boolResult = await repository.RemoveItemAsync(item1, CancellationToken.None);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.Empty(result);

                // We should get a single item. because we are using id.
                boolResult = await repository.ResetItemsAsync(new List<TU> { item, item1 }, CancellationToken.None);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.Single(result);

                boolResult = await repository.ResetItemsAsync(null, CancellationToken.None);
                Assert.True(boolResult);
                result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.Empty(result);

                boolResult = await repository.AddItemsAsync(null, CancellationToken.None);
                Assert.False(boolResult);

                boolResult = await repository.AddItemsAsync(new List<TU> { item, item1 }, CancellationToken.None);
                Assert.True(boolResult);

                result = await repository.GetItemsAsync(CancellationToken.None);
                Assert.Equal(2, result.Count());

                boolResult = await repository.RemoveItemsAsync(null, CancellationToken.None);
                Assert.False(boolResult);
            }
        }

        protected abstract IRepository<TU> GetRepository(DatabaseTestContext context);
    }
}
