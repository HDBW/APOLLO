using System.Diagnostics;
using System.Globalization;
using Invite.Apollo.App.Graph.Assessment.Data;
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
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCodeFirstGrpc(config =>
        {
            config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            config.EnableDetailedErrors = true;

            //TODO: https://docs.microsoft.com/en-us/aspnet/core/grpc/performance?view=aspnetcore-6.0
        });

        services.AddDbContext<AssessmentContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //services.AddSingleton<IAssessmentService, Services.AssessmentService>();
        services.TryAddSingleton(BinderConfiguration.Create(binder: new ServiceBinderWithServiceResolutionFromServiceCollection(services)));

        services.AddCodeFirstGrpcReflection();

        //services.AddGrpcHealthChecks(o =>
        //{
        //    o.Services.MapService("Assessments.Greeter", r => r.Tags.Contains("assessment"));
        //});
        
        //services.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
