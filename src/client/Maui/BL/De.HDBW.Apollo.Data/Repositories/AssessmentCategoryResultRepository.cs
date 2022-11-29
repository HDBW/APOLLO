// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace De.HDBW.Apollo.Data.Repositories
{
    public class AssessmentCategoryResultRepository :
        AbstractInMemoryRepository<AssessmentCategoryResult>,
        IAssessmentCategoryResultRepository
    {
        public AssessmentCategoryResultRepository(ILogger<QuestionItemRepository> logger)
            : base(logger)
        {
        }

        public Task<IEnumerable<AssessmentCategoryResult>> GetItemsByForeignKeyAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new ReadOnlyCollection<AssessmentCategoryResult>(Items?.Where(i => i.Category == id).ToList() ?? new List<AssessmentCategoryResult>()) as IEnumerable<AssessmentCategoryResult>);
        }
    }
}
