namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IUserSecretsService
    {
        string? this[string name]
        {
            get;
        }

        bool LoadSecrets();
    }
}
