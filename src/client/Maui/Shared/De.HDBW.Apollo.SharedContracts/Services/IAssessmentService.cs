// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IAssessmentService
    {
        Task<IEnumerable<AssessmentTile>> GetAssessmentTilesAsync(CancellationToken token);

        Task<Module> GetModuleAsync(string moduleId, string? language, CancellationToken token);

        Task<IEnumerable<ModuleTile>> GetModuleTilesAsync(IEnumerable<string> moduleIds, CancellationToken token);

        Task<LocalAssessmentSession?> CreateSessionAsync(string moduleId, string assessmentId, string? language, CancellationToken token);

        Task<LocalAssessmentSession?> GetSessionAsync(string sessionId, string? language, CancellationToken token);

        Task<bool> CancelSessionAsync(string sessionId, CancellationToken token);

        Task<Invite.Apollo.App.Graph.Common.Models.Assessments.RawData?> AnswerAsync(string sessionId, string rawDataId, double score, CancellationToken token);

        Task<bool> UpdateSessionAsync(LocalAssessmentSession session, CancellationToken token);
    }
}
