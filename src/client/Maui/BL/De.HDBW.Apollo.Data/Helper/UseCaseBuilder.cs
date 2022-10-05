namespace De.HDBW.Apollo.Data.Helper
{
    using De.HDBW.Apollo.SharedContracts.Enums;
    using De.HDBW.Apollo.SharedContracts.Helper;
    using Microsoft.Extensions.Logging;

    public class UseCaseBuilder : IUseCaseBuilder
    {
        public UseCaseBuilder(ILogger<UseCaseBuilder> logger)
        {
            this.Logger = logger;
        }

        private ILogger<UseCaseBuilder> Logger { get; }

        public Task<bool> BuildAsync(UseCase usecase, CancellationToken token)
        {
            var result = true;
            try
            {
                switch (usecase)
                {
                    default:
                        throw new NotSupportedException($"Usecase {usecase} is not supported by builder.");
                }
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown error while building usecase {usecase}");
            }

            return Task.FromResult(result);
        }
    }
}
