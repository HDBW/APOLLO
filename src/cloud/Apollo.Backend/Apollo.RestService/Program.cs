
using Apollo.Api;
using Apollo.Service.Middleware;
using Daenet.ApiKeyAuthenticator;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Apollo.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen((c) => {

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = @"Please provide the key in the value field bellow.",
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKey"
                });
            });

            // Registers the action filter
            builder.Services.AddControllers().AddMvcOptions(options =>
            {
                options.Filters.Add(new ApiPrincipalFilter());
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "ApiKey";
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddScheme<ValidateApiKeyOptions, ApiKeyAuthenticationHandler>
                  ("ApiKey", op =>
                  {
                  });

            RegisterDaenetMongoDal(builder);
            RegisterApi(builder);
            RegisterApiKey(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }



        private static void RegisterDal(WebApplicationBuilder builder)
        {
            MongoDalConfig cfg = new MongoDalConfig();
            var sec = builder.Configuration.GetSection("MongoDalConfig");
            sec.Bind(cfg);
            builder.Services.AddSingleton< MongoDataAccessLayer>();
            builder.Services.AddScoped<MongoDataAccessLayer>();
        }

        private static void RegisterApi(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ApolloApi>();
        }

        private static void RegisterApiKey(WebApplicationBuilder builder)
        {
            ApiKeyConfig apiKeyCfg = new ApiKeyConfig();

            builder.Configuration.GetSection("ApiKeyConfig").Bind(apiKeyCfg);

            builder.Services.AddSingleton(apiKeyCfg);
        }

        /// <summary>
        /// Registers the Daenet Mongo Dal.
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="Exception"></exception>
        private static void RegisterDaenetMongoDal(WebApplicationBuilder builder)
        {
            var cfg = builder.Configuration.GetSection("MongoDalConfig").Get<MongoDalConfig>();

            if (cfg == null)
            {
                throw new Exception("MongoDalConfig section is missing in the configuration file.");
            }

            builder.Services.AddSingleton(cfg);
            builder.Services.AddSingleton<MongoDataAccessLayer>();

            builder.Services.AddScoped<MongoDataAccessLayer>();
        }
    }
}
