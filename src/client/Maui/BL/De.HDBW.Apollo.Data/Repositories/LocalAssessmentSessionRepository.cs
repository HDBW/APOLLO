// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class LocalAssessmentSessionRepository :
        AbstractDataBaseRepository<LocalAssessmentSession>,
        ILocalAssessmentSessionRepository
    {
        public LocalAssessmentSessionRepository(
            IDataBaseConnectionProvider dataBaseConnectionProvider,
            ILogger<LocalAssessmentSessionRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<LocalAssessmentSession?> GetItemByAssessmentIdAndModuleIdAsync(string assessmentId, string moduleId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<LocalAssessmentSession>().FirstOrDefaultAsync(x => x.AssessmentId == assessmentId && x.ModuleId == moduleId).ConfigureAwait(false);
        }

        public async Task<LocalAssessmentSession?> GetItemBySessionIdAsync(string sessionId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<LocalAssessmentSession>().FirstOrDefaultAsync(x => x.SessionId == sessionId).ConfigureAwait(false);
        }

        public async Task<bool> RemoveItemBySessionIdAsync(string sessionId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<LocalAssessmentSession>().DeleteAsync(x => x.SessionId == sessionId).ConfigureAwait(false) == 1;
        }
    }
}
