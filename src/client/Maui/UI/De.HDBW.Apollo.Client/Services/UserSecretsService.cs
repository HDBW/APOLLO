﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Services
{
    public class UserSecretsService : IUserSecretsService
    {
        private const string UserSecretsFileName = "secrets.json";

        private IConfigurationRoot? _config;

        public UserSecretsService()
        {
        }

        public ILogger? Logger { get; set; }

        public string? this[string name]
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
                using (var stream = assembly.GetManifestResourceStream($"{typeof(App).Namespace}.{UserSecretsFileName}"))
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
