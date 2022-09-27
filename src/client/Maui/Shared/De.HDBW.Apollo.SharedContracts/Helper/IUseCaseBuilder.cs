namespace De.HDBW.Apollo.SharedContracts.Helper
{
    using De.HDBW.Apollo.SharedContracts.Enums;

    public interface IUseCaseBuilder
    {
        Task<bool> BuildAsync(UseCase usecase, CancellationToken token);
    }
}
