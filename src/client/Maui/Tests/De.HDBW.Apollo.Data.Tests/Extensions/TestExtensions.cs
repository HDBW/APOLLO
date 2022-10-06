using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace De.HDBW.Apollo.Data.Tests.Extensions
{
    internal static class TestExtensions
    {
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
