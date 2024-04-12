// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class QuestionItemRepository :
        AbstractDataBaseRepository<QuestionItem>,
        IQuestionItemRepository
    {
        public QuestionItemRepository(IDataBaseConnectionProvider dataBaseConnectionProvider, ILogger<QuestionItemRepository> logger)
            : base(dataBaseConnectionProvider, logger)
        {
        }

        public async Task<IEnumerable<QuestionItem>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);
            return await asyncConnection.Table<QuestionItem>().Where(i => i.AssessmentId == id).ToListAsync().ConfigureAwait(false);
        }
    }
}
