//(c)Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Api;
using Apollo.Common.Entities;

namespace ApolloApiUnitTests
{
    /// <summary>
    /// Unit tests for the ApolloApi class.
    /// </summary>
    [TestClass]
    public class ApolloApiUnitTests
    {
        private ApolloApi _api;

        /// <summary>
        /// Initialize the test environment and create an instance of ApolloApi for testing.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            var dal = Helpers.GetDal();
            _api = new ApolloApi(dal, null);
        }

        /// <summary>
        /// Test method to verify the InsertTrainings functionality.
        /// </summary>
        [TestMethod]
        public async Task InsertTrainingsTest()
        {
            // Create test data (trainings).
            var testTrainings = new List<Training>
            {
                new Training { Id = "T01", ProviderId = "unittest", TrainingName = "Open AI" },
                new Training { Id = "T02", ProviderId = "unittest", TrainingName = "Azure AI" },
                new Training { Id = "T03", ProviderId = "unittest" },
            };

            // Test the InsertTrainings method.
            await _api.InsertTrainings(testTrainings);

            // Add assertions to check if the data is inserted correctly.
            foreach (var training in testTrainings)
            {
                var insertedTraining = await _api.GetTraining(training.Id);
                Assert.IsNotNull(insertedTraining);
                Assert.AreEqual(training.TrainingName, insertedTraining.TrainingName);
                Assert.AreEqual(training.ProviderId, insertedTraining.ProviderId);
                Assert.AreEqual(training.Description, insertedTraining.Description);

                Assert.AreEqual(training.ShortDescription, insertedTraining.ShortDescription);
                Assert.AreEqual(training.Content, insertedTraining.Content);
                Assert.AreEqual(training.BenefitList, insertedTraining.BenefitList);

            }

            [TestMethod]
            async Task UpdateTrainingsTest()
            {
                // Create test data (trainings).
                var testTrainings = new List<Training>
                {
                    new Training { Id = "T01", ProviderId = "unittest", TrainingName = "Open AI" },
                    new Training { Id = "T02", ProviderId = "unittest", TrainingName = "Azure AI" },
                    new Training { Id = "T03", ProviderId = "unittest" },
                };

                // Insert the initial data.
                await _api.InsertTrainings(testTrainings);

                // Update the training data.
                foreach (var training in testTrainings)
                {
                    // Modify some properties for update.
                    training.Description = "Updated Description";
                    training.ShortDescription = "Updated Short Description";
                    // Add more properties to update as needed.
                }

                // Call the UpdateTrainings method in ApolloApi.
                await _api.CreateOrUpdateTraining(testTrainings.First());

                // Add assertions to check if the data is updated correctly.
                foreach (var updatedTraining in testTrainings)
                {
                    var retrievedTraining = await _api.GetTraining(updatedTraining.Id);
                    Assert.IsNotNull(retrievedTraining);
                    Assert.AreEqual(updatedTraining.Description, retrievedTraining.Description);
                    Assert.AreEqual(updatedTraining.ShortDescription, retrievedTraining.ShortDescription);
                    // Add more assertions for other properties as needed.
                }
            }

            [TestMethod]
            async Task DeleteTrainingsTest()
            {
                // Create test data (trainings).
                var testTrainings = new List<Training>
                {
                    new Training { Id = "T01", ProviderId = "unittest", TrainingName = "Open AI" },
                    new Training { Id = "T02", ProviderId = "unittest", TrainingName = "Azure AI" },
                    new Training { Id = "T03", ProviderId = "unittest" },
                };

                // Insert the initial data.
                await _api.InsertTrainings(testTrainings);

                // Get the IDs of the inserted trainings.
                var trainingIds = testTrainings.Select(t => t.Id).ToList();

                // Delete the training data.
                await _api.DeleteTrainings(trainingIds);

                // Add assertions to check if the data is deleted correctly.
                foreach (var trainingId in trainingIds)
                {
                    var deletedTraining = await _api.GetTraining(trainingId);
                    Assert.IsNull(deletedTraining);
                }
            }


            /// <summary>
            /// Test method to verify the InsertUsers functionality.
            /// </summary>
            [TestMethod]

            async Task InsertUsersTest()
            {
                // Create test data (users).
                var testUsers = new List<User>
            {
                new User { Id = "U01", UserName = "user1", Goal = "Some goal", FirstName = "John", LastName = "Doe", Image = "user1.png" },
                new User { Id = "U02", UserName = "user2", Goal = "Another goal", FirstName = "Jane", LastName = "Doe", Image = "user2.png" },
                // Add more test users as needed.
            };

                // Test the InsertUsers method.
                await _api.InsertUsers(testUsers);

                // Add assertions to check if the data is inserted correctly.
                foreach (var user in testUsers)
                {
                    var insertedUser = await _api.GetUserById(user.Id);
                    Assert.IsNotNull(insertedUser);
                    Assert.AreEqual(user.UserName, insertedUser.UserName);
                    Assert.AreEqual(user.Goal, insertedUser.Goal);
                    Assert.AreEqual(user.FirstName, insertedUser.FirstName);
                    Assert.AreEqual(user.LastName, insertedUser.LastName);
                    Assert.AreEqual(user.Image, insertedUser.Image);
                    // Add more assertions for other properties as needed.
                }
            }


            [TestMethod]
            async Task UpdateUsersTest()
            {
                // Create test data (users).
                var testUsers = new List<User>
                {
                    new User { Id = "U01", UserName = "user1", Goal = "Some goal", FirstName = "John", LastName = "Doe", Image = "user1.png" },
                    new User { Id = "U02", UserName = "user2", Goal = "Another goal", FirstName = "Jane", LastName = "Doe", Image = "user2.png" },
                    // Add more test users as needed.
                };

                // Insert the initial data.
                await _api.InsertUsers(testUsers);

                // Update the user data.
                foreach (var user in testUsers)
                {
                    // Update user properties as needed.
                    user.Goal = "Updated goal";
                    user.FirstName = "Updated first name";
                    user.LastName = "Updated last name";

                    await _api.CreateOrUpdateUser(user);
                }

                // Add assertions to check if the data is updated correctly.
                foreach (var user in testUsers)
                {
                    var updatedUser = await _api.GetUser(user.Id);
                    Assert.IsNotNull(updatedUser);
                    Assert.AreEqual("Updated goal", updatedUser.Goal);
                    Assert.AreEqual("Updated first name", updatedUser.FirstName);
                    Assert.AreEqual("Updated last name", updatedUser.LastName);
                    // Add more assertions for other properties as needed.
                }
            }

            [TestMethod]
            async Task DeleteUsersTest()
            {
                // Create test data (users).
                var testUsers = new List<User>
                {
                    // Add test user objects here.
                };

                // Insert the initial data.
                await _api.InsertUsers(testUsers);

                // Delete the user data.
                foreach (var user in testUsers)
                {
                    await _api.DeleteUser(new int[] { int.Parse(user.Id) });
                }

                // Add assertions to check if the data is deleted correctly.
                foreach (var user in testUsers)
                {
                    var deletedUser = await _api.GetUserById(user.Id);
                    Assert.IsNull(deletedUser); // Check that the user is deleted.
                }
            }
        }
    }
}

