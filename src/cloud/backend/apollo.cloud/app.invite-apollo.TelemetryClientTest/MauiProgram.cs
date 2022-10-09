// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;

namespace app.invite_apollo.TelemetryClientTest
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //Read Variables from User Secrets
            SetEnvironmentVariablesFromUserSecrets();
            //TODO: Implement Logger
            //https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service#net-corenet-framework-console-application
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }

        private static void SetEnvironmentVariablesFromUserSecrets()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<App>().Build();
            foreach (var child in config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
        }
    }
}
