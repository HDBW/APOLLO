// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IAssessmentService
    {
        Task<AssessmentSession> CreateSessionAsync(string? moduleId, CancellationToken token);

        Task<IEnumerable<AssessmentTile>> GetAssessmentTilesAsync(CancellationToken token);

        Task<Module> GetModuleAsync(string moduleId, string? language, CancellationToken token);

        Task<IEnumerable<ModuleTile>> GetModuleTilesAsync(IEnumerable<string> moduleIds, CancellationToken token);

        Task<AssessmentSession> GetSessionAsync(string sessionId, string? language, CancellationToken token);
    }
}
