// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Security.Cryptography;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// TODO: Implement Azure Workload Identity and get this from KeyVault
Log.Information("Starting API Configuration");

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
Log.Logger.Information("Logging successfully Initialized");

//Gonna add a nice Database here - MongoDB
try
{
    Log.Logger.Information("API Configuration Initialize MongoDB");
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
    Log.Logger.Fatal(e, "API Configuration failed for Swagger and Swagger UI");
}

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.
try
{
    Log.Logger.Information("API Configuration Initialize Swagger");
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
}
catch (Exception e)
{
    Log.Logger.Fatal(e, "API Configuration failed Initialize Swagger");
}

app.MapGet("/", async (HttpContext context) =>
{
    Log.Logger.Information($"Hello World! send to {context.Connection.RemoteIpAddress} at UTC: {DateTime.UtcNow}");
    await context.Response.WriteAsync($"{DateTime.UtcNow} ><> Hello World!");
});

//TODO: Add Authentication and Authorization
//TODO: Add Authorization for Endpoints
//TODO: Add Flags (Obsolete, Deprecated, etc.) to Endpoints https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-7.0

app.MapGet("/api/v1/training", async (HttpContext context, TrainingsService trainingsService) =>
{
    Log.Logger.Information($"{DateTime.UtcNow} ><> {context.Connection.RemoteIpAddress} requested GET/trainings");
    return Results.Ok(trainingsService.Get());
})
    .WithName("Training")
    .WithOpenApi();


app.Run();

public class Training
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
