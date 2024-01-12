using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Apollo.Service.Controllers;
using Apollo.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Apollo.Api;
using Microsoft.Extensions.Logging;
using Apollo.RestService.Messages;
using Daenet.MongoDal;
using MongoDB.Driver;
using Daenet.MongoDal.Entitties;
using Moq;

namespace Apollo.RestService.UnitTests
{
    /// <summary>
    /// Unit tests for the TrainingController class.
    /// </summary>
    [TestClass]
    public class TrainingControllerTests
    {
        private TrainingController _controller;
        private Mock<ApolloApi> _mockApi;
        private Mock<ILogger<TrainingController>> _mockLogger;


        /// <summary>
        /// Initializes the test environment before each test method runs.
        /// This setup includes creating mock instances for ApolloApi and ILogger and initializing the TrainingController with these mocks.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            // Initialize Mocks
            _mockApi = new Mock<ApolloApi>();
            _mockLogger = new Mock<ILogger<TrainingController>>();

            // Initialize TrainingController with Mocked ApolloApi and Logger
            _controller = new TrainingController(_mockApi.Object, _mockLogger.Object);
        }


        /// <summary>
        /// Tests the GetTraining method in TrainingController to ensure it returns the correct Training response.
        /// </summary>
        [TestMethod]
        public async Task GetTraining_ReturnsTraining()
        {
            // Arrange
            var trainingId = "someId";
            var expectedTraining = new Training { Id = trainingId, TrainingName = "Test Training" };
            _mockApi.Setup(api => api.GetTraining(trainingId)).ReturnsAsync(expectedTraining);

            // Act
            var result = await _controller.GetTraining(trainingId) as GetTrainingResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTraining.TrainingName, result.Training.TrainingName);
        }


        /// <summary>
        /// Tests the QueryTrainings method in TrainingController to ensure it returns a list of Training objects.
        /// </summary>
        [TestMethod]
        public async Task QueryTrainings_ReturnsListOfTrainings()
        {
            // Arrange
            var request = new Apollo.RestService.Messages.QueryTrainingsRequest();
            var expectedTrainings = new List<Training>
            {
                new Training { TrainingName = "Training 1" },
                new Training { TrainingName = "Training 2" }
            };


            _mockApi.Setup(api => api.QueryTrainings(It.IsAny<Apollo.Common.Entities.Query>()))
                    .ReturnsAsync(expectedTrainings);

            // Act
            var result = await _controller.QueryTrainings(request) as QueryTrainingsResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Trainings.Count); // Expecting 2 trainings in the list
        }


        /// <summary>
        /// Tests the CreateOrUpdateTraining method in TrainingController to ensure it creates a new Training successfully.
        /// </summary>
        [TestMethod]
        public async Task CreateOrUpdateTraining_CreatesTrainingSuccessfully()
        {
            // Arrange
            var newTraining = new Training { TrainingName = "New Training" };
            var trainingList = new List<Training> { newTraining };

            // Mock the behavior of the CreateOrUpdateTraining method
            _mockApi.Setup(api => api.CreateOrUpdateTraining(It.IsAny<ICollection<Training>>()))
                    .Callback<ICollection<Training>>(trainings => trainings.First().Id = "newId") // Update the ID of the first training object in the collection
                    .ReturnsAsync(new List<string> { "newId" }); // Return a list with a single ID

            // Act
            var result = await _controller.CreateOrUpdateTraining(new CreateOrUpdateTrainingRequest { Training = newTraining }) as CreateOrUpdateTrainingResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Training); // Check if Training is not null
            Assert.AreEqual("newId", result.Training.Id); // Verify the ID of the training
        }


        /// <summary>
        /// Tests the InsertTrainings method in TrainingController to ensure it inserts a list of Training objects successfully.
        /// </summary>
        [TestMethod]
        public async Task InsertTrainings_InsertsTrainingsSuccessfully()
        {
            // Arrange
            var trainingsToInsert = new List<Training> { new Training { TrainingName = "Training 1" } };
            _mockApi.Setup(api => api.CreateOrUpdateTraining(It.IsAny<ICollection<Training>>()))
                    .ReturnsAsync(new List<string> { "generatedId" });  // Mocking the expected behavior of the API

            // Act
            var result = await _controller.InsertTrainings(trainingsToInsert);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<Training>));  // Check if the result is of type IList<Training>
            Assert.AreEqual(1, result.Count);  // Expecting 1 training in the list
            Assert.AreEqual("Training 1", result[0].TrainingName);  // Validate the content of the returned list
        }


        /// <summary>
        /// Tests the Delete method in TrainingController to ensure it successfully removes a specified Training.
        /// </summary>
        [TestMethod]
        public async Task Delete_RemovesTrainingSuccessfully()
        {
            // Arrange
            var trainingId = "trainingIdToDelete";
            _mockApi.Setup(api => api.DeleteTrainings(It.IsAny<string[]>()))
                    .ReturnsAsync(1);

            // Act
            await _controller.Delete(trainingId);

            // Assert
            _mockApi.Verify(api => api.DeleteTrainings(It.Is<string[]>(ids => ids.Contains(trainingId))), Times.Once());
        }


        /// <summary>
        /// Cleans up the test environment after each test method has run.
        /// This includes resetting the mock objects for ApolloApi and ILogger.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Perform any necessary cleanup after the tests have run

            // Clearing mocks
            _mockApi.Reset();
            _mockLogger.Reset();
        }

    }
}
