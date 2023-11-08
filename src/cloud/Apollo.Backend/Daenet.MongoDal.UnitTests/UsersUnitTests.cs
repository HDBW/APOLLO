using Apollo.Api;
using Apollo.Common.Entities;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Daenet.MongoDal.UnitTests
{
    /// <summary>
    /// This class contains unit tests for managing users using a MongoDB data access layer.
    /// </summary>
    [TestCategory("MongoDal")]
    [TestClass]
    public class UsersUnitTests
    {
        private readonly ApolloApi _api;
        private readonly ILogger<UsersUnitTests> _logger;

        public UsersUnitTests()
        {
            // Initialize ApolloApi and logger.
            _api = new ApolloApi(new MongoDataAccessLayer(new MongoDalConfig()), new LoggerFactory().CreateLogger<ApolloApi>());
            _logger = new LoggerFactory().CreateLogger<UsersUnitTests>();
        }

        [TestCleanup]
        /// <summary>
        /// Cleans up test documents after a test method execution.
        /// </summary>
        public async Task CleanupTest()
        {
            await CleanTestDocuments();
        }

        [TestInitialize]
        /// <summary>
        /// Initializes test environment by cleaning up test documents.
        /// </summary>
        public async Task InitTest()
        {
            await CleanTestDocuments();
        }

        private async Task CleanTestDocuments()
        {
            var dal = Helpers.GetDal();

            // Specify the user IDs you want to delete.
            var userToDeleteIds = new string[] { "U01" };

            // Delete the users with the specified IDs.
            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), userToDeleteIds, false);
        }

        [TestMethod]
        /// <summary>
        /// Unit test for inserting a user into the MongoDB database.
        /// </summary>
        public async Task InsertUserTest()
        {
            var dal = Helpers.GetDal();

            var user = new User
            {
                Id = "U01",
                UserName = "testuser",
                // Other properties
            };

            // Insert the user.
            await dal.InsertAsync(Helpers.GetCollectionName<User>(), Convertor.Convert(user));

            // Verify that the user has been inserted successfully.
            var insertedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), user.Id);
            Assert.IsNotNull(insertedUser);
            Assert.AreEqual(user.UserName, insertedUser.UserName);

            // Clean up - delete the inserted user.
            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), user.Id);
        }

        [TestMethod]
        /// <summary>
        /// Unit test for updating a user in the MongoDB database.
        /// </summary>
        public async Task UpdateUserTest()
        {
            var dal = Helpers.GetDal();

            // Create a user to update.
            var updatedUserModel = new User
            {
                Id = "U01",
                UserName = "updateduser",
                // Other updated properties
            };

            // Define the filter to identify the user by ID.
            var filter = Builders<BsonDocument>.Filter.Eq("_id", updatedUserModel.Id);

            // Create an update definition to specify the changes.
            var updateDefinition = Builders<BsonDocument>.Update
                .Set("UserName", updatedUserModel.UserName);
            // Add other update operations for additional properties as needed

            // Update the user.
            await dal.UpdateAsync(Helpers.GetCollectionName<User>(), filter, updateDefinition);

            // Verify that the user has been updated successfully.
            var updatedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), updatedUserModel.Id);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(updatedUserModel.UserName, updatedUser.UserName);

            // Clean up - restore the user to its previous state.
            updatedUserModel.UserName = "testuser";
            await dal.UpdateAsync(Helpers.GetCollectionName<User>(), filter, updateDefinition);
        }

        [TestMethod]
        /// <summary>
        /// Unit test for deleting a user from the MongoDB database.
        /// </summary>
        public async Task DeleteUserTest()
        {
            var dal = Helpers.GetDal();

            var userId = "U01";

            // Delete the user.
            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), userId);

            // Verify that the user has been deleted successfully.
            var deletedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), userId);
            Assert.IsNull(deletedUser);

            // Clean up - insert the user back.
            var user = new User
            {
                Id = "U01",
                UserName = "testuser",
                // Other properties
            };
            await dal.InsertAsync(Helpers.GetCollectionName<User>(), Convertor.Convert(user));
        }

        [TestMethod]
        /// <summary>
        /// Unit test for querying users from the MongoDB database.
        /// </summary>
        public async Task QueryUsersTest()
        {
            var dal = Helpers.GetDal();

            // Insert some test users.
            var usersToInsert = new List<User>
            {
                new User { Id = "U01", UserName = "user1" },
                new User { Id = "U02", UserName = "user2" },
                new User { Id = "U03", UserName = "user3" },
            };
            await dal.InsertManyAsync(Helpers.GetCollectionName<User>(), usersToInsert.Select(u => Convertor.Convert(u)).ToArray());

            // Define a query to retrieve users based on certain criteria.
            var query = new Query
            {
                Fields = new List<FieldExpression>
                {
                    new FieldExpression
                    {
                        FieldName = "UserName",
                        Operator = QueryOperator.Equals,
                        Argument = new List<object> { "user2" },
                    }
                }
            };

            // Execute the query.
            var queryResult = await dal.ExecuteQuery(Helpers.GetCollectionName<User>(), null, query, 100, 0);

            // Verify the results match the expected criteria.
            Assert.IsNotNull(queryResult);
            Assert.AreEqual(1, queryResult.Count);

            // Clean up - delete the test users.
            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), usersToInsert.Select(u => u.Id).ToArray(), false);
        }
    }
}
