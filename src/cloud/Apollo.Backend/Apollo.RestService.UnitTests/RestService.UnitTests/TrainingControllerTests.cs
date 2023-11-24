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

namespace Apollo.Service.Tests
{
    [TestClass]
    public class TrainingControllerTests
    {
        private TrainingController _controller;
        private Mock<ApolloApi> _mockApi;
        private Mock<ILogger<TrainingController>> _mockLogger;

        [TestInitialize]
        public void Init()
        {
            // Initialize Mocks
            _mockApi = new Mock<ApolloApi>();
            _mockLogger = new Mock<ILogger<TrainingController>>();

            // Initialize TrainingController with Mocked ApolloApi and Logger
            _controller = new TrainingController(_mockApi.Object, _mockLogger.Object);
        }

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
            var result = await _controller.QueryTrainings(request) as List<QueryTrainingsResponse>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count); // Expecting 2 trainings in the list
        }

        [TestMethod]
        public async Task CreateOrUpdateTraining_CreatesTrainingSuccessfully()
        {
            // Arrange
            var newTraining = new Training { TrainingName = "New Training", Id = "newId" };
            var request = new CreateOrUpdateTrainingRequest { Training = newTraining };

            // Mock the behavior of _mockApi.CreateOrUpdateTraining
            _mockApi.Setup(api => api.CreateOrUpdateTraining(newTraining)).ReturnsAsync("newId");

            // Act
            var result = await _controller.CreateOrUpdateTraining(request) as CreateOrUpdateTrainingResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("newId", result.Training.Id); 
        }

        [TestMethod]
        public async Task InsertTrainings_InsertsTrainingsSuccessfully()
        {
            // Arrange
            var trainingsToInsert = new List<Training> { new Training { TrainingName = "Training 1" } };
            _mockApi.Setup(api => api.CreateOrUpdateTraining(It.IsAny<Training>()))
                    .ReturnsAsync("generatedId");

            // Act
            var result = await _controller.InsertTrainings(trainingsToInsert) as IActionResult;

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

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
