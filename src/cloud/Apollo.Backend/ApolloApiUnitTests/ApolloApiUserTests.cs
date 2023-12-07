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
        [TestMethod]
        public async Task InsertUser()
        {
            // Arrange
            var api = Helpers.GetApolloApi();

            var user = new User
            {
                Id = "U01", // Provide a valid user ID here
                UserName = "TestUser",
                Goal = "Some Goal",
                FirstName = "John",
                LastName = "Doe",
                Image = "user1.png"
            };

            string userId = null; // Initialize userId outside the try block

            try
            {
                // Act
                userId = await api.InsertUser(user);

                // Assert
                Assert.IsNotNull(userId);
                // Update the assertion to check if userId is not null
                Assert.IsNotNull(userId);

            }
            finally
            {
                // Clean up (optional): Delete the user after testing
                if (!string.IsNullOrEmpty(userId))
                {
                    await api.DeleteUser(new string[] { userId }); // Pass the user ID as a string array
                }
            }
        }
      }
}
