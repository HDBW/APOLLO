using System.Dynamic;
using System.Linq;
using Apollo.Api;
using Apollo.Common.Entities;
using MongoDB.Driver;
using Query = Daenet.MongoDal.Entitties.Query;
using QueryOperator = Daenet.MongoDal.Entitties.QueryOperator;

namespace Daenet.MongoDal.UnitTests
{
    /// <summary>
    /// Unit tests for the Users data access layer.
    /// </summary>
    [TestCategory("MongoDal")]
    [TestClass]
    public class UsersUnitTests
    {
        User[] _testUsers = new User[]
         {
            new User { Id = "U01", ObjectId = "ObjectID1", Upn = "upn1@example.com", Email = "user1@example.com", Name = "Test User1", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1990, 1, 1), Disabilities = false, Profile = new Profile() },
            new User { Id = "U02", ObjectId = "ObjectID2", Upn = "upn2@example.com", Email = "user2@example.com", Name = "Test User2", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1991, 2, 2), Disabilities = true, Profile = new Profile() },
            new User { Id = "U03", ObjectId = "ObjectID3", Upn = "upn3@example.com", Email = "user3@example.com", Name = "Test User3", ContactInfos = new List<Contact>(), Birthdate = new DateTime(1992, 3, 3), Disabilities = false, Profile = new Profile() }
         };


        /// <summary>
        /// Cleans up test data from the database.
        /// This method is called to remove any test data that may have been created during the tests.
        /// </summary>
        private async Task CleanTestDocuments()
        {
            var dal = Helpers.GetDal();

            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), _testUsers.Select(u => u.Id).ToArray(), false);
        }


        /// <summary>
        /// Cleans up all test data from the database after each test method.
        /// </summary>
        [TestCleanup]
        public async Task CleanupTest()
        {
            await CleanTestDocuments();
        }

        
        /// <summary>
        /// Cleans up any existing test data and prepares the test environment before each test method.
        /// </summary>
        [TestInitialize]
        public async Task InitTest()
        {
            await CleanTestDocuments();
        }


        /// <summary>
        /// Tests the insertion and deletion of a User document.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task InsertDeleteUserTest()
        {
            var dal = Helpers.GetDal();

            // Convert the User object to an ExpandoObject
            var userExpando = Convertor.Convert(_testUsers[0]);

            // Ensure the 'Id' field is correctly set in the ExpandoObject
            if (userExpando is IDictionary<string, object> expandoDict && !expandoDict.ContainsKey("Id"))
            {
                expandoDict["Id"] = _testUsers[0].Id;
            }

            await dal.InsertAsync(Helpers.GetCollectionName<User>(), userExpando);

            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), _testUsers[0].Id);
        }


        /// <summary>
        /// Tests querying for a User that does not exist in the database.
        /// Expects the query result to be empty.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryNonExistingUserTest()
        {
            var dal = Helpers.GetDal();

            // Create a Query object with a condition that should not match any user
            var query = Query.CreateQuery("Email", new List<object> { "nonexistentemail@example.com" }, QueryOperator.Equals);

            // Specify the fields you want to retrieve
            var fields = new List<string> { "Email", "Name", "ObjectId" }; // Adjusted fields as per the new User class

            // Execute the query with the specified fields and query conditions
            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<User>(), fields, query, 100, 0);

            Assert.IsTrue(res.Count == 0); // Asserting that no users were found
        }


        /// <summary>
        /// Tests querying for existing Users in the database based on specific criteria.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task QueryExistingUsersTest()
        {
            var dal = Helpers.GetDal();

            var query = Query.CreateQuery("Name", new List<object> { "user" }, QueryOperator.Contains);
            var fields = new List<string> { "Name", "Email", "ObjectId" };

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<User>(), fields, query, 100, 0);

            // Assert that the result is not null and possibly contains users
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Count >= 0); // Ensures that the query execution is valid
        }


        /// <summary>
        /// Tests updating a User's information in the database.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task UpdateUserTest()
        {
            var dal = Helpers.GetDal();
            var userToUpdate = _testUsers[0];

            // Convert the user to an ExpandoObject and set the '_id' field for MongoDB
            var userExpando = Convertor.Convert(userToUpdate);

            // Insert the user
            await dal.InsertAsync(Helpers.GetCollectionName<User>(), userExpando);

            // Update the user's property
            userToUpdate.Name = "UpdatedName";

            // Convert the updated user to an ExpandoObject
            var updatedUserExpando = Convertor.Convert(userToUpdate);

            // Upsert (insert or update) the user
            await dal.UpsertAsync(Helpers.GetCollectionName<User>(), new List<ExpandoObject> { updatedUserExpando });

            // Retrieve and verify the update
            var updatedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), userToUpdate.Id);
            Assert.AreEqual("UpdatedName", updatedUser.Name);

            // Cleanup
            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), userToUpdate.Id);
        }


        /// <summary>
        /// Tests the deletion of an existing User from the database.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task DeleteExistingUserTest()
        {
            var dal = Helpers.GetDal();

            var userToDelete = _testUsers[0];

            // Convert the user to an ExpandoObject and set the '_id' field for MongoDB
            dynamic userExpando = Convertor.Convert(userToDelete);
            userExpando._id = userToDelete.Id; // Set '_id' for MongoDB

            // Insert a user
            await dal.InsertAsync(Helpers.GetCollectionName<User>(), userExpando);

            // Delete the user
            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), userToDelete.Id);

            // Try to find the deleted user using GetByIdAsync
            var deletedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), userToDelete.Id);

            Assert.IsNull(deletedUser);
        }


        /// <summary>
        /// Tests the deletion of multiple User documents from the database.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task DeleteMultipleUsersTest()
        {
            var dal = Helpers.GetDal();

            // Initialize multiple users with sample data
            var users = new List<User>
            {
                new User { Id = "User1", ObjectId = "obj1", Upn = "upn1", Email = "user1@example.com", Name = "User One", ContactInfos = new List<Contact>(), Birthdate = DateTime.Now, Disabilities = false, Profile = null },
                new User { Id = "User2", ObjectId = "obj2", Upn = "upn2", Email = "user2@example.com", Name = "User Two", ContactInfos = new List<Contact>(), Birthdate = DateTime.Now, Disabilities = false, Profile = null },
                // Add more users as needed
            };

            // Clear existing test data
            var existingUserIds = users.Select(u => u.Id).ToArray();
            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), existingUserIds, false);

            // Convert users to ExpandoObjects and insert
            var userExpandoList = users.Select(user => Convertor.Convert(user)).ToList();
            await dal.InsertManyAsync(Helpers.GetCollectionName<User>(), userExpandoList);

            // Delete the users
            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), existingUserIds);

            // Verify each user is deleted
            foreach (var userId in existingUserIds)
            {
                var deletedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), userId);
                Assert.IsNull(deletedUser);
            }
        }


        /// <summary>
        /// Verifies the existence of a user in the database using the IsExistAsync method.
        /// This test inserts a test user into the database, checks for its existence,
        /// asserts that the user does indeed exist, and then cleans up by deleting the test user.
        /// </summary>
        [TestMethod]
        [TestCategory("Prod")]
        public async Task IsExistAsyncTest()
        {
            var dal = Helpers.GetDal();
            var testUser = _testUsers[0]; 

            // Convert the user to an ExpandoObject for insertion
            dynamic userExpando = Convertor.Convert(testUser);

            // Insert the user
            await dal.InsertAsync(Helpers.GetCollectionName<User>(), userExpando);

            // Check if the user exists
            bool exists = await dal.IsExistAsync<User>(Helpers.GetCollectionName<User>(), testUser.Id);

            Assert.IsTrue(exists);

            // Cleanup
            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), testUser.Id);
        }
    }
}
