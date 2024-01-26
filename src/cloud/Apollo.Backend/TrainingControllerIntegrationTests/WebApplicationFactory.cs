// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;


namespace TrainingControllerIntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        // Override the ConfigureWebHost method to configure the test host
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Configure the test host environment 
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json"); 
            });

            // Configure test services or dependencies (if needed)
            
        }
    }
}
