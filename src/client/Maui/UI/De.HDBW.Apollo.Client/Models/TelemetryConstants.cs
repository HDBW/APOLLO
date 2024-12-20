﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Services;
using Microsoft.ApplicationInsights.Extensibility;

namespace De.HDBW.Apollo.Client.Models
{
    public static class TelemetryConstants
    {
        public static string? ConnectionString { get; private set; }

        public static TelemetryConfiguration Configuration { get; private set; } = TelemetryConfiguration.CreateDefault();

        public static string SessionId { get; set; } = Guid.NewGuid().ToString();

        public static void ApplySecrets(UserSecretsService userSecretsService)
        {
            try
            {
                ConnectionString = $"InstrumentationKey={userSecretsService["InstrumentationKey"] ?? string.Empty};";
                Configuration.DisableTelemetry = false;
                Configuration.ConnectionString = ConnectionString;
            }
            catch
            {
            }
        }
    }
}
