// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
// TODO: Implement Azure Workload Identity and get this from KeyVault
if (Log.IsEnabled(LogEventLevel.Information))
{
    Log.Information("Starting API Configuration");
}

// we want to be sure we are able to Log Anything
builder.Logging.ClearProviders();
// Add Serilog configuration
var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose() //TODO: @Talisi - change to desired level
    .WriteTo.Console()
    .WriteTo.Debug()
    .Enrich.FromLogContext()
    .CreateLogger();
//TODO: Add Azure Application Insights to Serilog Sinks and Configuration
builder.Logging.AddSerilog(logger);

if (Log.IsEnabled(LogEventLevel.Information))
{
    Log.Information("Logging successfully Initialized");
}

try
{
    builder.Services.AddAuthentication("Bearer").AddJwtBearer();
    builder.Services.AddAuthorization();

    builder.Services.AddAuthorizationBuilder().AddPolicy("admin_greetings",policy =>
        policy
            .RequireRole("admin")
            .RequireClaim("scope", "greetings_api"));
}
catch (Exception e)
{
    Log.Fatal(e, "API Configuration failed for Authentication");
    Log.Fatal(e.Message);
    throw;
}

//Gonna add a nice Database here - MongoDB
try
{
    if (Log.IsEnabled(LogEventLevel.Information))
    {
        Log.Information("API Configuration Initialize MongoDB");
    }
    
    builder.Services.Configure<TrainingsDatabaseSettings>(
        builder.Configuration.GetSection("TrainingsStoreDatabase"));
    builder.Services.AddSingleton<TrainingsService>();
}
catch (Exception e)
{
    Log.Fatal(e, "API Configuration failed for MongoDB");
    Log.Fatal(e.Message);
    throw;
}

try
{
    // We want to use OpenAPI as well as Endpoints discovery for our API
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version = "v1" });
    });
}
catch (Exception e)
{
    Log.Fatal(e, "API Configuration failed for Swagger and Swagger UI");
}


var app = builder.Build();
app.UseStatusCodePages();

//Enable Middleware to Authenticate and Authorize Requests
try
{
    //app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
}
catch (Exception e)
{
    Log.Fatal(e, "API Configuration failed for Authentication and Authorization");
    Log.Fatal(e.Message);
}

// Enable middleware to serve generated Swagger as a JSON endpoint.
try
{
    if (Log.IsEnabled(LogEventLevel.Information))
    {
        Log.Information("API Configuration Initialize Swagger");
    }

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));

    if (Log.IsEnabled(LogEventLevel.Information))
    {
        Log.Information("API Configuration Swagger initialized");
    }
}
catch (Exception e)
{
    Log.Fatal(e, "API Configuration failed Initialize Swagger");
}

app.MapGet("/hello", () => "Joshua:> Hello Prof. Falken!")
    .RequireAuthorization("admin_greetings");

app.MapGet("/", async (HttpContext context) =>
{
    if (Log.IsEnabled(LogEventLevel.Information))
    {
        Log.Information($"Hello World! send to {context.Connection.RemoteIpAddress} at UTC: {DateTime.UtcNow}");
    }
    await context.Response.WriteAsync($"{DateTime.UtcNow} ><> Hello World!\n                                                                                                                                                                                                         \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                                                                                                                                                                                        \r\n                                          .,**//*,.       ......                                                                                                                                        \r\n                                     .%&@@@@@@@@@@@@@@&(  .,,,,.                                                                                                                                        \r\n                                  .%@@@@@@#/,.  .,/%@@@@@@#,,,,.                                                                                                                                        \r\n                                .@@@@@%               .&@@%,,,,.                                                                             #@*    *@(                                                 \r\n                               *@@@@#                   .&%,,,,.                                ,**.   ,,.  ,,.   .,.                        #@*    *@(           .,.                                   \r\n                              ,@@@@*                      (,,,,.                            /@&%(**(&@@&@*  &@%@&#/*/#&@#     .%@&#//(%&@*   #@*    *@(      .%@&(**/%@&/                               \r\n                              #@@@@         .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.             &@*        /@@*  &@&         &@,  /@%        .&&. #@*    *@(     #@(        .@&.                             \r\n                              #@@@&         ..,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.            /@#          %@*  &@,         .@@ .@&          /@( #@*    *@(     &&          /@/                             \r\n                              /@@@@.                      ,,,,,.                           &&.        ,@@*  &@#         %@*  #@/         &@, .&&.    &@.    %@/        .@@,                             \r\n                               (@@@@,                    (%,,,,.                            /@&#,  ,%@@&@*  &@&@&/. ./&@&.    *@@#,  ./&@%.   .%@&/   #@&/   ,&&%*  .(&@%                               \r\n                                /@@@@%.                *&@%,,,,.                                ,**,   /(,  &@,  ,/(/,           ./(((,           *//    ,/(     ,//*.                                  \r\n                                  %@@@@@&/         .(@@@@@%.,,,.                                                                                                                                        \r\n                                    ,%@@@@@@@@@@@@@@@@@@(.......                                                                                                                                        \r\n                                         *%&&@@@&&%#,     ......                                                                                                                                        ");
});

//TODO: Add Authentication and Authorization
//TODO: Add Authorization for Endpoints
//TODO: Add Flags (Obsolete, Deprecated, etc.) to Endpoints https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-7.0


app.MapGet("/api/v1/training", async (HttpContext context, TrainingsService trainingsService) =>
    {
        if (Log.IsEnabled(LogEventLevel.Information))
        {
            Log.Information($"{DateTime.UtcNow} ><> {context.Connection.RemoteIpAddress} requested GET/trainings");
        }
        return Results.Ok(trainingsService.Get());
    })
    .WithName("GetTrainings")
    .WithOpenApi();

app.MapGet("/api/v1/training/{id}", async (string id, HttpContext context, TrainingsService trainingsService) =>
    {
        Log.Information($"{DateTime.UtcNow} ><> {context.Connection.RemoteIpAddress} requested GET/trainings/{id}");
        return Results.Ok(trainingsService.Get(id));
    })
    .WithName("GetTraining")
    .WithOpenApi();

//TODO: you can actually define endpoints somewhere else? https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0
//TODO: Enable Bulk Operations
//TODO: Implement Validation / Error Handling and CorrelationId
app.MapPost("/api/v1/training", async (Training training, HttpContext context, TrainingsService trainingsService) =>
    {
        if (Log.IsEnabled(LogEventLevel.Information))
        {
            Log.Information($"{DateTime.UtcNow} ><> {context.Connection.RemoteIpAddress} requested POST/trainings/");
        }
        //TODO: Object Id should be null
        //TODO: Validate Object https://blog.safia.rocks/endpoint-filters-exploration.html
        // Making sure we don't have an ID in the request, however there has to be a better way to do this, but it is late and I am tired
        training = training with { Id = null };
        return TypedResults.Ok(trainingsService.Create(training));
    })
    .WithName("PostTraining")
    .WithOpenApi();

app.MapPut("/api/v1/training", async (string id, Training training, HttpContext context, TrainingsService trainingsService) =>
{
    if (Log.IsEnabled(LogEventLevel.Information))
    {
        Log.Logger.Information($"{DateTime.UtcNow} ><> {context.Connection.RemoteIpAddress} requested PUT/{id}");
    }
    //TODO: Object Id should be null
    //TODO: Validate Object https://blog.safia.rocks/endpoint-filters-exploration.html
    // Making sure we don't have an ID in the request, however there has to be a better way to do this, but it is late and I am tired
    try
    {
        trainingsService.Update(id, training);
        return Results.Ok();
    }
    catch (Exception e)
    {
        if (Log.IsEnabled(LogEventLevel.Information))
        {
            Guid correlationId = Guid.NewGuid();
            Log.Logger.Error($"{DateTime.UtcNow} Error: {correlationId} | Client {context.Connection.RemoteIpAddress} requested PUT/{id} - TrainingsService throw a exception: {e.Message}");
        }
        return Results.BadRequest("Something went wrong! Contact Support: correlationId");
    }

})
    .WithName("UpdateTraining")
    .WithOpenApi();

app.MapDelete("/api/v1/training/{id}", async (string id, HttpContext context, TrainingsService trainingsService) =>
{
    if (Log.IsEnabled(LogEventLevel.Information))
    {
        Log.Logger.Information($"{DateTime.UtcNow} ><> {context.Connection.RemoteIpAddress} requested DELETE/{id}");
    }

    try
    {
        trainingsService.Remove(id);
        return Results.Ok();
    }
    catch (Exception e)
    {
        Guid correlationId = Guid.NewGuid();
        if (Log.IsEnabled(LogEventLevel.Information)) ;
        {
            Log.Error($"{DateTime.UtcNow} Error: {correlationId} | Client {context.Connection.RemoteIpAddress} requested DELETE/{id} - TrainingsService throw a exception: {e.Message}");
        }
        return Results.BadRequest($"Something went wrong! Contact Support: {correlationId}");
    }
})
    .WithName("DeleteTraining")
    .WithOpenApi();

app.Run();

public record Training
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Title")]
    public string TrainingName { get; set; }

    [BsonElement("Identifier")]
    public string Identifier { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }


}

public class TrainingsDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TrainingsCollectionName { get; set; } = null!;
}

public class TrainingsService
{
    private readonly IMongoCollection<Training> _trainings;

    public TrainingsService(IOptions<TrainingsDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);

        _trainings = database.GetCollection<Training>(settings.Value.TrainingsCollectionName);
    }

    public List<Training> Get() =>
        _trainings.Find(training => true).ToList();

    public Training Get(string id) =>
        _trainings.Find<Training>(training => training.Id == id).FirstOrDefault();

    public Training Create(Training training)
    {
        _trainings.InsertOne(training);
        return training;
    }

    public void Update(string id, Training trainingIn) =>
        _trainings.ReplaceOne(training => training.Id == id, trainingIn);

    public void Remove(Training trainingIn) =>
        _trainings.DeleteOne(training => training.Id == trainingIn.Id);

    public void Remove(string id) =>
        _trainings.DeleteOne(training => training.Id == id);
}
