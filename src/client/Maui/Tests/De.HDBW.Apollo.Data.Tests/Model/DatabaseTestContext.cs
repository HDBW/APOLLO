// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Reflection;
using De.HDBW.Apollo.SharedContracts.Models;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.Extensions.Logging;
using SQLite;

namespace De.HDBW.Apollo.Data.Tests.Model
{
    public class DatabaseTestContext : IDisposable
    {
        public DatabaseTestContext(string path, ILogger logger)
        {
            Path = path;
            var flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
            Connection = new SQLiteAsyncConnection(Path, flags);
            Connection.Tracer = new Action<string>(q =>
            {
                if (logger?.IsEnabled(LogLevel.Debug) ?? false)
                {
                    logger?.LogDebug(q);
                }
            });

            Connection.Trace = true;
            var entityType = typeof(IEntity);
            var entities = Assembly.GetAssembly(typeof(BaseItem))?.GetTypes().Where(t => t.IsPublic && t.IsClass && t != typeof(BaseItem) && entityType.IsAssignableFrom(t)).ToList() ?? new List<Type>();
            entities.AddRange(Assembly.GetAssembly(typeof(SearchHistory))?.GetTypes().Where(t => t.IsPublic && t.IsClass && t != typeof(BaseItem) && entityType.IsAssignableFrom(t)).ToList() ?? new List<Type>());
            entities.Select(entity => (entity, Connection.GetConnection().CreateTable(entity, CreateFlags.ImplicitPK | CreateFlags.ImplicitIndex))).ToList();
        }

        public string Path { get; }

        public SQLiteAsyncConnection Connection { get; private set; }

        public void Dispose()
        {
            Connection?.GetConnection().Close();
            Connection?.GetConnection().Dispose();
            Connection = null;
            File.Delete(Path);
        }
    }
}
