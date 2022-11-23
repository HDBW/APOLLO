using System.Data;
using System.Diagnostics;
using Invite.Apollo.App.Graph.Assessment.Data;
using Invite.Apollo.App.Graph.Assessment.Logs;
using Invite.Apollo.App.Graph.Assessment.Repository;
using Invite.Apollo.App.Graph.Assessment.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Grpc.Server;
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
        //services.AddGrpc();
        services.AddLogging();
        //services.AddSingleton<IDataService,AssessmentDataService>();
        

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

        //services.AddDbContext<CourseContext>(options =>
        //{
        //    options.UseSqlServer(Configuration.GetConnectionString("CourseSql")).LogTo(Log.Logger.Information, LogLevel.Information, null);
        //    options.EnableSensitiveDataLogging();
        //});
        //Trace.TraceInformation($"ef writing on database {Configuration.GetConnectionString("CourseSql")}");

        #endregion



        DiagnosticListener.AllListeners.Subscribe(new DiagnosticObserver());


        //services.AddScoped<IAssessmentGRPCService, Services.AssessmentGrpcService>();
        //services.AddSingleton<IAssessmentGRPCService, Services.AssessmentGrpcService>();
        services.TryAddSingleton(BinderConfiguration.Create(binder: new ServiceBinderWithServiceResolutionFromServiceCollection(services)));

        services.AddCodeFirstGrpcReflection();

        services.AddScoped<IAssessmentRepository, AssessmentRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerHasMetaDataRepository, AnswerHasMetaDataRepository>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEscoSkillRepository, EscoSkillRepository>();
        services.AddScoped<IMetaDataRepository, MetaDataRepository>();
        services.AddScoped<IQuestionHasMetaDataRepository, QuestionHasMetaDataRepository>();
        services.AddScoped<IMetaDataHasMetaDataRepository, MetaDataHasMetaDataRepository>();
        





        services.AddScoped<IDataService, AssessmentDataService>();



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
            endpoints.MapGrpcService<AssessmentGrpcService>();
            //TODO: Add Healthchecks to Assessments
            //endpoints.MapGrpcHealthChecksService();
        });
    }



}
