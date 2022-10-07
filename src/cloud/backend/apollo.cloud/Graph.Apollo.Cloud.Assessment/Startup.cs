// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Graph.Apollo.Cloud.Assessment.Services;
using Graph.Apollo.Cloud.Common.Models;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Server;
using ProtoBuf.Grpc.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Graph.Apollo.Cloud.Assessment;

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

        //services.AddSingleton<IAssessmentService, Services.AssessmentService>();
        services.TryAddSingleton(BinderConfiguration.Create(binder: new ServiceBinderWithServiceResolutionFromServiceCollection(services)));

        services.AddCodeFirstGrpcReflection();

        //services.AddGrpcHealthChecks(o =>
        //{
        //    o.Services.MapService("Assessments.Greeter", r => r.Tags.Contains("assessment"));
        //});
        
        services.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AssessmentService>();
                //TODO: Add Healthchecks to Assessments
                //endpoints.MapGrpcHealthChecksService();
            });
        }
}
