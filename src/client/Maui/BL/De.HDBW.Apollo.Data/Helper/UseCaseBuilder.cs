﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace De.HDBW.Apollo.Data.Helper
{
    public class UseCaseBuilder : IUseCaseBuilder
    {
        public UseCaseBuilder(
            ILogger<UseCaseBuilder>? logger,
            IAssessmentItemRepository? assessmentItemRepository,
            IAssessmentCategoryRepository? assessmentCategoriesRepository,
            IAssessmentCategoryResultRepository? assessmentCategoryResultRepository,
            IAssessmentScoreRepository? assessmentScoreRepository,
            IQuestionItemRepository? questionItemRepository,
            IAnswerItemRepository? answerItemRepository,
            IAnswerItemResultRepository? answerItemResultRepository,
            IMetaDataMetaDataRelationRepository? metaDataMetaDataRelationRepository,
            IAnswerMetaDataRelationRepository? answerMetaDataRelationRepository,
            IQuestionMetaDataRelationRepository? questionMetaDataRelationRepository,
            IMetaDataRepository? metadataRepository,
            ICourseItemRepository? courseItemRepository,
            ICourseContactRepository? courseContactRepository,
            ICourseAppointmentRepository? courseAppointmentRepository,
            ICourseContactRelationRepository? courseContactRelationRepository,
            IEduProviderItemRepository? eduProviderItemRepository,
            ICategoryRecomendationItemRepository? categoryRecomendationItemRepository)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(assessmentItemRepository);
            ArgumentNullException.ThrowIfNull(assessmentCategoriesRepository);
            ArgumentNullException.ThrowIfNull(assessmentCategoryResultRepository);
            ArgumentNullException.ThrowIfNull(assessmentScoreRepository);
            ArgumentNullException.ThrowIfNull(questionItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemResultRepository);
            ArgumentNullException.ThrowIfNull(metaDataMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(answerMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(questionMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(metadataRepository);
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            ArgumentNullException.ThrowIfNull(courseContactRepository);
            ArgumentNullException.ThrowIfNull(courseAppointmentRepository);
            ArgumentNullException.ThrowIfNull(courseContactRelationRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);
            ArgumentNullException.ThrowIfNull(categoryRecomendationItemRepository);

            Logger = logger;
            AssessmentItemRepository = assessmentItemRepository;
            AssessmentCategoriesRepository = assessmentCategoriesRepository;
            AssessmentCategoryResultRepository = assessmentCategoryResultRepository;
            AssessmentScoreRepository = assessmentScoreRepository;
            QuestionItemRepository = questionItemRepository;
            AnswerItemRepository = answerItemRepository;
            AnswerItemResultRepository = answerItemResultRepository;
            MetaDataMetaDataRelationRepository = metaDataMetaDataRelationRepository;
            AnswerMetaDataRelationRepository = answerMetaDataRelationRepository;
            QuestionMetaDataRelationRepository = questionMetaDataRelationRepository;
            MetadataRepository = metadataRepository;
            CourseItemRepository = courseItemRepository;
            CourseContactRepository = courseContactRepository;
            CourseAppointmentRepository = courseAppointmentRepository;
            CourseContactRelationRepository = courseContactRelationRepository;
            EduProviderItemRepository = eduProviderItemRepository;
            CategoryRecomendationItemRepository = categoryRecomendationItemRepository;
        }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IAssessmentCategoryRepository AssessmentCategoriesRepository { get; }

        private IAssessmentCategoryResultRepository AssessmentCategoryResultRepository { get; }

        private IAssessmentScoreRepository AssessmentScoreRepository { get; }

        private IQuestionItemRepository QuestionItemRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IAnswerItemResultRepository AnswerItemResultRepository { get; }

        private IMetaDataMetaDataRelationRepository MetaDataMetaDataRelationRepository { get; }

        private IAnswerMetaDataRelationRepository AnswerMetaDataRelationRepository { get; }

        private IQuestionMetaDataRelationRepository QuestionMetaDataRelationRepository { get; }

        private IMetaDataRepository MetadataRepository { get; }

        private ICourseItemRepository CourseItemRepository { get; }

        private ICourseContactRepository CourseContactRepository { get; }

        private ICourseAppointmentRepository CourseAppointmentRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        private ICategoryRecomendationItemRepository CategoryRecomendationItemRepository { get; }

        private ICourseContactRelationRepository CourseContactRelationRepository { get; }

        private ILogger<UseCaseBuilder> Logger { get; }

        public async Task<bool> BuildAsync(UseCase usecase, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = true;
            try
            {
                await ClearAllRepositoriesAsync(token).ConfigureAwait(false);
                var fileName = string.Empty;
                switch (usecase)
                {
                    case UseCase.A:
                        fileName = "De.HDBW.Apollo.Data.SampleData.Usecase1.bin";
                        break;
                    case UseCase.B:
                        fileName = "De.HDBW.Apollo.Data.SampleData.Usecase2.bin";
                        break;
                    case UseCase.C:
                        fileName = "De.HDBW.Apollo.Data.SampleData.Usecase3.bin";
                        break;
                    default:
                        throw new NotSupportedException($"Usecase {usecase} is not supported by builder.");
                }

                result = await DeserializeSampleDataAndInitalizeRepositoriesAsync(fileName, token).ConfigureAwait(false);

                switch (usecase)
                {
                    default:
                        throw new NotSupportedException($"Usecase {usecase} is not supported by builder.");
                }
            }
            catch (Exception ex)
            {
                result = false;
                Logger?.LogError(ex, $"Unknown error while {nameof(BuildAsync)} in {GetType().Name}. Usecase was {usecase}");
            }

            return result;
        }

        private async Task<bool> DeserializeSampleDataAndInitalizeRepositoriesAsync(string fileName, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            UseCaseCollections usecase;
            using (var stream = GetType().Assembly.GetManifestResourceStream(fileName))
            {
                usecase = Serializer.Deserialize<UseCaseCollections>(stream);
            }

            await AnswerItemRepository.ResetItemsAsync(usecase?.AnswerItems, token).ConfigureAwait(false);
            await QuestionItemRepository.ResetItemsAsync(usecase?.QuestionItems, token).ConfigureAwait(false);
            await AssessmentItemRepository.ResetItemsAsync(usecase?.AssessmentItems, token).ConfigureAwait(false);
            await MetadataRepository.ResetItemsAsync(usecase?.MetaDataItems, token).ConfigureAwait(false);
            await AnswerMetaDataRelationRepository.ResetItemsAsync(usecase?.AnswerMetaDataRelations, token).ConfigureAwait(false);
            await QuestionMetaDataRelationRepository.ResetItemsAsync(usecase?.QuestionMetaDataRelations, token).ConfigureAwait(false);
            await MetaDataMetaDataRelationRepository.ResetItemsAsync(usecase?.MetaDataMetaDataRelations, token).ConfigureAwait(false);
            await EduProviderItemRepository.ResetItemsAsync(usecase?.EduProviderItems, token).ConfigureAwait(false);
            await CourseItemRepository.ResetItemsAsync(usecase?.CourseItems, token).ConfigureAwait(false);
            await CourseContactRepository.ResetItemsAsync(usecase?.CourseContacts, token).ConfigureAwait(false);
            await CourseAppointmentRepository.ResetItemsAsync(usecase?.CourseAppointments, token).ConfigureAwait(false);
            await AssessmentCategoriesRepository.ResetItemsAsync(usecase?.AssessmentCategories, token).ConfigureAwait(false);
            await CourseContactRelationRepository.ResetItemsAsync(usecase?.CourseContactRelations, token).ConfigureAwait(false);
            await AssessmentCategoryResultRepository.ResetItemsAsync(usecase?.AssessmentCategoryResults, token).ConfigureAwait(false);
            await AnswerItemResultRepository.ResetItemsAsync(usecase?.AnswerItemResults, token).ConfigureAwait(false);
            await CategoryRecomendationItemRepository.ResetItemsAsync(usecase?.CategoryRecomendations, token).ConfigureAwait(false);
            return true;
        }

        private async Task ClearAllRepositoriesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await AssessmentItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AssessmentCategoriesRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AssessmentCategoryResultRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AssessmentScoreRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await QuestionItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AnswerItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AnswerItemResultRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await MetaDataMetaDataRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AnswerMetaDataRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await QuestionMetaDataRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await MetadataRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await CourseItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await CourseContactRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await CourseAppointmentRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await EduProviderItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AssessmentCategoriesRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await CourseContactRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
        }
    }
}
