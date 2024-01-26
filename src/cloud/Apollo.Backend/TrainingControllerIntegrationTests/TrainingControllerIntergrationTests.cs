// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net.Http;
using System.Threading.Tasks;
using Apollo.Common.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace TrainingControllerIntegrationTests
{
    public class TrainingControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly IMongoDatabase _database;

        public TrainingControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _database = factory.Services.GetRequiredService<IMongoDatabase>();
        }

        [Fact]
        public async Task GetTraining_ReturnsTraining()
        {
            // Arrange: Set up any necessary test data or context
            var collection = _database.GetCollection<Training>("training");

            var trainingData = new Training
            {
                Id = "T10",
                TrainingType = "Seminar",
                TrainingName = "Training T10",
                // Set other properties as needed
            };
            await collection.InsertOneAsync(trainingData);

            // Act: Make an HTTP GET request to the GetTraining endpoint
            var response = await _client.GetAsync("/api/Training/T10");

            // Assert: Check the response status code and content
            response.EnsureSuccessStatusCode(); // Ensure a successful response (status code 2xx)
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);

            var returnedTraining = Newtonsoft.Json.JsonConvert.DeserializeObject<Training>(content);
            Assert.NotNull(returnedTraining);
            Assert.Equal("T10", returnedTraining.Id);
            Assert.Equal("Seminar", returnedTraining.TrainingType);
            Assert.Equal("Training T10", returnedTraining.TrainingName);
            // Assert content as needed
        }
    }
}
