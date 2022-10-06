using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Helper
{
    public class UseCaseBuilder : IUseCaseBuilder
    {
        public UseCaseBuilder(ILogger<UseCaseBuilder> logger)
        {
            Logger = logger;
        }

        private ILogger<UseCaseBuilder> Logger { get; }

        public Task<bool> BuildAsync(UseCase usecase, CancellationToken token)
        {
            var result = true;
            try
            {
                // TODO: Generate Data for the usecases and store them to repositories
                switch (usecase)
                {
                    default:
                        throw new NotSupportedException($"Usecase {usecase} is not supported by builder.");
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while building usecase {usecase}");
            }

            return Task.FromResult(result);
        }
    }
}
