// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace TrainingControllerIntegrationTests
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var mongoDBConnectionString = Configuration.GetConnectionString("MongoConnStr");
            var mongoDatabaseName = Configuration["MongoDatabase"];


            services.Configure<MongoDBOptions>(options =>
            {
                options.ConnectionString = mongoDBConnectionString;
                options.DatabaseName = mongoDatabaseName; 
            });

            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MongoDBOptions>>().Value;
                var client = new MongoClient(options.ConnectionString);
                return client.GetDatabase(options.DatabaseName);
            });


            services.Configure<MongoDBOptions>(options =>
            {
                options.ConnectionString = mongoDBConnectionString;
            });

           
            services.AddControllers();

            // Configure other services here

            // Add Swagger/OpenAPI configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "REST SERVICE INTERGRATION TESTS", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApolloApi");
                    c.RoutePrefix = "test/swagger"; // Different route prefix for testing
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Add other endpoints here if needed
            });
        }

        public class MongoDBOptions
        {
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }
    }
}
