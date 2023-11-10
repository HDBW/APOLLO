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
    /// Unit tests for the UserController class.
    /// </summary>
    [TestClass]
    public class UserControllerTests
    {
        private UserController _controller;
        private ILogger<UserController> _logger;
        private ApolloApi _api;

        /// <summary>
        /// Set up the test environment by creating instances of required dependencies and initializing the controller.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create instances of required dependencies, including configuring the MongoDataAccessLayer.
            var dal = new MongoDataAccessLayer(new MongoDalConfig
            {
                // Configure the connection string and database name for the MongoDB.
                MongoConnStr = "your_connection_string",
                MongoDatabase = "your_database"
            });

            // Create an ILogger<ApolloApi> instance for the ApolloApi class.
            var apiLogger = new LoggerFactory().CreateLogger<ApolloApi>();

            _logger = new LoggerFactory().CreateLogger<UserController>();

            // Pass the dependencies to the ApolloApi constructor.
            _api = new ApolloApi(dal, apiLogger);

            // Initialize the UserController with the ApolloApi and logger.
            _controller = new UserController(_api, _logger);
        }

        /// <summary>
        /// Test method to verify the GetUser action of UserController with a valid user ID.
        /// </summary>
        [TestMethod]
        public async Task GetUser_ValidId_ReturnsUser()
        {
           
            string userId = "someId";
            var expectedUser = new User { Id = userId, UserName = "TestUser" };

            
            var result = await _controller.GetUser(userId);

           
            Assert.IsInstanceOfType(result, typeof(GetUserResponse));
            Assert.AreEqual(expectedUser, result.User);
        }

        // Add more test methods for other UserController actions
    }
}
