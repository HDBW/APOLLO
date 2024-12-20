﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Helper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Extensions
{
    internal static class TestExtensions
    {
        internal static IDataBaseConnectionProvider SetupDataBaseConnectionProvider(this object test, DatabaseTestContext context)
        {
            var mock = new Mock<IDataBaseConnectionProvider>();
            mock.Setup(m => m.GetConnectionAsync(It.IsAny<CancellationToken>()))
                .Returns((CancellationToken token) =>
                {
                    token.ThrowIfCancellationRequested();
                    return Task.FromResult(context.Connection);
                });
            return mock.Object;
        }

        internal static ILogger<TU> SetupLogger<TU>(this object test, ITestOutputHelper? output = null)
        {
            var mock = new Mock<ILogger<TU>>();
            mock.Setup(m => m.Log<It.IsAnyType>(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((
                     logLevel,
                     eventId,
                     state,
                     exception,
                     formatter) =>
                {
                    var message = $"Level:{logLevel}{Environment.NewLine}EventId:{eventId}{Environment.NewLine}Message:{formatter.DynamicInvoke(state, exception)}{Environment.NewLine}";
                    output?.WriteLine(message);
                    Console.WriteLine(message);
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine(message);
                    }
                });
            mock.Setup(m => m.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
            return mock.Object;
        }

        internal static ILoggerProvider SetupLoggerProvider<TU>(this object test, ITestOutputHelper? output = null)
        {
            var mock = new Mock<ILoggerProvider>();
            var mockLogger = test.SetupLogger<TU>(output);
            mock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger);
            return mock.Object;
        }
    }
}
