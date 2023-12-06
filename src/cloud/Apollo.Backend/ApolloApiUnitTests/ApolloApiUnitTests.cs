// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using System.Collections;
using Apollo.Api;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Daenet.MongoDal.Entitties;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace apolloapiunittests
{

    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiTests
    {
        private ApolloApi? _apolloApi;
        private MongoDataAccessLayer? _mongoDal;
        private ILogger<ApolloApi>? _logger;
        private readonly MongoDataAccessLayer? _dal;

        [TestInitialize]
        public void Initialize()
        {
            // Create an instance of MongoDalConfig with your MongoDB configuration
            var mongoDalConfig = new MongoDalConfig
            {
                MongoConnStr = "mongodb://apollodb-cosmos-dev:TV9uJP68Tr07LalP2dVazch7AVBXfsVqB4HIzkGMNbBQtKWe7aTT42iKdZt8IuCuH0UHLjbgmwwRACDbIv9d2A==@apollodb-cosmos-dev.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@apollodb-cosmos-dev@",
                MongoDatabase = "apollodb"
                // Add other configuration settings as needed
            };

            // Validate the configuration
            mongoDalConfig.Validate();

            // Initialize the actual implementations
            _mongoDal = new MongoDataAccessLayer(mongoDalConfig, null); // Use null for the optional ILogger parameter
            _logger = new Logger<ApolloApi>(new LoggerFactory());

            // Create an instance of ApolloApi for testing
            _apolloApi = new ApolloApi(_mongoDal, _logger, new ApolloApiConfig());
        }


        [TestMethod]
        public async Task UpdateTrainingTest()
        {
            // Create a new training or retrieve an existing one for updating
            var updatedTraining = new Training
            {
                // Update other properties as needed
                TrainingName = "Updated Training Name" // Example: Update the training name
            };

            // Call the CreateOrUpdateTraining method to update the training
            var updatedTrainingId = await _apolloApi.CreateOrUpdateTraining(updatedTraining);

            // Assert that the updatedTrainingId is not null or empty, indicating a successful update
            Assert.IsFalse(string.IsNullOrEmpty(updatedTrainingId));
        }


        [TestMethod]
        public async Task DeleteTrainingTest()
        {
            // Specify the training ID to delete
            var trainingIdToDelete = "your_training_id_here"; // Replace with a valid training ID

            // Call the DeleteTrainings method to delete the training by ID
            var deletedCount = await _apolloApi.DeleteTrainings(new string[] { trainingIdToDelete });

            // Assert that the correct number of trainings were deleted (typically 1 if successful)
            Assert.AreEqual(1, deletedCount);
        }

        [TestMethod]
        public async Task GetTraining_ShouldReturnTrainingInfo()
        {
            var trainingId = "123";
            var result = await _apolloApi.GetTraining(trainingId);

            Assert.IsNotNull(result);

            // Define a list of properties to validate
            var propertiesToValidate = new List<Func<object>>
            {
                () => result.Id,
                () => result.TrainingName,
                () => result.ExternalTrainingId,
                () => result.Description,
                () => result.Content,
                () => result.BenefitList,
                () => result.Certificate,
                () => result.Prerequisites,
                () => result.TrainingProvider,
                () => result.CourseProvider,
                () => result.Appointments,
                () => result.ProductUrl,
                () => result.Contacts,
                () => result.TrainingType,
                () => result.IndividualStartDate,
                () => result.Price,
                () => result.AccessibilityAvailable,
                () => result.Tags,
                () => result.Categories,
                () => result.PublishingDate,
                () => result.UnpublishingDate,
                () => result.Successor,
                () => result.Predecessor
            };

            // Use a loop to check each property
            foreach (var propertyToValidate in propertiesToValidate)
            {
                var propertyValue = propertyToValidate();
                Assert.IsNotNull(propertyValue);

                if (propertyValue is string stringValue)
                {
                    Assert.IsFalse(string.IsNullOrEmpty(stringValue));
                }
                else if (propertyValue is ICollection collection)
                {
                    Assert.IsTrue(collection.Count > 0);
                }
            }
        }

        [TestMethod]
        public async Task QueryTrainings_ShouldReturnListOfTrainings()
        {
            // Create a query to retrieve trainings
            var query = new Apollo.Common.Entities.Query();

            // Call the _apolloApi.QueryTrainings method to retrieve the list of trainings
            var result = await _apolloApi.QueryTrainings(query);

            // Assert that the result is not null
            Assert.IsNotNull(result);

            // Add more assertions to validate the retrieved list of trainings
            foreach (var training in result)
            {
                Assert.IsNotNull(training.Id); // Check that the training has an ID
                Assert.IsFalse(string.IsNullOrEmpty(training.TrainingName)); // Check that the training name is not empty
                Assert.IsTrue(training.Certificate.Count > 0); // Check that the training has certificates
                // Add more assertions as needed to validate other properties of the training
            }
        }


        [TestMethod]
        public async Task CreateOrUpdateTraining_ShouldReturnGeneratedGuid()
        {
            // Arrange
            var training = new Training(); // Customize the training object as needed

            // Act
            var result = await _apolloApi.CreateOrUpdateTraining(training);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(Guid.TryParse(result, out _));
        }

        [TestMethod]
        public async Task DeleteTrainings_ShouldReturnNumberOfDeletedTrainings()
        {
            var trainingIds = new string[] { "123", "456" };

            var result = await _apolloApi.DeleteTrainings(trainingIds);

            Assert.IsTrue(result >= 0);

            foreach (var trainingId in trainingIds)
            {
                var deletedTraining = await _apolloApi.GetTraining(trainingId);
                Assert.IsNull(deletedTraining); // Assert that the deleted training is null
            }
            var apolloApi = new ApolloApi();

            var initialTrainingCount = await apolloApi.GetTotalTrainingCountAsync();

            var expectedTrainingCount = initialTrainingCount - result;
            var updatedTrainingCount = await apolloApi.GetTotalTrainingCountAsync();
            Assert.AreEqual(expectedTrainingCount, updatedTrainingCount);

            // Add more assertions if needed
        }


        [TestMethod]
        public async Task QueryUsersByKeyword_ShouldReturnListOfUsers()
        {
            // Arrange
            var keyword = "John";
            var expectedUsers = new List<User> { new User() { FirstName = "John", LastName = "Doe" } };

            // Act
            var result = await _apolloApi.QueryUsersByKeyword(keyword);

            // Assert
            CollectionAssert.AreEqual(expectedUsers.ToArray(), result.ToArray(), "The lists are not equal. Custom assertion message.");
        }

        [TestMethod]
        public async Task UpdateUserTest()
        {
            // Create a new user or retrieve an existing one for updating
            var userId = "your_user_id_here"; // Replace with a valid user ID
            var updatedUser = new User
            {
                Id = userId, // Set the ID of the user to update
                             // Update other properties as needed
                FirstName = "Updated First Name" // Example: Update the first name
            };

            // Call the CreateOrUpdateUser method to update the user
            var result = await _apolloApi.CreateOrUpdateUser(updatedUser);

            // Assert that the update was successful by checking if the result is not null
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DeleteUserTest()
        {
            // Specify the user ID to delete
            var userIdToDelete = "your_user_id_here"; // Replace with a valid user ID

            // Convert the user ID to an integer (if necessary)
            int userIdAsInt = int.Parse(userIdToDelete);

            // Call the DeleteUser method to delete the user
            var deletedCount = await _apolloApi.DeleteUser(new int[] { userIdAsInt });

            // Assert that the correct number of users were deleted (typically 1 if successful)
            Assert.AreEqual(1, deletedCount);
        }

        [TestMethod]
        public async Task GetUserTest()
        {
            // Specify the user ID to retrieve
            var userIdToRetrieve = "your_user_id_here"; // Replace with a valid user ID

            var retrievedUser = await _apolloApi.GetUser(userIdToRetrieve);

            // Assert that the retrievedUser is not null (indicating a successful retrieval)
            Assert.IsNotNull(retrievedUser);
        }

        [TestMethod]
        public async Task CreateOrUpdateUserTest()
        {
            // Create a new user or update an existing one
            var newUser = new User
            {
                FirstName = "New First Name",
                LastName = "New Last Name"
                // Add other properties as needed
            };

            var result = await _apolloApi.CreateOrUpdateUser(newUser);

            // Assert that the result is not null (indicating a successful creation or update)
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task QueryUsersByFirstNameTest()
        {
            // Specify the first name to use as a filter
            var firstNameToQuery = "John"; // Replace with a valid first name

            var filteredUsers = await _apolloApi.QueryUsersByFirstName(firstNameToQuery);

            // Assert that the filteredUsers list is not null or empty
            Assert.IsNotNull(filteredUsers);
            Assert.IsTrue(filteredUsers.Count > 0);
        }

        [TestMethod]
        public async Task QueryUsersByLastNameTest()
        {
            // Specify the last name to use as a filter
            var lastNameToQuery = "Doe"; // Replace with a valid last name

            var filteredUsers = await _apolloApi.QueryUsersByLastName(lastNameToQuery);

            // Assert that the filteredUsers list is not null or empty
            Assert.IsNotNull(filteredUsers);
            Assert.IsTrue(filteredUsers.Count > 0);
        }

        [TestMethod]
        public async Task QueryUsersByMultipleCriteriaTest()
        {
            // Specify filter criteria (example: first name, last name, and goal)
            var firstName = "John"; // Replace with a valid first name
            var lastName = "Doe";   // Replace with a valid last name
            var goal = "Learn";     // Replace with a valid goal

            var filteredUsers = await _apolloApi.QueryUsersByMultipleCriteria(firstName, lastName, goal);

            // Assert that the filteredUsers list is not null or empty
            Assert.IsNotNull(filteredUsers);
            Assert.IsTrue(filteredUsers.Count > 0);
        }

        [TestMethod]
        public async Task QueryUsersWithPaginationTest()
        {
            // Specify pagination parameters (example: page number and page size)
            var pageNumber = 1; // Replace with a valid page number
            var pageSize = 10;  // Replace with a valid page size

            var paginatedUsers = await _apolloApi.QueryUsersWithPagination(pageNumber, pageSize);

            // Assert that the paginatedUsers list is not null or empty
            Assert.IsNotNull(paginatedUsers);
            Assert.IsTrue(paginatedUsers.Count > 0);
        }


        [TestMethod]
        public async Task InsertUser_ShouldReturnGeneratedGuid()
        {
            // Arrange
            var user = new User();

            // Act
            var result = await _apolloApi.InsertUser(user);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(Guid.TryParse(result, out _));
        }


        /// <summary>
        /// Gets the total count of users.
        /// </summary>
        /// <returns>Task that represents the asynchronous operation, containing the total count of users.</returns>
        [TestMethod]
        public async Task GetTotalUserCount()
        {
            // You can use your data access layer to retrieve the total count of users
            var filter = Builders<BsonDocument>.Filter.Empty; // Create an empty filter
            var count = await _dal.CountDocumentsAsync("yourCollectionName", filter);

            Assert.IsNotNull(count);

            var totalCount = (int)count;

            // Add assertions based on your requirements
        }



        public class UserEqualityComparer : IEqualityComparer<User>
        {
            public bool Equals(User x, User y)
            {
                if (ReferenceEquals(x, y))
                    return true;
                if (x is null || y is null)
                    return false;

                return x.FirstName == y.FirstName && x.LastName == y.LastName;
            }

            public int GetHashCode(User obj)
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + obj.FirstName?.GetHashCode() ?? 0;
                    hash = hash * 23 + obj.LastName?.GetHashCode() ?? 0;
                    return hash;
                }
            }
        }

        private Daenet.MongoDal.Entitties.SortExpression ConvertSortExpression(Apollo.Common.Entities.SortExpression source)
        {
            if (source == null)
            {
                return null;
            }

            // Perform the conversion here based on the properties of source
            var dalSortExpression = new Daenet.MongoDal.Entitties.SortExpression
            {
                FieldName = source.FieldName,
                Order = ConvertSortOrder(source.Order)
                // Add any other properties you need to convert
            };

            return dalSortExpression;
        }

        private Daenet.MongoDal.Entitties.SortOrder ConvertSortOrder(Apollo.Common.Entities.SortOrder order)
        {
            // Define the mapping from Apollo sort order to Daenet sort order
            switch (order)
            {
                case Apollo.Common.Entities.SortOrder.Ascending:
                    return Daenet.MongoDal.Entitties.SortOrder.Ascending;
                case Apollo.Common.Entities.SortOrder.Descending:
                    return Daenet.MongoDal.Entitties.SortOrder.Descending;
                default:
                    // Handle any other cases as needed
                    return Daenet.MongoDal.Entitties.SortOrder.Ascending; // Default to Ascending
            }
        }
    }
}
