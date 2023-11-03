// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Daenet.MongoDal
{
    public class DaenetMongoDal
    {
        private readonly MongoDalConfig _config;

        public DaenetMongoDal(IOptions<MongoDalConfig> config)
        {
            _config = config.Value;

            if (string.IsNullOrWhiteSpace(_config.MongoConnStr))
            {
                throw new InvalidOperationException("MongoConnStr is missing or empty in configuration.");
            }

            if (string.IsNullOrWhiteSpace(_config.MongoDatabase))
            {
                throw new InvalidOperationException("MongoDatabase is missing or empty in configuration.");
            }

            // Initialize the DaenetMongoDal with the provided config.
        }

        public bool IsDatabaseConnected(IMongoClient mongoClient)
        {
            try
            {
                // Attempt to connect to the database
                var database = mongoClient.GetDatabase(_config.MongoDatabase);

                // This will throw an exception if the connection fails
                var collections = database.ListCollectionNames();

                return true; // If the connection is successful
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return false; // If the connection is not successful
            }
        }

        // Implementation of other methods and functionalities for MongoDB operations here
    }
}
