using De.HDBW.Apollo.Client.Services;

namespace De.HDBW.Apollo.Client.Models
{
    public static class B2CConstants
    {
        public static string ClientId
        {
            get;
            private set;
        }

        public static string[] Scopes
        {
            get;
            private set;
        } = { "openid", "offline_access" };

        public static string TenantName
        {
            get;
            private set;
        }

        public static string TenantId
        {
            get
            {
                return $"{TenantName}.onmicrosoft.com";
            }
        }

        public static string SignInPolicy
        {
            get;
            private set;
        }

        public static string AuthorityBase
        {
            get
            {
                return $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
            }
        }

        public static string AuthoritySignIn
        {
            get
            {
                return $"{AuthorityBase}{SignInPolicy}";
            }
        }

        public static string IosKeychainSecurityGroups
        {
            get
            {
                return "com.microsoft.adalcache";
            }
        }

        public static void ApplySecrets(UserSecretsService userSecretsService)
        {
            try
            {
                ClientId = userSecretsService["ClientId"] ?? string.Empty;
                TenantName = userSecretsService["TenantName"] ?? string.Empty;
                SignInPolicy = userSecretsService["SignInPolicy"] ?? string.Empty;
            }
            catch
            {
            }
        }
    }
}
