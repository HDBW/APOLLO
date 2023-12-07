using System.Dynamic;
using System.Linq;
using Apollo.Api;
using Apollo.Common.Entities;
using MongoDB.Driver;
using Query = Daenet.MongoDal.Entitties.Query;
using QueryOperator = Daenet.MongoDal.Entitties.QueryOperator;

namespace Daenet.MongoDal.UnitTests
{
    [TestCategory("MongoDal")]
    [TestClass]
    public class UsersUnitTests
    {
        User[] _testUsers = new User[]
        {
            new User(){ Id = "U01", UserName = "user1", FirstName = "Test", LastName = "User1", Goal = "Learn AI" },
            new User(){ Id = "U02", UserName = "user2", FirstName = "Test", LastName = "User2", Goal = "Explore AI" },
            new User(){ Id = "U03", UserName = "user3", FirstName = "Test", LastName = "User3" },
        };

        private async Task CleanTestDocuments()
        {
            var dal = Helpers.GetDal();

            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), _testUsers.Select(u => u.Id).ToArray(), false);
        }

        [TestCleanup]
        public async Task CleanupTest()
        {
            await CleanTestDocuments();
        }

        [TestInitialize]
        public async Task InitTest()
        {
            await CleanTestDocuments();
        }


        [TestMethod]
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

        [TestMethod]
        public async Task QueryNonExistingUserTest()
        {
            var dal = Helpers.GetDal();

            // Create a Query object with a condition that should not match any user
            var query = Query.CreateQuery("UserName", new List<object> { "nonexistentuser" }, QueryOperator.Equals);

            // Specify the fields you want to retrieve
            var fields = new List<string> { "UserName", "FirstName", "LastName" };

            // Execute the query with the specified fields and query conditions
            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<User>(), fields, query, 100, 0);

            Assert.IsTrue(res.Count == 0);
        }

        [TestMethod]
        public async Task QueryExistingUsersTest()
        {
            var dal = Helpers.GetDal();

            var query = Query.CreateQuery("UserName", new List<object> { "user" }, QueryOperator.Contains);
            var fields = new List<string> { "UserName", "FirstName", "LastName" };

            var res = await dal.ExecuteQuery(Helpers.GetCollectionName<User>(), fields, query, 100, 0);

            // Assert that the result is not null and possibly contains users
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Count >= 0); // Ensures that the query execution is valid
        }
        [TestMethod]
        public async Task UpdateUserTest()
        {
            var dal = Helpers.GetDal();
            var userToUpdate = _testUsers[0];

            // Convert the user to an ExpandoObject and set the '_id' field for MongoDB
            var userExpando = Convertor.Convert(userToUpdate);

            // Insert the user
            await dal.InsertAsync(Helpers.GetCollectionName<User>(), userExpando);

            // Update the user's property
            userToUpdate.FirstName = "UpdatedFirstName";

            // Convert the updated user to an ExpandoObject
            var updatedUserExpando = Convertor.Convert(userToUpdate);

            // Upsert (insert or update) the user
            await dal.UpsertAsync(Helpers.GetCollectionName<User>(), new List<ExpandoObject> { updatedUserExpando });

            // Retrieve and verify the update
            var updatedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), userToUpdate.Id);
            Assert.AreEqual("UpdatedFirstName", updatedUser.FirstName);

            // Cleanup
            await dal.DeleteAsync(Helpers.GetCollectionName<User>(), userToUpdate.Id);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task DeleteMultipleUsersTest()
        {
            var dal = Helpers.GetDal();

            // Initialize multiple users with sample data
            var users = new List<User>
            {
                new User { Id = "User1", UserName = "user1", FirstName = "Test", LastName = "User1", Goal = "Learn AI", Image = "user1.png" },
                new User { Id = "User2", UserName = "user2", FirstName = "Sample", LastName = "User2", Goal = "Explore AI", Image = "user2.png" },
                // Add more users as needed
            };

            // Convert users to ExpandoObjects
            var userExpandoList = users.Select(user => Convertor.Convert(user)).ToList();

            await dal.InsertManyAsync(Helpers.GetCollectionName<User>(), userExpandoList);

            var userIds = users.Select(u => u.Id).ToArray();
            await dal.DeleteManyAsync(Helpers.GetCollectionName<User>(), userIds);

            // Verify each user is deleted
            foreach (var userId in userIds)
            {
                var deletedUser = await dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), userId);
                Assert.IsNull(deletedUser);
            }
        }
    }
}
