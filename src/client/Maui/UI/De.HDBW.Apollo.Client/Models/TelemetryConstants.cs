using De.HDBW.Apollo.Client.Services;
using Microsoft.ApplicationInsights.Extensibility;

namespace De.HDBW.Apollo.Client.Models
{
    public static class TelemetryConstants
    {
        public static string? ConnectionString { get; private set; }

        public static TelemetryConfiguration Configuration { get; private set; } = TelemetryConfiguration.CreateDefault();

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
