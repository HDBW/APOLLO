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

namespace Apollo.RestService.RestService.UnitTests
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

        [TestInitialize]
        public void Init()
        {
            _mockApi = new Mock<ApolloApi>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_mockApi.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetUser_ReturnsUser()
        {
            // Arrange
            var userId = "someId";
            var expectedUser = new User { Id = userId, FirstName = "John", LastName = "Doe" };
            _mockApi.Setup(api => api.GetUser(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.GetUser(userId) as GetUserResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.FirstName, result.User.FirstName);
        }

        [TestMethod]
        public async Task QueryUsers_ReturnsListOfUsers()
        {
            // Arrange
            var request = new QueryUsersRequest();
            var expectedUsers = new List<User>
            {
                new User { FirstName = "Alice" },
                new User { FirstName = "Bob" }
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

        //TODO:
        //[TestMethod]
        //public async Task CreateOrUpdateUser_CreatesUserSuccessfully()
        //{
        //    // Arrange
        //    var newUser = new User { FirstName = "Charlie" };
        //    var expectedResult = "newId";

        //    _mockApi.Setup(api => api.CreateOrUpdateUser(It.Is<List<User>>(users => users.Contains(newUser)))).ReturnsAsync(new List<string> { expectedResult });

        //    // Act
        //    var result = await _controller.CreateOrUpdateUser(newUser) as CreateOrUpdateUserResponse;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(expectedResult, result.Result);
        //}



        [TestMethod]
        public async Task InsertUsers_InsertsUsersSuccessfully()
        {
            // Arrange
            var usersToInsert = new List<User> { new User { FirstName = "Dave" } };
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
