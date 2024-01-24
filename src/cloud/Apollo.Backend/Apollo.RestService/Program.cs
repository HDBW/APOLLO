
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Apollo.Api;
using Apollo.RestService.Midleware;
using Apollo.Service.Middleware;
using Daenet.ApiKeyAuthenticator;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Apollo.RestService;
using TrainingControllerIntegrationTests;

namespace Apollo.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen((c) => {


                c.DescribeAllParametersInCamelCase();
                
                //c.UseInlineDefinitionsForEnums();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "APOLLO REST Service",
                    Description = "REST API for APOLLO Backend. Copyright HDBW",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = @"Please provide the key in the value field bellow.",
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKey"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            },
                            Scheme = "ApiKey",
                            Name = "ApiKey",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();
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

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new ApolloExceptionFilter());
            });

            // Makes sure that null values are not serialized and all props are CamelCased.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.KebabCaseUpper;
                //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.KebabCaseUpper;
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>()
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       config.AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
                   });
           });

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
            ApolloApiConfig apiCfg = new ApolloApiConfig();
            builder.Configuration.GetSection("ApolloApiConfig").Bind(apiCfg);
            builder.Services.AddSingleton(apiCfg);

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
