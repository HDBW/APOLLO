
using System.Reflection;
using Invite.Apollo.App.Graph.Assessment;
using Serilog;
using Serilog.Events;

namespace Graph.Apollo.Cloud.Assessment;

public class Program
{
    public static void Main(string[] args) 
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft",LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information($"{DateTime.Now} : {Assembly.GetEntryAssembly()?.GetName().Name} - Starting up!");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Host terminated unexpectedly!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
