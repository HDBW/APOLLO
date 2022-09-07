using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace GreeterServiceTests.IntegrationTests.Helpers
{
    public delegate void LogMessage(LogLevel logLevel, string categoryName, EventId eventId, string message, Exception? exception);

    public class GrpcTestFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer _server;
        private readonly IHost _host;

        public event LogMessage? LoggedMessage;

        public GrpcTestFixture() : this(null) { }

        public GrpcTestFixture(Action<IServiceCollection>? initialConfigureServices)
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new ForwardingLoggerProvider((logLevel, category, eventId, message, exception) =>
            {
                LoggedMessage?.Invoke(logLevel, category, eventId, message, exception);
            }));

            var builder = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ILoggerFactory>(LoggerFactory);
                })
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost
                        .UseTestServer()
                        .UseStartup<TStartup>();

                    if (initialConfigureServices != null)
                    {
                        webHost.ConfigureServices(initialConfigureServices);
                    }
                });
            _host = builder.Start();
            _server = _host.GetTestServer();

            Handler = _server.CreateHandler();
        }

        public LoggerFactory LoggerFactory { get; }

        public HttpMessageHandler Handler { get; }

        public void Dispose()
        {
            Handler.Dispose();
            _host.Dispose();
            _server.Dispose();
        }

        public IDisposable GetTestContext()
        {
            return new GrpcTestContext<TStartup>(this);
        }
    }
}
