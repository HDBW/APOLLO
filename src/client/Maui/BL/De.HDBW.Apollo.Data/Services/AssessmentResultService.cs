// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    internal class AssessmentResultService : IAssessmentResultService
    {
        private readonly IQuestionItemRepository _questions;
        private readonly IAnswerItemRepository _answers;
        private readonly IAssessmentCategoriesRepository _categories;
        private readonly ILogger<AssessmentResultService> _logger;

        public AssessmentResultService(
            ILogger<AssessmentResultService>? logger,
            IQuestionItemRepository questions,
            IAnswerItemRepository answers,
            IAssessmentCategoriesRepository categoriesRepository)
        {
            _logger = logger;
            _questions = questions;
            _answers = answers;
            _categories = categoriesRepository;
        }

        public async Task<AssessmentScore> GetAssessmentScore(List<AnswerItemResult> answerItems, CancellationToken token)
        {
            _logger.Log(LogLevel.Information, new EventId(101, "GetAssessmentScore Called"),"{answerItems.Dump()}",answerItems);

            AssessmentScore score = new();
            List<AssessmentCategoryResult> results = new List<AssessmentCategoryResult>();

            token.ThrowIfCancellationRequested();

            //retrive assessmentId
            long assessmentId = answerItems.FirstOrDefault()!.AssessmentItemId;

            // TODO: Iterate over answerItems and create Category result
            var questions = await _questions.GetItemsByForeignKeyAsync(assessmentId, token);

            //iterate through the questions and check if there are answeritems for the questions
            foreach (QuestionItem question in questions)
            {
                //load question specific information just as answers and results
                var questionCategory = await _categories.GetItemByIdAsync(question.Category, token);
                List<long> questionIds = new() { question.Id };
                var answers = await _answers.GetItemsByForeignKeysAsync(questionIds, token);

                //check if we have a answerItemResult for a question
                List<AnswerItemResult> usersResultsForQuestion =
                    answerItems.Where(a => a.QuestionItemId == question.Id).ToList();
                if (usersResultsForQuestion.Count>0)
                {
                    //TODO: score based on vector x scalar?
                }
                else
                {
                    results.Add(new AssessmentCategoryResult
                    {
                        Category = questionCategory.Id,
                        Result = 0,
                        //UserProfileId = TODO: Need User ProfileId here ^_^
                    });
                }
            }
            AssessmentCategoryResult categoryResult = new AssessmentCategoryResult();
            // TODO: Calculate Assessment Score based on results and store that shit and be happy ^_^
            return score;
        }
    }
}
