// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Graph.Apollo.Cloud.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Repositories
{
    public abstract class AbstractInMemoryRepository<TU> :
        IRepository<TU>,
        IDatabaseRepository<TU>
        where TU : IEntity, new()
    {
        private readonly List<TU> _items = new List<TU>();

        public Task<bool> AddItemAsync(TU item, CancellationToken token)
        {
            return AddOrUpdateItemAsync(item, token);
        }

        public Task<bool> AddItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            return AddOrUpdateItemsAsync(items, token);
        }

        public Task<bool> AddOrUpdateItemAsync(TU item, CancellationToken token)
        {
            return AddOrUpdateItemsAsync(new List<TU>() { item }, token);
        }

        public Task<bool> AddOrUpdateItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var ids = items.Select(x => x.Id);
            var itemsToRemove = items.Where(i => ids.Contains(i.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                _items.Remove(item);
            }

            _items.AddRange(items);
            return Task.FromResult(true);
        }

        public Task<TU?> GetItemByIdAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = _items.FirstOrDefault(i => i.Id == id);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<TU>> GetItemsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = new ReadOnlyCollection<TU>(_items) as IEnumerable<TU>;
            return Task.FromResult(result);
        }

        public Task<IEnumerable<TU>> GetItemsByIdsAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = new ReadOnlyCollection<TU>(_items.Where(i => ids.Contains(i.Id)).ToList()) as IEnumerable<TU>;
            return Task.FromResult(result);
        }

        public Task<bool> RemoveItemAsync(TU item, CancellationToken token)
        {
            return RemoveItemsByIdsAsync(new List<long>() { item.Id }, token);
        }

        public Task<bool> RemoveItemByIdAsync(long id, CancellationToken token)
        {
            return RemoveItemsByIdsAsync(new List<long>() { id}, token);
        }

        public Task<bool> RemoveItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            return RemoveItemsByIdsAsync(items.Select(i => i.Id), token);
        }

        public Task<bool> RemoveItemsByIdsAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var itemsToRemove = _items.Where(i => ids.Contains(i.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                _items.Remove(item);
            }

            return Task.FromResult(true);
        }

        public Task<bool> ResetItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            _items.Clear();
            _items.AddRange(items);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateItemAsync(TU item, CancellationToken token)
        {
            return AddOrUpdateItemAsync(item, token);
        }

        public Task<bool> UpdateItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            return AddOrUpdateItemsAsync(items, token);
        }
    }
}
