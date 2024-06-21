// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Api;
using Apollo.Common.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using Newtonsoft.Json;
using System.Text.Json;
using System.Globalization;
using System.Threading.Channels;
using Apollo.SmartLib;
using Daenet.MongoDal;
using Daenet.EmbeddingSearchApi.Interfaces;
using Daenet.EmbeddingSearchApi.Services;
using Daenet.EmbeddingSearchApi.Entities;
using Daenet.EmbeddingSearchApi.Api.Convertors;
using Daenet.EmbeddingSearchApi.Api.Entities;
using Daenet.EmbeddingSearchApi.Api.Services;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace Apollo.ProfileImporter
{

    /// <summary>
    /// The main entry point for the Profile Importer application.
    /// This class configures and starts the host for the application.
    /// It sets up configuration sources, logging, services, and dependencies required for running the Profile Importer Service.
    /// </summary>
    public class Program
    {

        /// <summary>
        /// The main method that starts the application.
        /// It creates, configures, and runs the host which orchestrates the importing services.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application.</param>
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            await builder.RunAsync();
        }


        /// <summary>
        /// Configures the host builder with settings, services, and logging.
        /// This includes setting up the app configuration, registering services, and configuring logging.
        /// </summary>
        /// <param name="args">Arguments provided at application startup.</param>
        /// <returns>The configured IHostBuilder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    // Register MongoDataAccessLayer
                    services.AddSingleton(sp =>
                    {
                        var configuration = sp.GetRequiredService<IConfiguration>();
                        var mongoConnStr = configuration["MongoDb:ConnectionString"];
                        var apolloDb = configuration["MongoDb:DatabaseName"] ?? "apollodb";

                        return new MongoDataAccessLayer(new Daenet.MongoDal.Entitties.MongoDalConfig
                        {
                            MongoConnStr = mongoConnStr,
                            MongoDatabase = apolloDb
                        });
                    });

                    // Register ApolloApi with its dependencies
                    services.AddSingleton(sp =>
                    {
                        var dal = sp.GetRequiredService<MongoDataAccessLayer>();
                        var logger = sp.GetRequiredService<ILogger<ApolloApi>>();
                        var config = new ApolloApiConfig(); 
                        return new ApolloApi(dal, logger, config, null); // null for ApolloSemanticSearchApi if not needed
                    });

                    // Register ProfileImporterService
                    services.AddHostedService<ProfileImporterService>();
                    //services.AddSingleton<ApolloSemanticSearchApi>();   Do we need it ?

                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                });
    }
}
