// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Reflection;
using De.HDBW.Apollo.SharedContracts.Helper;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.Extensions.Logging;
using SQLite;

namespace De.HDBW.Apollo.Data
{
    public class DataBaseConnectionProvider : IDataBaseConnectionProvider
    {
        public DataBaseConnectionProvider(string dbFilePath, SQLiteOpenFlags flags, ILogger<DataBaseConnectionProvider>? logger)
        {
            Connection = new SQLiteAsyncConnection(dbFilePath, flags);
            var entityType = typeof(IEntity);
            Logger = logger;
            Entities = Assembly.GetAssembly(typeof(BaseItem))?.GetTypes().Where(t => t.IsPublic && t.IsClass && t != typeof(BaseItem) && entityType.IsAssignableFrom(t)).ToList() ?? new List<Type>();
        }

        private SQLiteAsyncConnection Connection { get; }

        private List<Type> Entities { get; }

        private ILogger? Logger { get; }

        public async Task<SQLiteAsyncConnection> GetConnectionAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await EnsureDataBaseAsync(token).ConfigureAwait(false);
            return Connection;
        }

        private async Task<bool> EnsureDataBaseAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                Logger?.LogDebug($"{nameof(EnsureDataBaseAsync)} in {GetType().Name}.");
                var types = new List<Type>();
                await Connection.RunInTransactionAsync((SQLiteConnection connection) =>
                {
                    var result = Entities.Select(entity => (entity, connection.CreateTable(entity, CreateFlags.ImplicitPK | CreateFlags.ImplicitIndex))).ToList();
                }).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {nameof(EnsureDataBaseAsync)} in {GetType().Name}.");
            }

            return false;
        }
    }
}
