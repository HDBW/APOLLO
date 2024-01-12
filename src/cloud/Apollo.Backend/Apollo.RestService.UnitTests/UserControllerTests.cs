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
using Moq;
using System.Reflection;

namespace Apollo.RestService.UnitTests
{
    /// <summary>
    /// Unit tests for the UserController class.
    /// </summary>
    [TestClass]
    public class UserControllerTests
    {

        private UserController _controller;
        private Mock<ApolloApi> _mockApi;
        private Mock<ILogger<UserController>> _mockLogger;


        /// <summary>
        /// Initializes the test environment before each test method runs.
        /// This setup includes creating mock instances for ApolloApi and ILogger and initializing the UserController with these mocks.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _mockApi = new Mock<ApolloApi>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_mockApi.Object, _mockLogger.Object);
        }


        /// <summary>
        /// Tests the GetUser method in UserController to ensure it returns the correct User response.
        /// </summary>
        [TestMethod]
        public async Task GetUser_ReturnsUser()
        {
            // Arrange
            var userId = "someId";
            var expectedUser = new User { Id = userId, Name = "John Doe" };
            _mockApi.Setup(api => api.GetUser(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.GetUser(userId) as GetUserResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.User);
            Assert.AreEqual(expectedUser.Id, result.User.Id);
            Assert.AreEqual(expectedUser.Name, result.User.Name);
        }


        /// <summary>
        /// Tests the QueryUsers method in UserController to ensure it returns a list of User objects.
        /// </summary>
        [TestMethod]
        public async Task QueryUsers_ReturnsListOfUsers()
        {
            // Arrange
            var request = new QueryUsersRequest();
            var expectedUsers = new List<User>
             {
                new User { Name = "Alice", Id = "1", ObjectId = "OID1", Upn = "alice@example.com", Email = "alice@example.com", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1990, 1, 1), Disabilities = false, Profile = new Profile() },
                new User { Name = "Bob", Id = "2", ObjectId = "OID2", Upn = "bob@example.com", Email = "bob@example.com", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1992, 2, 2), Disabilities = false, Profile = new Profile() }
             };

            _mockApi.Setup(api => api.QueryUsers(It.IsAny<QueryUsersRequest>())).ReturnsAsync(expectedUsers);

            // Act
            var actionResult = await _controller.QueryUsers(request);

            // Assert
            Assert.IsNotNull(actionResult, "The action result should not be null.");
            var result = actionResult as QueryUsersResponse;
            Assert.IsNotNull(result, "The result should be of type QueryUsersResponse.");
            Assert.IsNotNull(result.Users, "The Users property should not be null.");
            Assert.AreEqual(2, result.Users.Count, "The Users count should be 2.");
        }


        /// <summary>
        /// Tests the CreateOrUpdateUser method in UserController to ensure it creates or updates User objects successfully.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateUser_CreatesUsersSuccessfully()
        {
            // Arrange
            var newUser = new User { Name = "NewUser" };
            var request = new CreateOrUpdateUserRequest(newUser);

            // Mock the behavior of the CreateOrUpdateUser method of ApolloApi
            _mockApi.Setup(api => api.CreateOrUpdateUser(It.IsAny<ICollection<User>>()))
                    .ReturnsAsync(new List<string> { "newUserId" }); // Return a list with a single ID

            // Act
            var response = await _controller.CreateOrUpdateUser(request) as CreateOrUpdateUserResponse;

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result); // Verify the Result property is not null
                                               // Additional assertions can be made based on the actual structure of Result
        }


        /// <summary>
        /// Tests the InsertUsers method in UserController to ensure it inserts User objects successfully.
        /// </summary>
        [TestMethod]
        public async Task InsertUsers_InsertsUsersSuccessfully()
        {
            // Arrange
            var usersToInsert = new List<User> { new User { Name = "Dave", ObjectId = "ObjectID1", Upn = "upn1@example.com", Email = "dave@example.com", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1980, 1, 1), Disabilities = false, Profile = new Profile() } };
            _mockApi.Setup(api => api.InsertUser(It.IsAny<User>()))
                    .ReturnsAsync(() => Guid.NewGuid().ToString());

            // Act
            var userIds = await _controller.InsertUsers(usersToInsert);

            // Assert
            Assert.IsNotNull(userIds);
            Assert.AreEqual(1, userIds.Count); // Expecting 1 user ID in the list
            Assert.IsFalse(string.IsNullOrEmpty(userIds.First())); // Check if the ID is not null or empty
            Assert.IsTrue(Guid.TryParse(userIds.First(), out _)); // Check if the ID is in a valid GUID format
        }


        /// <summary>
        /// Tests the DeleteUser method in UserController to ensure it successfully removes specified User objects.
        /// </summary>
        [TestMethod]
        public async Task DeleteUser_RemovesUserSuccessfully()
        {
            // Arrange
            var userIds = new string[] { "1" };

            // Act
            await _controller.DeleteUser(userIds);

            // Assert
            _mockApi.Verify(api => api.DeleteUsers(userIds), Times.Once());
        }


        /// <summary>
        /// Cleans up the test environment after each test method has run.
        /// This includes resetting the mock objects for ApolloApi and ILogger.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Perform any necessary cleanup after the tests have run

            // Clearing mocks
            _mockApi.Reset();
            _mockLogger.Reset();


        }

    }
}
