#region (c) Licensed to the HDBW under one or more agreements.\nThe HDBW licenses this file to you under the MIT license.

// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

#endregion

using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IAssessmentCategoryResultRepository :
        IRepository<AssessmentCategoryResult>,
        IDatabaseRepository<AssessmentCategoryResult>
    {
        /// <summary>
        /// Returns all AssessmentCategoryResults for a specific Question Category.
        /// </summary>
        /// <param name="id">QuestionCategory</param>
        /// <param name="token">Cancellation Token</param>
        /// <returns>List of AssessmentCategoryResults</returns>
        Task<IEnumerable<AssessmentCategoryResult>> GetItemsByForeignKeyAsync(long id, CancellationToken token);
    }
}
