// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AnswerItemRepository :
        AbstractDataBaseRepository<AnswerItem>,
        IAnswerItemRepository
    {
        public AnswerItemRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<AnswerItemRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<AnswerItem>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<AnswerItem>().Where(i => ids.Contains(i.QuestionId)).ToListAsync().ConfigureAwait(false);
        }
    }
}
