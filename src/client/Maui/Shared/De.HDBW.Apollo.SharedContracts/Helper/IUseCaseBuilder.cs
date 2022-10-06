using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public interface IUseCaseBuilder
    {
        Task<bool> BuildAsync(UseCase usecase, CancellationToken token);
    }
}
