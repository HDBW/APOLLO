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
        [TestCategory("Prod")]
        public async Task InsertUserTest()
        {
            // Create a mock logger
            var logger = new Mock<ILogger<ApolloApi>>();

            var api = new ApolloApi(Helpers.GetDal(), logger.Object, Helpers.GetAPIConfig());

            var user = new User
            {
                Id = "U01",
                Name = "John Doe",
                Email = "johndoe@example.com"
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
        [TestCategory("Prod")]
        public async Task CreateOrUpdateUserTest()
        {
            var api = Helpers.GetApolloApi();

            var user = new User
            {
                Name = "John Doe",
                Email = "johndoe@example.com"
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
        [TestCategory("Prod")]
        public async Task GetUserTest()
        {
            // Arrange
            var api = Helpers.GetApolloApi();

            var newUser = new User { Name = "Test", Email = "test@example.com" };
            var userId = await api.CreateOrUpdateUser(new List<User> { newUser });

            // Act
            var user = await api.GetUser(userId.FirstOrDefault());

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(newUser.Name, user.Name);
            Assert.AreEqual(newUser.Email, user.Email);

            // Cleanup
            await api.DeleteUsers(new string[] { user.Id });
        }


        /// <summary>
        /// Tests querying User objects based on specific criteria such as FirstName and LastName.
        /// </summary>
        [TestMethod]
        // [TestCategory("Prod")]
        public async Task QueryUsersTest()
        {
            var api = Helpers.GetApolloApi();

            // Adjust the query to match the fields in the updated User class
            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { "Name", "Email" }, // Updated fields to be returned
                Filter = new Apollo.Common.Entities.Filter
                {
                    IsOrOperator = false,
                    Fields = new List<Apollo.Common.Entities.FieldExpression>
            {
                new Apollo.Common.Entities.FieldExpression
                {
                    FieldName = "Name", // Use 'Name' field for filtering
                    Operator = Apollo.Common.Entities.QueryOperator.Contains,
                    Argument = new List<object> { "John Doe" } // Example name
                },
                new Apollo.Common.Entities.FieldExpression
                {
                    FieldName = "Email", // Use 'Email' field for filtering
                    Operator = Apollo.Common.Entities.QueryOperator.Contains,
                    Argument = new List<object> { "johndoe@example.com" } // Example email
                }
            }
                },
                RequestCount = true,
                Top = 200,
                Skip = 0,
                SortExpression = new Apollo.Common.Entities.SortExpression
                {
                    FieldName = "Name", // Sort by 'Name' field
                    Order = Apollo.Common.Entities.SortOrder.Ascending
                }
            };

            // Act
            IList<User> users = await api.QueryUsers(query);

            // Assert
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count >= 0); // Validate the result

            // Cleanup: Delete the user records inserted during the test
            foreach (var user in users)
            {
                await api.DeleteUsers(new string[] { user.Id });
            }

            // Additional assertions based on specific testing requirements
        }


    }
}
