
using System;
using System.Reflection;
using Invite.Apollo.App.Graph.Assessment;
using Invite.Apollo.App.Graph.Assessment.Data;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Meta;
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
            //executes Startup
            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);
            
            host.Run();



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

    private static void CreateDbIfNotExists(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AssessmentContext>();
                DbInitializer.Initialize(context);
                //DbInitializer init = new DbInitializer();
                //init.Initialize(context);
            }
            catch (Exception exception)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(exception, "1-An error occurred creating the DB.");
                Log.Fatal(exception, "2-An error occurred creating the DB.");
            }
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
