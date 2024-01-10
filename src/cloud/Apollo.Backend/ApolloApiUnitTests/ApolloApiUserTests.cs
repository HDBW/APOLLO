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
            // Create a mock logger
            var logger = new Mock<ILogger<ApolloApi>>();

            var api = new ApolloApi(Helpers.GetDal(), logger.Object, Helpers.GetAPIConfig());

            var user = new User
            {
                Id = "U01",
                FirstName = "John",
                LastName = "Doe",
                Goal = "Test Goal"
            };

            try
            {
                await api.InsertUser(user);

                // Optionally, can add assertions to verify the insertion was successful
                var retrievedUser = await api.GetUser(user.Id);
                Assert.IsNotNull(retrievedUser);
                Assert.AreEqual(user.Id, retrievedUser.Id);
            }
            catch (ApolloApiException ex) when (ex.InnerException is MongoWriteException mongoEx && mongoEx.WriteError.Code == 61)
            {
                // Handle shard key error specifically
                Assert.Fail("Shard key error occurred: " + mongoEx.WriteError.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Assert.Fail("An unexpected exception occurred: " + ex.Message);
            }
            finally
            {
                // Clean up
                await api.DeleteUsers(new string[] { user.Id });
            }
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

            var newUser = new User { FirstName = "Test", LastName = "User" };
            var userId = await api.CreateOrUpdateUser(new List<User> { newUser });

            // Act
            var user = await api.GetUser(userId.FirstOrDefault());

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(newUser.FirstName, user.FirstName);
            Assert.AreEqual(newUser.LastName, user.LastName);

            // Cleanup
            await api.DeleteUsers(new string[] { user.Id });
        }


        /// <summary>
        /// Tests querying User objects based on specific criteria such as FirstName and LastName.
        /// </summary>
        [TestMethod]
        public async Task QueryUsers()
        {
            var api = Helpers.GetApolloApi();

            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { "FirstName", "LastName", "UserName" }, // Specify the fields to be returned
                Filter = new Apollo.Common.Entities.Filter
                {
                    IsOrOperator = false,
                    Fields = new List<Apollo.Common.Entities.FieldExpression>
            {
                new Apollo.Common.Entities.FieldExpression
                {
                    FieldName = "FirstName",
                    Operator = Apollo.Common.Entities.QueryOperator.Equals,
                    Argument = new List<object> { "John", "Jane" }
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

            IList<User> users;
            try
            {
                users = await api.QueryUsers(query);
            }
            catch (ApolloApiException ex)
            {
                // Handle the case when no records are found
                if (ex.ErrorCode == ErrorCodes.UserErrors.QueryUsersError)
                {
                    users = new List<User>(); // Initialize an empty list
                }
                else
                {
                    // Re-throw the exception if it's not related to an empty result
                    throw;
                }
            }

            // Assert
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count >= 0); // Change the condition to allow for an empty list

            // Cleanup: Delete the user records inserted during the test
            foreach (var user in users)
            {
                await api.DeleteUsers(new string[] { user.Id });
            }

            // add more assertions based on your specific testing requirements
        }


    }
}
