// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Grpc.Net.Client;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Microsoft.Extensions.Logging;
using OccupationGrpcService.Protos;
using OccupationGrpcService.Services;
using ProtoBuf.Grpc.Client;

namespace GrpcClient.Service
{
    public class OccupationService : De.HDBW.Apollo.SharedContracts.Services.IOccupationService
    {
        public OccupationService(string address, ILogger<OccupationService>? logger)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(address);
            ArgumentNullException.ThrowIfNull(logger);
            Address = address;
            Logger = logger;
        }

        private string Address { get; }

        private ILogger? Logger { get; }

        public async Task<IEnumerable<OccupationTerm>> SearchAsync(string searchstring, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            IEnumerable<OccupationTerm> response = new List<OccupationTerm>();
            using (var channel = GrpcChannel.ForAddress(Address))
            {
                var occupationSuggestionClient = channel.CreateGrpcService<IOccupationSuggestionService>();
                try
                {
                    var request = new OccupationSuggestionRequest { Input = searchstring, CorrelationId = CreateCorrelationId() };
                    var result = await occupationSuggestionClient.GetOccupationSuggestions(request, token).ConfigureAwait(false);
                    response = result?.OccupationSuggestions.OfType<OccupationTerm>().Where(x => x != null).ToList<OccupationTerm>() ?? new List<OccupationTerm>();
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(SearchAsync)} in {GetType().Name}.");
                }
                finally
                {
                    await channel.ShutdownAsync();
                }
            }

            return response;
        }

        public async Task<Occupation?> CreateAsync(string searchstring, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Occupation? response = null;
            using (var channel = GrpcChannel.ForAddress(Address))
            {
                var occupationClient = channel.CreateGrpcService<OccupationGrpcService.Services.IOccupationService>();
                try
                {
                    var occupation = new Occupation();
                    occupation.CultureString = CultureInfo.CurrentCulture.Name;
                    occupation.TaxonomyInfo = Taxonomy.Unknown;
                    occupation.PreferedTerm = new List<string> { searchstring };
                    var request = new OccupationCreationRequest { Occupation = occupation, CorrelationId = CreateCorrelationId() };

                    var result = await occupationClient.CreateOrUpdate(request, token).ConfigureAwait(false);
                    response = result?.Occupation;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(CreateAsync)} in {GetType().Name}.");
                }
                finally
                {
                    await channel.ShutdownAsync();
                }
            }

            return response;
        }

        public async Task<Occupation?> GetItemByIdAsync(string id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Occupation? response = null;
            using (var channel = GrpcChannel.ForAddress(Address))
            {
                var occupationClient = channel.CreateGrpcService<OccupationGrpcService.Services.IOccupationService>();
                try
                {
                    var request = new OccupationRequest { Id = id, CorrelationId = CreateCorrelationId() };
                    var result = await occupationClient.GetOccupation(request, token).ConfigureAwait(false);
                    response = result?.Occupation;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(GetItemByIdAsync)} in {GetType().Name}.");
                }
                finally
                {
                    await channel.ShutdownAsync();
                }
            }

            return response;
        }

        private string CreateCorrelationId()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }
    }
}
