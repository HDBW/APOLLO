﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentCategoryResultRepository :
        AbstractDataBaseRepository<AssessmentCategoryResult>,
        IAssessmentCategoryResultRepository
    {
        public AssessmentCategoryResultRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<AssessmentCategoryResultRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<AssessmentCategoryResult>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<AssessmentCategoryResult>().Where(i => i.AssessmentScoreId == id).ToListAsync().ConfigureAwait(false);
        }
    }
}
