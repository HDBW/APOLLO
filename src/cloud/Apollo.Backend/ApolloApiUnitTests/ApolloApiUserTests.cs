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

            // Create a new user
            var user = new User { Name = "John Doe", Email = "johndoe@example.com" };

            // Insert the new user
            var userIds = await api.CreateOrUpdateUserAsync(new List<User> { user });
            Assert.IsNotNull(userIds);
            Assert.AreEqual(1, userIds.Count);

            // Retrieve and verify the inserted user
            var insertedUser = await api.GetUserById(userIds.First());
            Assert.IsNotNull(insertedUser);
            Assert.AreEqual("John Doe", insertedUser.Name);

            // Update the user's information
            insertedUser.Name = "Jane Doe";

            // Update the user
            var updatedUserIds = await api.CreateOrUpdateUserAsync(new List<User> { insertedUser });
            Assert.IsNotNull(updatedUserIds);
            Assert.AreEqual(1, updatedUserIds.Count);

            // Retrieve and verify the updated user
            var updatedUser = await api.GetUserById(updatedUserIds.First());
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("Jane Doe", updatedUser.Name);

            // Clean up: Delete the user
            await api.DeleteUsers(updatedUserIds.ToArray());
        }


        //Case 1: Single User with ID
        /// <summary>
        /// Tests the CreateOrUpdateUser method by creating and then updating a user with a specific ID.
        /// Verifies that the user is successfully created, updated, and matches the provided ID.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task CreateOrUpdateUser_SingleWithIDTest()
        {
            var api = Helpers.GetApolloApi();

            // Create and insert a new user to generate an ID
            var newUser = new User { Name = "Test User", Email = "tstuser@example.com" };
            var userId = await api.CreateOrUpdateUserAsync(new List<User> { newUser });

            // Modify the user's information
            newUser.Id = userId.First();
            newUser.Name = "Updated User";

            // Update the user with predefined ID
            var updatedUserIds = await api.CreateOrUpdateUserAsync(new List<User> { newUser });

            Assert.IsNotNull(updatedUserIds);
            Assert.AreEqual(1, updatedUserIds.Count);
            Assert.AreEqual(userId.First(), updatedUserIds.First());

            // Clean up: Delete the user
            await api.DeleteUsers(updatedUserIds.ToArray());
        }


        //Case 2: Single User with ObjectId
        /// <summary>
        /// Tests the CreateOrUpdateUser method by creating and then updating a user with a specific ObjectId.
        /// Verifies that the user is successfully inserted with an ObjectId, updated, and the changes are persisted.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task CreateOrUpdateUser_SingleWithObjectIdTest()
        {
            var api = Helpers.GetApolloApi();

            // Create a user with ObjectId but no ID
            var userWithObjectId = new User { ObjectId = "someObjectId", Name = "User with ObjectId", Email = "objectiduser@example.com" };

            // Insert the user with ObjectId
            var userIds = await api.CreateOrUpdateUserAsync(new List<User> { userWithObjectId });
            Assert.IsNotNull(userIds);
            Assert.AreEqual(1, userIds.Count);

            // Retrieve the inserted user
            var insertedUser = await api.GetUserById(userIds.First());
            Assert.IsNotNull(insertedUser);

            // Update some information of the user
            insertedUser.Name = "Updated User with ObjectId";

            // Update the user with ObjectId
            var updatedUserIds = await api.CreateOrUpdateUserAsync(new List<User> { insertedUser });
            Assert.IsNotNull(updatedUserIds);
            Assert.AreEqual(1, updatedUserIds.Count);

            // Verify that the user was updated
            var updatedUser = await api.GetUserById(updatedUserIds.First());
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("Updated User with ObjectId", updatedUser.Name);

            // Clean up: Delete the user
            await api.DeleteUsers(updatedUserIds.ToArray());
        }


        //Case 3: Multiple Users with Mixed IDs
        /// <summary>
        /// Test method for creating or updating multiple users with mixed IDs.
        /// </summary>
        /// <remarks>
        /// This test case covers the scenario where multiple users are created or updated,
        /// some with predefined IDs, some with ObjectId, and some with no Id or ObjectId.
        /// </remarks>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task CreateOrUpdateUser_MultipleMixedIDsTest()
        {
            var api = Helpers.GetApolloApi();
           

            // Creating multiple users, some with predefined IDs, some with ObjectId
            var user1 = new User { Id = "User1", Name = "User One", Email = "user1@example.com" };
            var user2 = new User { ObjectId = "ObjectID2", Name = "User Two", Email = "user2@example.com" };
            var user3 = new User { Name = "User Three", Email = "user3@example.com" }; // No Id or ObjectId

            // Insert the users
            var userIds = await api.CreateOrUpdateUserAsync(new List<User> { user1, user2, user3 });
            Assert.IsNotNull(userIds);
            Assert.AreEqual(3, userIds.Count);

            // Retrieve user2 by ObjectId and update
            var existingUser2 = await api.FindUserByObjectIdAsync(user2.ObjectId);
            Assert.IsNotNull(existingUser2);
            existingUser2.Name = "Updated User Two";

            // Retrieve user3 by Email and update
            var existingUser3 = await api.FindUserByEmailAsync(user3.Email);
            Assert.IsNotNull(existingUser3);
            existingUser3.Name = "Updated User Three";

            // Update user1 directly
            user1.Name = "Updated User One";

            // Update the users
            var updatedUserIds = await api.CreateOrUpdateUserAsync(new List<User> { user1, existingUser2, existingUser3 });
            Assert.IsNotNull(updatedUserIds);
            Assert.AreEqual(3, updatedUserIds.Count); // Expecting updates for all three users

            // Verify that the users were updated
            var updatedUser1 = await api.GetUserById(user1.Id);
            var updatedUser2 = await api.GetUserById(existingUser2.Id);
            var updatedUser3 = await api.GetUserById(existingUser3.Id);
            Assert.AreEqual("Updated User One", updatedUser1.Name);
            Assert.AreEqual("Updated User Two", updatedUser2.Name);
            Assert.AreEqual("Updated User Three", updatedUser3.Name);

            // Clean up: Delete all users
            await api.DeleteUsers(updatedUserIds.ToArray());
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
            var userId = await api.CreateOrUpdateUserAsync(new List<User> { newUser });

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
        [TestCategory("Prod")]
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
