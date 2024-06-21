// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace GrpcClient.Service
{
    public class AssessmentService : De.HDBW.Apollo.SharedContracts.Services.IAssessmentService
    {
        public async Task<IEnumerable<AssessmentTile>> GetTilesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return new List<AssessmentTile>()
            {
                new AssessmentTile()
                {
                    Deleted = false,
                    Type = AssessmentType.So,
                    Grouping = string.Empty,
                    ModuleId = string.Empty,
                    Title = ""
                }
            }
        }
    }
}
