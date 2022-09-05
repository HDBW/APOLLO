using Graph.Apollo.Cloud.Greeter.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Graph.Apollo.Cloud.Greeter
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(o => o.EnableDetailedErrors = true);
            services.AddSingleton<IGreeter, Services.Greeter>();
            services.AddGrpcHealthChecks(o =>
            {
                o.Services.MapService("greet.Greeter", r => r.Tags.Contains("greeter"));
            });
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
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcHealthChecksService();
            });
        }
    }
}
