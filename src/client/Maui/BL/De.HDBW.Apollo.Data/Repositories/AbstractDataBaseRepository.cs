// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using SQLite;

namespace De.HDBW.Apollo.Data.Repositories
{
    public abstract class AbstractDataBaseRepository<TU> :
        IRepository<TU>,
        IDatabaseRepository<TU>
        where TU : IEntity, new()
    {
        public AbstractDataBaseRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger logger)
        {
            Logger = logger;
            DataBaseConnectionProvider = dataBaseConnectionProvider;
        }

        protected ILogger Logger { get; set; }

        protected IDataBaseConnectionProvider DataBaseConnectionProvider { get; set; }

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

            return ResetAsync(items, token);
        }

        public async Task<TU?> GetItemByIdAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.GetAsync<TU>(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TU>> GetItemsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<TU>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TU>> GetItemsByIdsAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<TU>().Where(p => ids.Contains(p.Id)).ToListAsync().ConfigureAwait(false);
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

        public async Task<bool> RemoveItemsByIdsAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            await asyncConnection.RunInTransactionAsync((connection)
               =>
            {
                connection.Table<TU>().Delete(c => ids.Contains(c.Id));
            }).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> ResetItemsAsync(IEnumerable<TU>? items, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            await asyncConnection.RunInTransactionAsync((connection)
               =>
            {
                connection.Execute(@"PRAGMA defer_foreign_Keys=On;");
                connection.DeleteAll<TU>();
                if (items?.Any() ?? false)
                {
                    connection.InsertAll(items, "OR REPLACE", false);
                }

                connection.Execute(@"PRAGMA defer_foreign_Keys=Off;");
            }).ConfigureAwait(false);
            return true;
        }

        public Task<bool> UpdateItemAsync(TU item, CancellationToken token)
        {
            return AddOrUpdateItemAsync(item, token);
        }

        public Task<bool> UpdateItemsAsync(IEnumerable<TU> items, CancellationToken token)
        {
            return AddOrUpdateItemsAsync(items, token);
        }

        private async Task<bool> ResetAsync(IEnumerable<TU> items, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Logger?.LogInformation($"Called {nameof(ResetAsync)} in {GetType().Name}. Items count: {items?.Count() ?? -1}.");
            if (!(items?.Any() ?? false))
            {
                return false;
            }

            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            await asyncConnection.RunInTransactionAsync((connection)
                =>
            {
                var mapping = connection.GetMapping<TU>();
                try
                {
                    long affectedRows = 0;
                    var rowIdCommand = new SQLiteCommand(connection);
                    rowIdCommand.CommandText = $"SELECT last_insert_rowid()";
                    foreach (var item in items)
                    {
                        var columns = item.Id == -1 ?
                                mapping.Columns.Where(x => !x.Name.Equals("id", StringComparison.OrdinalIgnoreCase)).ToArray() :
                                mapping.Columns;
                        var command = new SQLiteCommand(connection);
                        var columnsJoined = string.Join(", ", columns.Select(c => c.Name));
                        command.CommandText = $"REPLACE INTO {mapping.TableName}({columnsJoined}) VALUES ({string.Join(", ", Enumerable.Range(0, columns.Length).Select(p => $"@p{p}"))})";

                        for (var i = 0; i < columns.Length; i++)
                        {
                            var currentDef = columns[i];
                            command.Bind($"@p{i}", currentDef.GetValue(item));
                        }

                        var result = command.ExecuteNonQuery();
                        if (result == 0)
                        {
                            continue;
                        }

                        affectedRows += result;
                        if (item.Id == -1)
                        {
                            item.Id = rowIdCommand.ExecuteScalar<int>();
                        }
                    }

                    Logger?.LogDebug($"REPLACE in {mapping.TableName} affected {affectedRows} Rows");
                }
                catch (SQLiteException ex)
                {
                    Logger?.LogError(ex, $"SqliteException in {nameof(ResetAsync)} in Repository: {mapping.TableName}");

                    foreach (var item in items)
                    {
                        try
                        {
                            connection.InsertOrReplace(item);
                        }
                        catch (SQLiteException)
                        {
                            Logger?.LogInformation($"Item in Error {item?.GetType().Name ?? "null"}");
                        }
                    }

                    throw;
                }
            }).ConfigureAwait(false);
            return true;
        }
    }
}
