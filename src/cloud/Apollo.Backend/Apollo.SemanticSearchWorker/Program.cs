// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.IO.Compression;
using System.Text;
using System.Text.Unicode;
using Apollo.Api;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Daenet.MongoDal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Apollo.SemanticSearchWorker
{
    /// <summary>
    /// Implement sthe console application that exports the given Apollo entity into the CSV file
    /// compatible with daenet Semantc Search.
    /// </summary>
    internal class Program
    {
        private static IConfigurationRoot? _cfg;
        private static ILogger<Program>? _logger;

        /// <summary>
        /// Starts from the configuration provided as arguments or environment variables.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            InitializeConfiguration(args);


            // Configure logging
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole() // Log to console
                    .SetMinimumLevel(LogLevel.Debug); 
            });

            _logger = loggerFactory.CreateLogger<Program>();

            _logger.LogDebug("Exporter started..");
            
            try
            {
                ApolloApi api = GetApi();

                string? entity = _cfg!["entity"];
                if (entity == null)
                {
                    throw new ArgumentException("The entity name must be specified in the configuration. I.e:as argument  '--entity=training' or as environment variable 'entity=training'");
                }

                string? blobConnStr = _cfg["blobConnStr"];
                if (blobConnStr == null)
                {
                    throw new ArgumentException("The connection string of the blob storage with the write permission must be specified in the configuration. I.e:as argument  '--blobConnStr=...' or as environment variable 'blobConnStr=...'");
                }

                var exporterLogger = loggerFactory.CreateLogger<BlobStorageExporter>();

                // Initialize the exporter with the required parameters
                var exp = new BlobStorageExporter(api, entity, blobConnStr, exporterLogger);

                // Starts the long running operation.
                await exp.ExportAsync();

                _logger.LogDebug("Exporter completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the export process.");
            }
        }


        /// <summary>
        /// Gets the configuration from args or environment variables.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static void InitializeConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .AddCommandLine(args)
                 .AddEnvironmentVariables();

            _cfg = builder.Build();
        }

        /// <summary>
        /// Creates the API from configuration.
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        private static ApolloApi GetApi()
        {
            return new ApolloApi(GetDal(), GetLogger(), GetApolloConfig(),null);
        }

        private static ILogger<ApolloApi> GetLogger()
        {
            using var loggerFactory = LoggerFactory.Create(logBuilder =>
            {
                logBuilder.AddDebug();
                logBuilder.AddConsole();
                logBuilder.SetMinimumLevel(LogLevel.Trace);
            });

            return loggerFactory.CreateLogger<ApolloApi>();
        }

        private static ApolloApiConfig GetApolloConfig()
        {
            // Not used in the api right now.
            ApolloApiConfig cfg = new ApolloApiConfig
            {

            };

            return cfg;
        }



        private static MongoDataAccessLayer GetDal()
        {
            string? mongoConnStr = _cfg["mongoConnStr"];

            if (mongoConnStr == null)
                throw new ArgumentException("The connection string of the Mongo database must be specified in the configuration. I.e:as argument  '--mongoConnStr=...' or as environment variable 'mongoConnStr=...'");


            string? apolloDb = _cfg["apolloDb"];

            if (apolloDb == null)
                apolloDb = "apollodb";

            MongoDataAccessLayer dal = new MongoDataAccessLayer(new Daenet.MongoDal.Entitties.MongoDalConfig()
            {
                MongoConnStr = mongoConnStr,
                MongoDatabase = apolloDb
            });

            return dal;
        }
    }
}
