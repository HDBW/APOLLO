// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using Graph.Apollo.Cloud.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class QuestionItemRepository :
        AbstractInMemoryRepository<QuestionItem>,
        IQuestionItemRepository
    {
    }
}
