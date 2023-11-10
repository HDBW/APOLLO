// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Apollo.Service.Controllers;
using Daenet.MongoDal.Entitties;
using Daenet.MongoDal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apollo.RestService.RestService.UnitTests
{
    /// <summary>
    /// Unit tests for the TrainingController class.
    /// </summary>
    [TestClass]
    public class TrainingControllerTests
    {
        private TrainingController _controller;
        private ILogger<TrainingController> _logger;
        private ApolloApi _api;

        /// <summary>
        /// Set up the test environment by creating instances of required dependencies and initializing the controller.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create instances of required dependencies
            var dal = new MongoDataAccessLayer(new MongoDalConfig { /* configure  */ });

            // Create an ILogger<ApolloApi> instance for the ApolloApi class
            var apiLogger = new LoggerFactory().CreateLogger<ApolloApi>();

            _logger = new LoggerFactory().CreateLogger<TrainingController>();

            // Pass the dependencies to the ApolloApi constructor
            _api = new ApolloApi(dal, apiLogger);

            // Initialize the TrainingController with the ApolloApi and logger
            _controller = new TrainingController(_api, _logger);
        }

        /// <summary>
        /// Test method to verify the GetTraining action of TrainingController with a valid training ID.
        /// </summary>
        [TestMethod]
        public async Task GetTraining_ValidId_ReturnsTraining()
        {
            
            string trainingId = "someTrainingId";
            var expectedTraining = new Training { Id = trainingId, TrainingName = "TestTraining" };

            
            var result = await _controller.GetTraining(trainingId);

            
            Assert.IsInstanceOfType(result, typeof(GetTrainingResponse));
            Assert.AreEqual(expectedTraining, result.Training);
        }

        // Add more test methods for other TrainingController actions
    }
}
