// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;

namespace Apollo.RestService.IntergrationTests
{
    internal class Helpers
    {
        private static readonly TestSettings _settings;

        static Helpers()
        {
            var settingsFile = Environment.GetEnvironmentVariable("testsettings");

            if (String.IsNullOrEmpty(settingsFile))
                settingsFile = "testsettings.tst.json";

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(settingsFile, optional: false, reloadOnChange: true)
               .AddCommandLine(Environment.GetCommandLineArgs())
               .AddEnvironmentVariables();

            var cfg = builder.Build();

            _settings = new TestSettings();

            cfg.Bind(_settings);
        }

      

        public static HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"{_settings.ServiceUrl.TrimEnd('/')}/api/")
            };

      

            httpClient.DefaultRequestHeaders.Add("ApiKey", _settings.ApiKey);

            return httpClient;
        }
    }
}
