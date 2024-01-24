// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TrainingControllerIntegrationTests
{
    public class TrainingControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TrainingControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTraining_ReturnsTraining()
        {
            // Arrange: Set up any necessary test data or context

            // Act: Make an HTTP GET request to the GetTraining endpoint
            var response = await _client.GetAsync("/api/Training/T06");

            // Assert: Check the response status code and content
            response.EnsureSuccessStatusCode(); // Ensure a successful response (status code 2xx)
            var content = await response.Content.ReadAsStringAsync();
            // Assert content as needed
        }
    }
}
