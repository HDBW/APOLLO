
using Apollo.Api;
using Apollo.Service.Middleware;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;

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
            builder.Services.AddSwaggerGen();

            // Registers the action filter
            builder.Services.AddControllers().AddMvcOptions(options =>
            {
                options.Filters.Add(new ApiPrincipalFilter());
            });
          
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

        private static void RegisterApi(WebApplicationBuilder builder)
        {

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
