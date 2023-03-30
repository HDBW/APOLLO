// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using SQLite;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AnswerItemResultRepository :
        AbstractDataBaseRepository<AnswerItemResult>,
        IAnswerItemResultRepository
    {
        public AnswerItemResultRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<AnswerItemResultRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public Task<IEnumerable<AnswerItemResult>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            return GetItemsByForeignKeysAsync(new List<long>() { id }, token);
        }

        public async Task<IEnumerable<AnswerItemResult>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<AnswerItemResult>().Where(i => ids.Contains(i.AssessmentItemId)).ToListAsync().ConfigureAwait(false);
        }
    }
}
