// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.ProfileImporter
{
    internal class Program
    {
        /// <summary>
        /// Imports profiles from blob storage, maps them to the the profile including skills property and upsert them into the
        /// Apollo backend.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Import of profiles started");

            //InitializeConfiguration(args);

            //// Configure logging
            //using var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder
            //        .AddConsole() // Log to console
            //        .SetMinimumLevel(LogLevel.Debug);
            //});

            //_logger = loggerFactory.CreateLogger<Program>();

            //_logger.LogDebug("Exporter started..");

            //try
            //{
            //    ApolloApi api = GetApi();
            //}
            //catch (Exception ex)
            //{

            //}
        }
    }
}
