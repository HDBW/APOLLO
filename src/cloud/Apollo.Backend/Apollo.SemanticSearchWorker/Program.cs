// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Api;
using Microsoft.Extensions.Configuration;

namespace Apollo.SemanticSearchWorker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Exporter started..");

            IConfigurationRoot cfg = InitializeConfiguration(args);

            ApolloApi api = GetApi(cfg);

        }

        private static IConfigurationRoot InitializeConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .AddCommandLine(args)
                 .AddEnvironmentVariables();

            return builder.Build();
        }


        private static ApolloApi GetApi(IConfigurationRoot cfg)
        {
            return null;
        }
    }
}
