// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class AssessmentScoreService : IAssessmentScoreService
    {
        public AssessmentScoreService(
            ILogger<AssessmentScoreService>? logger,
            IQuestionItemRepository? questionItemRepository,
            IAnswerItemRepository? answerItemRepository,
            IAssessmentCategoryRepository? assessmentCategoriesRepository)
        {
            Logger = logger;
            QuestionItemRepository = questionItemRepository;
            AnswerItemRepository = answerItemRepository;
            AssessmentCategoriesRepository = assessmentCategoriesRepository;
        }

        private IQuestionItemRepository? QuestionItemRepository { get; }

        private IAnswerItemRepository? AnswerItemRepository { get; }

        private IAssessmentCategoryRepository? AssessmentCategoriesRepository { get; }

        private ILogger? Logger { get; }

        public Task<AssessmentScore> GetAssessmentScoreAsync(IEnumerable<AnswerItemResult> answerItemResults, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Logger?.Log(LogLevel.Information, new EventId(101, "GetAssessmentScore Called"), "{answerItems.Dump()}", answerItemResults);

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
