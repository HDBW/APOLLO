// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Reflection;
using De.HDBW.Apollo.SharedContracts.Helper;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.Extensions.Logging;
using Moq;
using SQLite;

namespace De.HDBW.Apollo.Data.Tests.Extensions
{
    internal static class TestExtensions
    {
        internal static IDataBaseConnectionProvider SetupDataBaseConnectionProvider(this object test)
        {
            var mock = new Mock<IDataBaseConnectionProvider>();
            var connection = new SQLiteAsyncConnection(":memory:");
            var entityType = typeof(IEntity);
            var entities = Assembly.GetAssembly(typeof(BaseItem))?.GetTypes().Where(t => t.IsPublic && t.IsClass && t != typeof(BaseItem) && entityType.IsAssignableFrom(t)).ToList() ?? new List<Type>();
            entities.Select(entity => (entity, connection.GetConnection().CreateTable(entity, CreateFlags.AutoIncPK | CreateFlags.ImplicitPK | CreateFlags.ImplicitIndex))).ToList();
            mock.Setup(m => m.GetConnectionAsync(It.IsAny<CancellationToken>()))
                .Returns((CancellationToken token) =>
                {
                    token.ThrowIfCancellationRequested();
                    return Task.FromResult(connection);
                });
            return mock.Object;
        }

        internal static ILogger<TU> SetupLogger<TU>(this object test)
        {
            var mock = new Mock<ILogger<TU>>();
            mock.Setup(m => m.Log<It.IsAnyType>(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
                .Callback<LogLevel, EventId, object, Exception?, Delegate>((
                     logLevel,
                     eventId,
                     state,
                     exception,
                     formatter) =>
                {
                    Console.WriteLine($"Level:{logLevel} EventId:{eventId} Message:{formatter.DynamicInvoke(state, exception)}");
                });
            return mock.Object;
        }
    }
}
