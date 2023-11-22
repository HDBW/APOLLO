// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Apollo.Service.Controllers;
using Daenet.MongoDal.Entitties;
using Daenet.MongoDal;
using Microsoft.Extensions.Logging;
using Apollo.RestService.Apollo.Common.Messages;
using Microsoft.AspNetCore.Mvc;
using Amazon.Auth.AccessControlPolicy;

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


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GetUserResponse));
            Assert.IsNotNull(result.User);
            Assert.AreEqual(userId, result.User.Id);
        }

        [TestMethod]
        public async Task QueryUsers_ValidRequest_ReturnsListOfUsers()
        {
            // Define the query criteria
            var queryRequest = new QueryUsersRequest
            {
                Filter = new Filter
                {
                    Fields = new List<Common.Entities.FieldExpression>
            {
                new Common.Entities.FieldExpression
                {
                    FieldName = "UserName", // Adjust to your entity's property
                    Operator = Common.Entities.QueryOperator.Equals,
                    Argument = new List<object> { "TestUser" } // Define the value to match
                }
                
            }
                }
            };

            var result = await _controller.QueryUsers(queryRequest);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(QueryUsersResponse));
            // Add more assertions based on the expected response
        }

        [TestMethod]
        public async Task CreateOrUpdateUser_ValidRequest_ReturnsCreatedUser()
        {
            // Create a valid CreateOrUpdateUserRequest
            var createOrUpdateRequest = new CreateOrUpdateUserRequest { /* Define User object */ };

            var result = await _controller.CreateOrUpdateUser(createOrUpdateRequest);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreateOrUpdateUserResponse));
            // Add more assertions based on the expected response
        }

        [TestMethod]
        public async Task InsertUsers_ValidUsers_ReturnsOkResult()
        {
            // Create a valid collection of Users
            var users = new List<User> { /* Define User objects */ };

            var result = await _controller.InsertUsers(users);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            // Add more assertions based on the expected response
        }

        [TestMethod]
        public async Task DeleteUser_ValidId_ReturnsNoContent()
        {
            int[] userIds = new int[] { /* Define valid user IDs */ };

            await _controller.DeleteUser(userIds);

            // Add assertions based on the expected behavior after deletion
        }

        // Add more test methods for other UserController actions
    }
}
