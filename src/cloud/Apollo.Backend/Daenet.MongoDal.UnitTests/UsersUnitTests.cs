using Apollo.Api;
using Apollo.Common.Entities;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Daenet.MongoDal.UnitTests
{
    /// <summary>
    /// Unit tests for user-related operations using MongoDB data access layer.
    /// </summary>
    [TestCategory("MongoDal")]
    [TestClass]
    public class UsersUnitTests
    {
        // Define an array of test users.
        User[] _testUsers = new User[]
        {
            new User { Id = "U01", UserName = "testuser1" },
            new User { Id = "U02", UserName = "testuser2" },
            new User { Id = "U03", UserName = "testuser3" }
            // Add more test users as needed
        };

        private readonly ApolloApi _api;
        private readonly ILogger<UsersUnitTests> _logger;

        /// <summary>
        /// Constructor for initializing the Apollo API and logger.
        /// </summary>
        public UsersUnitTests()
        {
            //var mongoDalConfig = new MongoDalConfig
            //{
            //    // Set MongoDalConfig properties if needed
            //};
            //var loggerFactory = new LoggerFactory();
            //var apolloApiConfig = new ApolloApiConfig
            //{
            //    // Set ApolloApiConfig properties here
            //};
            //_api = new ApolloApi(new MongoDataAccessLayer(mongoDalConfig), loggerFactory.CreateLogger<ApolloApi>(), apolloApiConfig);
            //_logger = loggerFactory.CreateLogger<UsersUnitTests>();

        }

        /// <summary>
        /// Cleanup method to remove test documents after each test.
        /// </summary>
        [TestCleanup]
        public async Task CleanupTest()
        {
            await CleanTestDocuments();
        }

        /// <summary>
        /// Initialization method to ensure a clean state before each test.
        /// </summary>
        [TestInitialize]
        public async Task InitTest()
        {
            await CleanTestDocuments();
        }

        /// <summary>
        /// Clean up test documents by deleting test users.
        /// </summary>
        private async Task CleanTestDocuments()
        {
            var dal = Helpers.GetDal();

            var userToDeleteIds = _testUsers.Select(u => u.Id).ToArray();
            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), userToDeleteIds, false);
        }

        /// <summary>
        /// Test method for inserting a user into the database.
        /// </summary>

        const string UserId = "U01";

        [TestMethod]
        public async Task InsertUserTest()
        {
            var dal = Helpers.GetDal();

            var user = new User
            {
                Id = UserId, // Use the constant instead of the hard-coded "U01".
                UserName = "testuser1",
                // Other properties
            };

            await dal.InsertAsync(Helpers.GetCollectionName<User>(), Convertor.Convert(user));

            var insertedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), user.Id);
            Assert.IsNotNull(insertedUser);
            Assert.AreEqual(user.UserName, insertedUser.UserName);

            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), user.Id);
        }

        /// <summary>
        /// Test method for updating a user's information in the database.
        /// </summary>
        [TestMethod]
        public async Task UpdateUserTest()
        {
            var dal = Helpers.GetDal();

            var updatedUserModel = new User
            {
                Id = UserId, // Use the constant "UserId".
                UserName = "updateduser",
                // Other updated properties
            };

            var filter = Builders<BsonDocument>.Filter.Eq("_id", updatedUserModel.Id);

            var updateDefinition = Builders<BsonDocument>.Update
                .Set("UserName", updatedUserModel.UserName);

            await dal.UpdateAsync(Helpers.GetCollectionName<User>(), filter, updateDefinition);

            var updatedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), updatedUserModel.Id);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(updatedUserModel.UserName, updatedUser.UserName);

            updatedUserModel.UserName = "testuser";
            await dal.UpdateAsync(Helpers.GetCollectionName<User>(), filter, updateDefinition);
        }

        /// <summary>
        /// Test method for deleting a user from the database.
        /// </summary>


        [TestMethod]
        public async Task DeleteUserTest()
        {
            var dal = Helpers.GetDal();

            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), UserId);

            var deletedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), UserId);
            Assert.IsNull(deletedUser);

            var user = new User
            {
                Id = UserId, 
                UserName = "testuser1",
               
            };
            await dal.InsertAsync(Helpers.GetCollectionName<User>(), Convertor.Convert(user));
        }


        /// <summary>
        /// Test method for querying users from the database.
        /// </summary>
        [TestMethod]
        public async Task QueryUsersTest()
        {
            var dal = Helpers.GetDal();

            var usersToInsert = _testUsers.ToList();
            await dal.InsertManyAsync(Helpers.GetCollectionName<User>(), usersToInsert.Select(u => Convertor.Convert(u)).ToArray());

            var query = new Daenet.MongoDal.Entitties.Query
            {
                Fields = new List<Daenet.MongoDal.Entitties.FieldExpression>
                {
                    new Daenet.MongoDal.Entitties.FieldExpression
                    {
                        FieldName = "UserName",
                        Operator = Daenet.MongoDal.Entitties.QueryOperator.Equals,
                        Argument = new List<object> { "testuser2" },
                    }
                }
            };

            var queryResult = await dal.ExecuteQuery(Helpers.GetCollectionName<User>(), null, query, 100, 0);

            Assert.IsNotNull(queryResult);
            Assert.AreEqual(1, queryResult.Count);

            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), usersToInsert.Select(u => u.Id).ToArray(), false);
        }
    }
}
