﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace De.HDBW.Apollo.SharedContracts.Repositories
{
    public interface IAnswerItemRepository :
        IRepository<AnswerItem>,
        IDatabaseRepository<AnswerItem>
    {
        Task<IEnumerable<AnswerItem>> GetItemsByForeignKeysAsync(IEnumerable<long> ids, CancellationToken token);
    }
}