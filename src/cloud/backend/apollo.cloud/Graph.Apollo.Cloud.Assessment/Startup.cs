using System.Diagnostics;
using System.Globalization;
using Invite.Apollo.App.Graph.Assessment.Data;
using Invite.Apollo.App.Graph.Assessment.Logs;
using Invite.Apollo.App.Graph.Assessment.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Grpc.Server;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Invite.Apollo.App.Graph.Assessment;

public class Startup
{
    //#region DefineLoggerFactory
    //public static readonly ILoggerFactory MyLoggerFactory
    //    = LoggerFactory.Create(builder => { builder.AddConsole(); });
    //#endregion

    //public IConfiguration Configuration { get; private set; }
    public IConfiguration Configuration { get; }

    public IWebHostEnvironment HostingEnvironment { get; private set; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        HostingEnvironment = env;
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddLogging();

        services.AddCodeFirstGrpc(config =>
        {
            config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            config.EnableDetailedErrors = true;

            //TODO: https://docs.microsoft.com/en-us/aspnet/core/grpc/performance?view=aspnetcore-6.0
        });

        #region DBContext

        services.AddDbContext<AssessmentContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("AzureSql")).LogTo(Log.Logger.Information, LogLevel.Information, null);
            options.EnableSensitiveDataLogging();
        });
        Trace.TraceInformation($"ef writing on database {Configuration.GetConnectionString("AzureSql")}");
        #endregion



        DiagnosticListener.AllListeners.Subscribe(new DiagnosticObserver());

        //services.AddSingleton<IAssessmentService, Services.AssessmentService>();
        services.TryAddSingleton(BinderConfiguration.Create(binder: new ServiceBinderWithServiceResolutionFromServiceCollection(services)));

        services.AddCodeFirstGrpcReflection();

        //services.AddGrpcHealthChecks(o =>
        //{
        //    o.Services.MapService("Assessments.Greeter", r => r.Tags.Contains("assessment"));
        //});
        
        //services.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddSerilog();  
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            //InitializeDb(app.ApplicationServices).Wait();
        }

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<AssessmentService>();
            //TODO: Add Healthchecks to Assessments
            //endpoints.MapGrpcHealthChecksService();
        });
    }



}
