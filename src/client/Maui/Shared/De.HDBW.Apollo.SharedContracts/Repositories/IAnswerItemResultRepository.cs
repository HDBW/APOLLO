// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IAnswerItemResultRepository :
        IRepository<AnswerItemResult>,
        IDatabaseRepository<AnswerItemResult>
    {
        Task<IEnumerable<AnswerItemResult>> GetItemsByForeignKeyAsync(long id, CancellationToken token);

        Task<IEnumerable<AnswerItemResult>> GetItemsByForeignKeysAsync(IEnumerable<long> list, CancellationToken token);
    }
}
