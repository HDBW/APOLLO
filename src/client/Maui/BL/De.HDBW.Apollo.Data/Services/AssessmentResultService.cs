// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    internal class AssessmentResultService : IAssessmentResultService
    {
        private readonly QuestionItemRepository _questions;
        private readonly AnswerItemRepository _answers;
        private readonly AssessmentCategoriesRepository _categorieses;
        private readonly ILogger<AssessmentResultService> _logger;

        public AssessmentResultService(
            ILogger<AssessmentResultService>? logger,
            QuestionItemRepository questions,
            AnswerItemRepository answers,
            AssessmentCategoriesRepository categoriesRepository)
        {
            _logger = logger;
            _questions = questions;
            _answers = answers;
            _categorieses = categoriesRepository;
        }

        public AssessmentScore GetAssessmentScore(List<AnswerItemResult> answerItems)
        {
            _logger.Log(LogLevel.Information, new EventId(101, "GetAssessmentScore Called"),"{answerItems.Dump()}",answerItems);

            AssessmentScore score = new();

            // TODO: Iterate over answerItems
            // TODO: Load Assessment CategoryResult
            // TODO: Create CategoryResults Collection
            AssessmentCategoryResult categoryResult = new AssessmentCategoryResult();
            // TODO: Calculate Assessment Score and be happy ^_^
            return score;
        }
    }
}
