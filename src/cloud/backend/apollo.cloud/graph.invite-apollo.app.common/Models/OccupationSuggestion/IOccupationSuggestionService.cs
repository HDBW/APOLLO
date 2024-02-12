// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using OccupationGrpcService.Protos;

namespace OccupationGrpcService.Services
{
    [ServiceContract(Name = "OccupationTerms.SuggestionService")]
    public interface IOccupationSuggestionService
    {
        [OperationContract]
        Task<OccupationSuggestionResponse> GetOccupationSuggestions(OccupationSuggestionRequest request, CancellationToken token);
    }
}
