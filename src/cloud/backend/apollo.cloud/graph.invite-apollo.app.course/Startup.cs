using Graph.Invite.Apollo.App.Course;
using Graph.Invite.Apollo.App.Course.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;


public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    public IWebHostEnvironment HostingEnvironment { get; private set; }

    {
        Configuration = configuration;
        HostingEnvironment = env;
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger(); Configuration = configuration;

    }

    public void ConfigureServices(IServiceCollection service)
    {
        service.AddGrpc(o =>
        {
            o.EnableDetailedErrors = true;
            o.Interceptors.Add<RequestLogger>();
        });
    service.AddScoped<ApolloApi>();
    service.AddSingleton<MongoDataAccessLayer>();
    service.AddSingleton<IGreeter, Graph.Invite.Apollo.App.Course.Services.Greeter>();
    service.AddApplicationInsightsTelemetry(Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<GreeterService>();
        });
    }
}
