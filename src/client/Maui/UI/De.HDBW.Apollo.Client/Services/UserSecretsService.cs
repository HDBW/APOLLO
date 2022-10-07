using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace De.HDBW.Apollo.Client.Services
{
    public class UserSecretsService : IUserSecretsService
    {
        private const string UserSecretsFileName = "secrets.json";

        private JObject _secretsJObject;

        public UserSecretsService(ILogger<UserSecretsService>? logger)
        {
            Logger = logger;
        }

        private ILogger<UserSecretsService>? Logger { get; }

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

                    JToken? node = _secretsJObject[path.First()];
                    if (node == null)
                    {
                        return null;
                    }

                    for (int index = 1; index < path.Length; index++)
                    {
                        if (node == null)
                        {
                            return null;
                        }

                        node = node[path[index]];
                    }

                    return node?.ToString();
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
                    if (stream == null)
                    {
                        return false;
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        var json = reader.ReadToEnd();
                        _secretsJObject = JObject.Parse(json);
                    }
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
