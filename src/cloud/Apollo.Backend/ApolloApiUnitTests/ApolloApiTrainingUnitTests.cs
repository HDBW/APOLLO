// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Microsoft.Extensions.Logging;
using Moq;

namespace Apollo.Api.UnitTests
{
    [TestClass]
    public class ApolloApiTrainingUnitTests
    {
        private Mock<MongoDataAccessLayer>? _mockDal;
        private ApolloApi? _apolloApi;
        private Mock<ILogger<ApolloApi>>? _mockLogger;
        private ApolloApiConfig? _config;

        [TestInitialize]
        public void Initialize()
        {
            _mockDal = new Mock<MongoDataAccessLayer>();
            _apolloApi = new ApolloApi(_mockDal.Object, _mockLogger.Object, _config);
        }

        [TestMethod]
        public async Task GetTraining_ReturnsCorrectTraining()
        {
            // Arrange
            var expectedTraining = new Training { Id = "T01", TrainingName = "Test Training" };
            _mockDal.Setup(dal => dal.GetByIdAsync<Training>(It.IsAny<string>(), expectedTraining.Id))
                    .ReturnsAsync(expectedTraining);

            // Act
            var actualTraining = await _apolloApi.GetTraining(expectedTraining.Id);

            // Assert
            Assert.IsNotNull(actualTraining);
            Assert.AreEqual(expectedTraining.Id, actualTraining.Id);
            Assert.AreEqual(expectedTraining.TrainingName, actualTraining.TrainingName);
        }
    }
}
