﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface ILocalAssessmentSessionRepository :
        IRepository<LocalAssessmentSession>,
        IDatabaseRepository<LocalAssessmentSession>
    {
        Task<LocalAssessmentSession?> GetItemByAssessmentIdAndModuleIdAsync(string assessmentId, string moduleId, CancellationToken token);

        Task<LocalAssessmentSession?> GetItemBySessionIdAsync(string sessionId, CancellationToken token);

        Task<bool> RemoveItemBySessionIdAsync(string sessionId, CancellationToken token);
    }
}
