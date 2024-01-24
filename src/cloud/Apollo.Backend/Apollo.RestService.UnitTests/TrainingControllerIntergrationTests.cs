// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;
using Apollo.Service.Controllers;
using Daenet.MongoDal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Apollo.RestService.UnitTests
{
    [TestClass]
    public class TrainingControllerIntergrationTests
    {
        private TrainingController _controller;
        private ApolloApi _api;
        private ILogger<TrainingController> _logger;

        [TestInitialize]
        public void Init()
        {
            // Load your configuration (e.g., from appsettings.json)
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve the MongoDB connection string
            string mongoConnectionString = configuration.GetConnectionString("MongoConnStr");

            // Create MongoDB settings
            var mongoClientSettings = MongoClientSettings.FromConnectionString(mongoConnectionString);

            // Instantiate the MongoDataAccessLayer
            //var dal = new MongoDataAccessLayer(mongoClientSettings);

            // Mock ILogger<ApolloApi>
            // NOTE: This is still mocked because it doesnt change the test enviroment
            var loggerMock = new Mock<ILogger<ApolloApi>>();
            ILogger<ApolloApi> logger = loggerMock.Object;
        }

    }
}
