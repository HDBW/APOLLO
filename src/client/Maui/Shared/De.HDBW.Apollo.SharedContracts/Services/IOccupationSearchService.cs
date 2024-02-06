// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using De.HDBW.Apollo.SharedContracts.Mock;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IOccupationSearchService
    {
        Task<IEnumerable<OccupationTerm>?> SearchAsync(string searchstring, CancellationToken token);
    }
}
