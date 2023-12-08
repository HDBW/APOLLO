// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Apollo.Api.UnitTests
{

    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiUsersUnitTests
    {

        /// <summary>
        /// Tests the insertion of a User object and its retrieval from the database.
        /// </summary>
        [TestMethod]
        public async Task InsertUser()
        {
            var api = Helpers.GetApolloApi();

            var user = new User
            {
                Id = "U01",
                FirstName = "John",
                LastName = "Doe",
                Goal = "Test Goal"
            };

            await api.InsertUser(user);

            // Optionally, can add assertions to verify the insertion was successful
            var retrievedUser = await api.GetUser(user.Id);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(user.Id, retrievedUser.Id);

            await api.DeleteUsers(new string[] { user.Id });
        }


        /// <summary>
        /// Tests creating or updating a User object and then cleaning up by deleting it.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateUser()
        {
            var api = Helpers.GetApolloApi();

            var user = new User
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var userIds = await api.CreateOrUpdateUser(new List<User> { user });

            // Ensure that the user was created or updated and has a valid ID
            Assert.IsNotNull(userIds);
            Assert.IsTrue(userIds.Count > 0);

            // Clean up: Delete the created or updated user
            await api.DeleteUsers(userIds.ToArray());
        }


        /// <summary>
        /// Tests retrieving a specific User object by its ID.
        /// </summary>
        [TestMethod]
        public async Task GetUser()
        {
            // Arrange
            var api = Helpers.GetApolloApi();

            // Assuming there is a user with Id "U01" in the database
            string userId = "U01";

            // Act
            var user = await api.GetUser(userId);

            // Assert
            Assert.IsNotNull(user);
            // You can add custom assertions based on your requirements
        }


        /// <summary>
        /// Tests querying User objects based on specific criteria such as FirstName and LastName.
        /// </summary>
        [TestMethod]
        public async Task QueryUsers()
        {
            // Arrange
            var api = Helpers.GetApolloApi();

            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { "FirstName", "LastName", "UserName" },
                Filter = new Apollo.Common.Entities.Filter
                {
                    Fields = new List<Apollo.Common.Entities.FieldExpression>
            {
                new Apollo.Common.Entities.FieldExpression
                {
                    FieldName = "FirstName",
                    Operator = Apollo.Common.Entities.QueryOperator.Equals,
                    Argument = new List<object> { "John" }
                },
                new Apollo.Common.Entities.FieldExpression
                {
                    FieldName = "LastName",
                    Operator = Apollo.Common.Entities.QueryOperator.Equals,
                    Argument = new List<object> { "Doe" }
                }
            }
                },
                RequestCount = true,
                Top = 200,
                Skip = 0,
                SortExpression = new Apollo.Common.Entities.SortExpression
                {
                    FieldName = "LastName",
                    Order = Apollo.Common.Entities.SortOrder.Ascending
                }
            };

            // Act
            var users = await api.QueryUsers(query);

            // Assert
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count >= 0);

            // Cleanup
            foreach (var user in users)
            {
                await api.DeleteUsers(new string[] { user.Id });
            }
        }
    }
}
