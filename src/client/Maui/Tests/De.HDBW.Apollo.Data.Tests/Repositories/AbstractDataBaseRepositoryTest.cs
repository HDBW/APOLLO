// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Repositories
{
    public abstract class AbstractDataBaseRepositoryTest<TU> : AbstractRepositoryTest<TU>
        where TU : IEntity, new()
    {
        protected AbstractDataBaseRepositoryTest(ITestOutputHelper outputHelper)
           : base(outputHelper)
        {
        }

        [Fact]
        public async Task TestDatabaseRepositoryWithCanceledAndDisposedTokenAsync()
        {
            using (var context = new DatabaseTestContext(Path.GetTempFileName(), Logger))
            {
                var repository = GetDataBaseRepository(context);
                using (var cts = new CancellationTokenSource())
                {
                    cts.Cancel();
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddOrUpdateItemsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.AddOrUpdateItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.UpdateItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<OperationCanceledException>(async () => { await repository.UpdateItemsAsync(null, cts.Token); });
                    cts.Dispose();
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddOrUpdateItemsAsync(null, cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.AddOrUpdateItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.UpdateItemAsync(default(TU), cts.Token); });
                    await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await repository.UpdateItemsAsync(null, cts.Token); });
                }
            }
        }

        [Fact]
        public async Task TestDatabaseRepositoryAsync()
        {
            using (var context = new DatabaseTestContext(Path.GetTempFileName(), Logger))
            {
                var repository = GetDataBaseRepository(context);
                using (var cts = new CancellationTokenSource())
                {
                    Assert.False(await repository.AddOrUpdateItemAsync(default(TU), cts.Token), "Passing null resulted in success.");
                    Assert.False(await repository.AddOrUpdateItemsAsync(null, cts.Token), "Passing null resulted in success.");
                    Assert.False(await repository.UpdateItemAsync(default(TU), cts.Token), "Passing null resulted in success.");
                    Assert.False(await repository.UpdateItemsAsync(null, cts.Token), "Passing null resulted in success.");
                    var instance = Activator.CreateInstance(typeof(TU)) as IEntity;
                    Assert.NotNull(instance);
                    instance!.Id = 1;
                    Assert.True(await repository.AddOrUpdateItemAsync((TU)instance, cts.Token), "Passing instance resulted in success.");
                    var instances = new List<TU>();
                    instances.Add((TU)instance);
                    instance = Activator.CreateInstance(typeof(TU)) as IEntity;
                    Assert.NotNull(instance);
                    instance!.Id = 2;
                    instances.Add((TU)instance);
                    Assert.True(await repository.AddOrUpdateItemsAsync(instances, cts.Token), "Passing instance resulted in success.");
                    instance = Activator.CreateInstance(typeof(TU)) as IEntity;
                    Assert.NotNull(instance);
                    instance!.Id = 3;
                    Assert.False(await repository.UpdateItemAsync((TU)instance, cts.Token), "Passing instance not in DB resulted in success.");
                }

                // TODO: Add tests for
                // AddOrUpdateItemsAsync(IEnumerable<TU> items, CancellationToken token);
                // AddOrUpdateItemAsync(TU item, CancellationToken token);
                // UpdateItemAsync(TU item, CancellationToken token);
                // UpdateItemsAsync(IEnumerable<TU> items, CancellationToken token);
            }
        }

        private IDatabaseRepository<TU> GetDataBaseRepository(DatabaseTestContext context)
        {
            return GetRepository(context) as IDatabaseRepository<TU>;
        }
    }
}
