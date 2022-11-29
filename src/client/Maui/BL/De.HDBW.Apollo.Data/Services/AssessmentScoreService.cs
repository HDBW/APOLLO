// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class AssessmentScoreService : IAssessmentScoreService
    {
        public AssessmentScoreService(
            ILogger<AssessmentScoreService>? logger,
            IUserProfileItemRepository userProfileItemRepository,
            IQuestionItemRepository questionItemRepository,
            IAnswerItemRepository answerItemRepository,
            IAssessmentCategoryRepository assessmentCategoriesRepository)
        {
            ArgumentNullException.ThrowIfNull(userProfileItemRepository);
            ArgumentNullException.ThrowIfNull(questionItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemRepository);
            ArgumentNullException.ThrowIfNull(assessmentCategoriesRepository);
            Logger = logger;
            UserProfileItemRepository = userProfileItemRepository;
            QuestionItemRepository = questionItemRepository;
            AnswerItemRepository = answerItemRepository;
            AssessmentCategoriesRepository = assessmentCategoriesRepository;
        }

        private ILogger? Logger { get; }

        private IUserProfileItemRepository UserProfileItemRepository { get; }

        private IQuestionItemRepository QuestionItemRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IAssessmentCategoryRepository AssessmentCategoriesRepository { get; }

        public async Task<AssessmentScore> GetAssessmentScoreAsync(IEnumerable<AnswerItemResult> answerItemResults, CancellationToken token)
        {
            Logger?.Log(LogLevel.Information, new EventId(101, "GetAssessmentScore Called"), "{answerItems.Dump()}", answerItemResults);

            AssessmentScore score = new ();
            List<AssessmentCategoryResult> results = new List<AssessmentCategoryResult>();

            token.ThrowIfCancellationRequested();

            // retrive assessmentId
            long assessmentId = answerItemResults.FirstOrDefault()!.AssessmentItemId;

            // TODO: Iterate over answerItems and create Category result
            var questions = await QuestionItemRepository.GetItemsByForeignKeyAsync(assessmentId, token).ConfigureAwait(false);

            var userProfiles = await UserProfileItemRepository.GetItemsAsync(token).ConfigureAwait(false);
            var userId = userProfiles.First().Id;

            // iterate through the questions and check if there are answeritems for the questions
            foreach (QuestionItem question in questions)
            {
                // load question specific information just as answers and results
                var questionCategory = await AssessmentCategoriesRepository.GetItemByIdAsync(question.CategoryId, token).ConfigureAwait(false);
                List<long> questionIds = new () { question.Id };
                var answers = await AnswerItemRepository.GetItemsByForeignKeysAsync(questionIds, token).ConfigureAwait(false);

                // check if we have a answerItemResult for a question
                List<AnswerItemResult> usersResultsForQuestion =
                    answerItemResults.Where(a => a.QuestionItemId == question.Id).ToList();
                if (usersResultsForQuestion.Count > 0)
                {
                    // TODO: score based on vector x scalar?
                }
                else
                {
                    results.Add(new AssessmentCategoryResult
                    {
                        Category = questionCategory?.Id ?? -1,
                        Result = 0,
                        UserProfileId = userId,
                    });
                }
            }

            AssessmentCategoryResult categoryResult = new AssessmentCategoryResult();

            // TODO: Calculate Assessment Score based on results and store that shit and be happy ^_^
            return score;
        }
    }
}
