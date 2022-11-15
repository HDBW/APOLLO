// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class AssessmentResultService : IAssessmentResultService
    {
        public AssessmentResultService(
            ILogger<AssessmentResultService>? logger,
            QuestionItemRepository? questionItemRepository,
            AnswerItemRepository? answerItemRepository,
            AssessmentCategoryRepository? assessmentCategoriesRepository)
        {
            Logger = logger;
            QuestionItemRepository = questionItemRepository;
            AnswerItemRepository = answerItemRepository;
            AssessmentCategoriesRepository = assessmentCategoriesRepository;
        }

        private QuestionItemRepository? QuestionItemRepository { get; }

        private AnswerItemRepository? AnswerItemRepository { get; }

        private AssessmentCategoryRepository? AssessmentCategoriesRepository { get; }

        private ILogger? Logger { get; }

        public Task<AssessmentScore> GetAssessmentScoreAsync(IEnumerable<AnswerItemResult> answerItems, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Logger?.Log(LogLevel.Information, new EventId(101, "GetAssessmentScore Called"), "{answerItems.Dump()}", answerItems);

            AssessmentScore score = new ();

            // TODO: Iterate over answerItems
            // TODO: Load Assessment CategoryResult
            // TODO: Create CategoryResults Collection
            AssessmentCategoryResult categoryResult = new AssessmentCategoryResult();

            // TODO: Calculate Assessment Score and be happy ^_^
            return Task.FromResult(score);
        }
    }
}
