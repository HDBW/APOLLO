﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Numerics;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
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
            IAssessmentCategoryRepository assessmentCategoriesRepository,
            IAssessmentCategoryResultRepository assessmentCategoryResultRepository,
            IAssessmentScoreRepository assessmentScoresRepository)
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
            AssessmentCategoryResultsRepository = assessmentCategoryResultRepository;
            AssessmentScoresRepository = assessmentScoresRepository;
        }

        private ILogger? Logger { get; }

        private IUserProfileItemRepository UserProfileItemRepository { get; }

        private IQuestionItemRepository QuestionItemRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IAssessmentCategoryRepository AssessmentCategoriesRepository { get; }

        private IAssessmentCategoryResultRepository AssessmentCategoryResultsRepository { get; }

        private IAssessmentScoreRepository AssessmentScoresRepository { get; }

        public async Task<AssessmentScore> GetAssessmentScoreAsync(IEnumerable<AnswerItemResult> answerItemResults, CancellationToken token)
        {
            Logger?.Log(LogLevel.Information, new EventId(101, "GetAssessmentScore Called"), "{answerItems.Dump()}", answerItemResults);

            AssessmentScore score = new ();
            // TODO: Initialize score object we need id for category result

            List<AssessmentCategoryResult> results = new List<AssessmentCategoryResult>();

            token.ThrowIfCancellationRequested();

            var userProfiles = await UserProfileItemRepository.GetItemsAsync(token).ConfigureAwait(false);
            var userId = userProfiles.First().Id;

            // retrive assessmentId
            long assessmentId = answerItemResults.FirstOrDefault() !.AssessmentItemId;

            // TODO: Iterate over answerItems and create Category result
            var questions = await QuestionItemRepository.GetItemsByForeignKeyAsync(assessmentId, token).ConfigureAwait(false);
            IEnumerable<QuestionItem> questionItems = questions.ToList();
            Dictionary<long, int> maxScoreDictionary = questions.GroupBy(c => c.CategoryId)
                .ToDictionary(c => c.Key, c => c.Sum(s => s.Scalar));


            //set the score data now that we have some information
            score.AssessmentId = assessmentId;
            score.UserId = userId;
            score.Id = AssessmentScoresRepository.Count();
            score.Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");
            score.Ticks = DateTime.Now.Ticks;

            await AssessmentScoresRepository.AddItemAsync(score, token);

            var categories = await AssessmentCategoriesRepository.GetItemsByIdsAsync(questionItems.Select(x => x.CategoryId).Distinct(), token);


            foreach (var category in categories)
            {
                var categoryResult = new AssessmentCategoryResult();
                categoryResult.Id = AssessmentCategoryResultsRepository.Count();
                categoryResult.CategoryId = category.Id;
                categoryResult.Ticks = DateTime.Now.Ticks;
                categoryResult.Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");
                // NOTE: Set the AssessmentScoreId !!! IMPORTANTE !!!
                categoryResult.AssessmentScoreId = score.Id;
                // NOTE: !!! Important set the CourseId of the category to the result !!!
                categoryResult.CourseId = category.CourseId;
                var categoryQuestions = questions.Where(x => x.CategoryId.Equals(category.Id));
                Dictionary<long, int> userScores = new ();
                int scores = 0;

                foreach (var categoryQuestion in categoryQuestions)
                {
                    switch (categoryQuestion.QuestionType)
                    {
                        case QuestionType.Choice:
                            int expected = GetBitMaskFromString(categoryQuestion.ScoringOption, categoryQuestion.Id);


                            var categoryQuestionAnswers = await AnswerItemRepository.GetItemsByForeignKeysAsync(new List<long>{categoryQuestion.Id}, token);
                            string r = ToBinary(expected);

                            int userScore = 0x0000;
                            int userScoreIndex = categoryQuestionAnswers.Count() - 1;
                            foreach (AnswerItem answerItem in categoryQuestionAnswers)
                            {
                                //TODO: Check if there is a answeritem for the answer, compare results and calculate bitmask
                                var userAnswer = answerItemResults.Where(a => a.AnswerItemId.Equals(answerItem.Id));
                                if (userAnswer != null)
                                {
                                    // TODO: Check if this assumption is correct for multiple choice
                                    if (answerItem.Value.Equals("True") && answerItem.Value.Equals(userAnswer.First().Value))
                                    {
                                        userScore |= 1 << userScoreIndex;
                                    }
                                }
                                userScoreIndex--;
                            }

                            if (expected.Equals(userScore))
                            {
                                scores += categoryQuestion.Scalar;
                            }
                            break;
                        case QuestionType.Associate:
                            break;
                        case QuestionType.Rating:
                            //rating question atm has a different structure so we need to look out for the answers instead?
                            var userRatingAnswer = answerItemResults.First(a => a.QuestionItemId == categoryQuestion.Id);
                            // THIS IS THE EXPECTED CALCULATION HOWEVER SINCE QUESTIONS ARE ANSWERS ARE QUESTIONS IN THE CURRENT IMPLEMENTATION IN THE CLIENT WE NEED A ANOTHER WAY TO DO IT ...
                            // We assume there are many questions to this question so we check it out. Also note in this case there is just one question per category.
                            var AnswerItems = await AnswerItemRepository.GetItemsByForeignKeysAsync(new List<long> { categoryQuestion.Id }, token);

                            Dictionary<long, int> tmpDictionary = AnswerItems
                                                                        .GroupBy(c => c.QuestionId)
                                                                        .ToDictionary(ca => ca.Key,
                                                                                      ca => ca.Where(s => s.Scalar > 0)
                                                                                                                    .Sum(s => s.Scalar*5).GetValueOrDefault(0));

                            maxScoreDictionary= maxScoreDictionary.Union(tmpDictionary).ToDictionary(k => k.Key, v => v.Value);

                            foreach (AnswerItem item in AnswerItems)
                            {
                                // Now in this case the answers are questions!
                                var userAnswer = answerItemResults.Where(a => a.AnswerItemId.Equals(item.Id));
                                if (userAnswer != null && item != null)
                                {
                                    // The value of the answeritem should be the index which should be the indication vector, these are a lot of should bes will see.
                                    scores += Convert.ToInt32(userAnswer.First().Value) * Convert.ToInt32(item.Scalar)*5;
                                }
                            }
                            break;
                        case QuestionType.Unknown:
                            break;
                    }
                }

                categoryResult.Result = (100 * scores) / maxScoreDictionary[category.Id];
                results.Add(categoryResult); // REVIEW: DO WE NEED IT?
                await AssessmentCategoryResultsRepository.AddItemAsync(categoryResult, token);
            }

            // TODO: Calculate Assessment Score based on results and store that shit and be happy ^_^
            decimal yourscore = 0;
            foreach (AssessmentCategoryResult resultScores in results)
            {
                yourscore += resultScores.Result;
            }
            score.PercentageScore = Math.Round((decimal)(yourscore / results.Count));

            // NOTE: We need to update the score item
            // TODO: Update to Weighted average in future see: https://www.wikihow.com/Calculate-Weighted-Average
            await AssessmentScoresRepository.UpdateItemAsync(score, token);

            return score;
        }

        private int GetBitMaskFromString(string questionScoringOption, long questionId)
        {
            int vector = 0x0000;
            if (questionScoringOption != null)
            {
                string[] values = questionScoringOption.Split("-");
                int j = values.Length - 1;
                for (int i = 0; i < values.Length; i++)
                {
                    if (Convert.ToInt32(values[i]) == 1)
                    {
                        vector |= 1 << j;
                    }
                    j--;
                }
                return vector;
            }
            return vector;
        }

        public static string ToBinary(int x)
        {
            char[] buff = new char[32];

            for (int i = 31; i >= 0; i--)
            {
                int mask = 1 << i;
                buff[31 - i] = (x & mask) != 0 ? '1' : '0';
            }

            return new string(buff);
        }
    }
}
