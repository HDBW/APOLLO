// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IAssessmentService
    {
        Task<IEnumerable<AssessmentTile>> GetAssessmentTilesAsync(CancellationToken token);

        Task<object> GetModuleInstructionAsync(string moduleId, CancellationToken token);

        Task<IEnumerable<ModuleTile>> GetModuleTilesAsync(IEnumerable<string> moduleIds, CancellationToken token);
    }
}
