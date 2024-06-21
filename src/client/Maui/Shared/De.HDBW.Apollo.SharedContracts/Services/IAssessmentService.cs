// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IAssessmentService
    {
        Task<IEnumerable<AssessmentTile>> GetTilesAsync(CancellationToken token);
    }
}
