// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Apollo.Service.Controllers;
using Daenet.MongoDal.Entitties;
using Daenet.MongoDal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;

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
            // Assuming you have a class ApolloApiConfig
            // Initialize your ApolloApiConfig here
            var apiConfig = new ApolloApiConfig
            {
                // Set the necessary configuration properties
            };

            // Initialize the MongoDataAccessLayer
            var dal = new MongoDataAccessLayer(new MongoDalConfig
            {
                MongoConnStr = "your_connection_string",
                MongoDatabase = "your_database"
            });

            // Initialize the ILogger for ApolloApi
            var apiLogger = new LoggerFactory().CreateLogger<ApolloApi>();

            // Initialize the ILogger for UserController
            _logger = new LoggerFactory().CreateLogger<UserController>();

            // Create an instance of ApolloApi with the necessary dependencies
            _api = new ApolloApi(dal, apiLogger, apiConfig);

            // Create an instance of UserController
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
