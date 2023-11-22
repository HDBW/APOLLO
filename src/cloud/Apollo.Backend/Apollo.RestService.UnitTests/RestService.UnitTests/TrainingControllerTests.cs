// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Apollo.Service.Controllers;
using Daenet.MongoDal.Entitties;
using Daenet.MongoDal;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

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
            var dal = new MongoDataAccessLayer(new MongoDalConfig { /* configure */ });

            // Create an ILogger<ApolloApi> instance for the ApolloApi class
            var apiLogger = new LoggerFactory().CreateLogger<ApolloApi>();

            // Define and initialize the ApolloApiConfig
            var config = new ApolloApiConfig
            {
                // Set the necessary configuration properties here
            };

            _logger = new LoggerFactory().CreateLogger<TrainingController>();

            // Pass the dependencies to the ApolloApi constructor
            _api = new ApolloApi(dal, apiLogger, config);

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

        [TestMethod]
        public async Task GetTraining_InvalidId_ReturnsNull()
        {
            string trainingId = "invalidTrainingId";

            var result = await _controller.GetTraining(trainingId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GetTrainingResponse));
            Assert.IsNotNull(result.Training);
            Assert.AreEqual(trainingId, result.Training.Id);
        }

        [TestMethod]
        public async Task QueryTrainings_ValidRequest_ReturnsListOfTrainings()
        {
            var queryRequest = new QueryTrainingsRequest { /* Define query criteria */ };

            var result = await _controller.QueryTrainings(queryRequest);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<QueryTrainingsResponse>));
            // Add more assertions based on the expected response
        }

        [TestMethod]
        public async Task CreateOrUpdateTraining_ValidRequest_ReturnsCreatedTraining()
        {
            var createOrUpdateRequest = new CreateOrUpdateTrainingRequest { /* Define Training object */ };

            var result = await _controller.CreateOrUpdateTraining(createOrUpdateRequest);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreateOrUpdateTrainingResponse));
            // Add more assertions based on the expected response
        }

        [TestMethod]
        public async Task InsertTrainings_ValidTrainings_ReturnsResult()
        {
            var trainings = new List<Training> { /* Define Training objects */ };

            var result = await _controller.InsertTrainings(trainings);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            // Add more assertions based on the expected response
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsNoContent()
        {
            string trainingId = "someValidTrainingId";

            await _controller.Delete(trainingId);

            // Add assertions based on the expected behavior after deletion
        }
        // Add more test methods for other TrainingController actions
    }
}
