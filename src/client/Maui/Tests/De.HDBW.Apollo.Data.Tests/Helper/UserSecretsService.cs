// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Tests.Helper
{
    public class UserSecretsService
    {
        private const string UserSecretsFileName = "secrets.json";

        private IConfigurationRoot _config;

        public UserSecretsService()
        {
        }

        public ILogger Logger { get; set; }

        public string this[string name]
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        return null;
                    }

                    var path = name.Split(':');

                    if (path.Length < 1)
                    {
                        return null;
                    }

                    return _config?.GetChildren().FirstOrDefault(c => c.Key == path.First())?.Value;
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unable to retrieve secret '{name}'");
                    return string.Empty;
                }
            }
        }

        public bool LoadSecrets()
        {
            try
            {
                var assembly = GetType().Assembly;
#if DEVICEBUILD
                var assemblyName = "De.HDBW.Apollo.DeviceTestRunner";
#else
                var assemblyName = "De.HDBW.Apollo.TestRunner";
#endif
                using (var stream = assembly.GetManifestResourceStream($"{assemblyName}.{UserSecretsFileName}"))
                {
                    if (stream == null || !stream.CanRead)
                    {
                        return false;
                    }

                    _config = new ConfigurationBuilder()
                   .AddJsonStream(stream)
                   .Build();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unable to load secrets file: {ex.Message}.");
            }

            return false;
        }
    }
}
