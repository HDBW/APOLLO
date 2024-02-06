// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Mock;
using De.HDBW.Apollo.SharedContracts.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;

namespace GrpcClient.Service
{
    public class OccupationSearchService : IOccupationSearchService
    {
        public OccupationSearchService(string address, ILogger<OccupationSearchService> logger)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(address);
            ArgumentNullException.ThrowIfNull(logger);
            Address = address;
            Logger = logger;
        }

        private string Address { get; }

        private ILogger? Logger { get; }

        public async Task<IEnumerable<OccupationTerm?>> SearchAsync(string searchstring, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            using (var channel = GrpcChannel.ForAddress(Address))
            {
                var occupationSuggestionClient = channel.CreateGrpcService<IOccupationSuggestionService>();
                try
                {
                    var request = new OccupationSuggestionRequest { Input = searchstring, CorrelationId = CreateCorrelationId() };
                    var result = await occupationSuggestionClient.GetOccupationSuggestions(request, token).ConfigureAwait(false);
                    return (result?.OccupationSuggestions ?? new List<OccupationTerm?>()).Where(x => x != null).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    await channel.ShutdownAsync();
                }
            }
        }

        private string CreateCorrelationId()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }
    }

    public interface IOccupationSuggestionService
    {
        Task<OccupationSuggestionResponse?> GetOccupationSuggestions(OccupationSuggestionRequest request, CancellationToken token);
    }
}
