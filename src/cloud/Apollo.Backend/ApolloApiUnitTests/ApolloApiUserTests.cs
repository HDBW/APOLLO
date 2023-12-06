// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using Moq;

namespace Apollo.Api.UnitTests
{
    [TestClass]
    public class ApolloApiUsersUnitTests
    {
        private Mock<MongoDataAccessLayer> ?_mockDal;
        private ApolloApi ?_apolloApi;
        private Mock<ILogger<ApolloApi>> ?_mockLogger;
        private ApolloApiConfig ?_config;


        [TestInitialize]
        public void Initialize()
        {
           
            // Setup mock for MongoDataAccessLayer
            var mockDalConfig = new Mock<MongoDalConfig>();
            var mockLoggerDal = new Mock<ILogger<MongoDataAccessLayer>>();
            _mockDal = new Mock<MongoDataAccessLayer>(mockDalConfig.Object, mockLoggerDal.Object);

            // Setup mock for ILogger<ApolloApi>
            _mockLogger = new Mock<ILogger<ApolloApi>>();

            // Initialize ApolloApi with mocks
            _apolloApi = new ApolloApi(_mockDal.Object, _mockLogger.Object, _config);
        }


        [TestMethod]
        public async Task GetUser_ShouldReturnUser_WhenUserIdIsValid()
        {
            // Arrange
            var expectedUser = new User
            {
                Id = "userId1",
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Goal = "Achieve something",
                Image = "user1.png"
            };

            _mockDal.Setup(dal => dal.GetByIdAsync<User>(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(expectedUser);

            // Act
            var result = await _apolloApi.GetUser("userId1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.Id, result.Id);
            Assert.AreEqual(expectedUser.UserName, result.UserName);
            Assert.AreEqual(expectedUser.FirstName, result.FirstName);
            Assert.AreEqual(expectedUser.LastName, result.LastName);
            Assert.AreEqual(expectedUser.Goal, result.Goal);
            Assert.AreEqual(expectedUser.Image, result.Image);
            // Further assertions...
        }


        // Additional test methods for QueryUsers, CreateOrUpdateUser, DeleteUser, etc.
    }

}
