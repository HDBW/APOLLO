// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using OccupationGrpcService.Protos;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IOccupationService
    {
        Task<IEnumerable<OccupationTerm>> SearchAsync(string searchstring, CancellationToken token);

        Task<Occupation?> CreateAsync(string name, CancellationToken token);

        Task<Occupation?> GetItemByIdAsync(string occupationId, CancellationToken token);
    }
}
