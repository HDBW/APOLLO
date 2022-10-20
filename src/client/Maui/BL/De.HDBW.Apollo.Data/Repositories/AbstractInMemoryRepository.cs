// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.Data.Repositories
{
    public abstract class AbstractInMemoryRepository<TU> :
        IRepository<TU>,
        IDatabaseRepository<TU>
        where TU : IEntity, new()
    {
        private IEqualityComparer<TU> _comparer = new EntityComparer<TU>();

        protected readonly List<TU> _items = new List<TU>();

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
            return AddOrUpdateItemsAsync(item != null ? new List<TU>() { item } : new List<TU>(), token);
        }

        public Task<bool> AddOrUpdateItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (items == null)
            {
                return Task.FromResult(false);
            }

            var ids = items.Select(x => x.Id).Distinct();
            var itemsToRemove = _items.Where(i => ids.Contains(i.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                _items.Remove(item);
            }

            _items.AddRange(items.Distinct(_comparer));
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
            var result = new ReadOnlyCollection<TU>(new List<TU>());
            if (ids != null)
            {
                result = new ReadOnlyCollection<TU>(_items.Where(i => ids.Contains(i.Id)).ToList());
            }

            return Task.FromResult(result as IEnumerable<TU>);
        }

        public Task<bool> RemoveItemAsync(TU item, CancellationToken token)
        {
            return RemoveItemsByIdsAsync(item != null ? new List<long>() { item.Id } : new List<long>(), token);
        }

        public Task<bool> RemoveItemByIdAsync(long id, CancellationToken token)
        {
            return RemoveItemsByIdsAsync(new List<long>() { id }, token);
        }

        public Task<bool> RemoveItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            return RemoveItemsByIdsAsync(items != null ? items.Select(i => i.Id) : new List<long>(), token);
        }

        public Task<bool> RemoveItemsByIdsAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            ids = ids ?? new List<long>();
            var itemsToRemove = _items.Where(i => ids.Contains(i.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                _items.Remove(item);
            }

            return Task.FromResult(true);
        }

        public Task<bool> ResetItemsAsync(IEnumerable<TU>? items, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            _items.Clear();
            if (items != null)
            {
                _items.AddRange(items.Distinct(_comparer));
            }

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
