// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace De.HDBW.Apollo.Data.Helper
{
    public class UseCaseBuilder : IUseCaseBuilder
    {
        public UseCaseBuilder(
            ILogger<UseCaseBuilder> logger,
            IAssessmentItemRepository assessmentItemRepository,
            IAssessmentCategoryRepository assessmentCategoriesRepository,
            IAssessmentCategoryResultRepository assessmentCategoryResultRepository,
            IQuestionItemRepository questionItemRepository,
            IAnswerItemRepository answerItemRepository,
            IAnswerItemResultRepository answerItemResultRepository,
            IMetaDataMetaDataRelationRepository metaDataMetaDataRelationRepository,
            IAnswerMetaDataRelationRepository answerMetaDataRelationRepository,
            IQuestionMetaDataRelationRepository questionMetaDataRelationRepository,
            IMetaDataRepository metadataRepository,
            ICourseItemRepository courseItemRepository,
            ICourseContactRepository courseContactRepository,
            ICourseAppointmentRepository courseAppointmentRepository,
            ICourseContactRelationRepository courseContactRelationRepository,
            IUserProfileItemRepository userProfileItemRepository,
            IEduProviderItemRepository eduProviderItemRepository)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(assessmentItemRepository);
            ArgumentNullException.ThrowIfNull(assessmentCategoriesRepository);
            ArgumentNullException.ThrowIfNull(assessmentCategoryResultRepository);
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
            ArgumentNullException.ThrowIfNull(userProfileItemRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);

            Logger = logger;
            AssessmentItemRepository = assessmentItemRepository;
            AssessmentCategoriesRepository = assessmentCategoriesRepository;
            AssessmentCategoryResultRepository = assessmentCategoryResultRepository;
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
            UserProfileItemRepository = userProfileItemRepository;
            EduProviderItemRepository = eduProviderItemRepository;
        }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IAssessmentCategoryRepository AssessmentCategoriesRepository { get; }

        private IAssessmentCategoryResultRepository AssessmentCategoryResultRepository { get; }

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

        private IUserProfileItemRepository UserProfileItemRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

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
                    case UseCase.A:
                        await UserProfileItemRepository.AddItemAsync(new UserProfileItem() { Id = 1, FirstName = "Adrian", LastName = "Grafenberger", Image = "user1.png", Goal = "Job finden" }, token).ConfigureAwait(false);
                        break;
                    case UseCase.B:
                        await UserProfileItemRepository.AddItemAsync(new UserProfileItem() { Id = 1, FirstName = "Kerstin", LastName = string.Empty, Image = "user2.png", Goal = "Weiterbildung" }, token).ConfigureAwait(false);
                        break;
                    case UseCase.C:
                        await UserProfileItemRepository.AddItemAsync(new UserProfileItem() { Id = 1, FirstName = "Arwa", LastName = string.Empty, Image = "user3.png", Goal = "Karriereaufstieg" }, token).ConfigureAwait(false);
                        break;
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

        private Task<bool> DeserializeSampleDataAndInitalizeRepositoriesAsync(string fileName, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            UseCaseCollections usecase;
            using (var stream = GetType().Assembly.GetManifestResourceStream(fileName))
            {
                usecase = Serializer.Deserialize<UseCaseCollections>(stream);
            }

            AnswerItemRepository.ResetItemsAsync(usecase?.AnswerItems, token).ConfigureAwait(false);
            QuestionItemRepository.ResetItemsAsync(usecase?.QuestionItems, token).ConfigureAwait(false);
            AssessmentItemRepository.ResetItemsAsync(usecase?.AssessmentItems, token).ConfigureAwait(false);
            MetadataRepository.ResetItemsAsync(usecase?.MetaDataItems, token).ConfigureAwait(false);
            AnswerMetaDataRelationRepository.ResetItemsAsync(usecase?.AnswerMetaDataRelations, token).ConfigureAwait(false);
            QuestionMetaDataRelationRepository.ResetItemsAsync(usecase?.QuestionMetaDataRelations, token).ConfigureAwait(false);
            MetaDataMetaDataRelationRepository.ResetItemsAsync(usecase?.MetaDataMetaDataRelations, token).ConfigureAwait(false);
            EduProviderItemRepository.ResetItemsAsync(usecase?.EduProviderItems, token).ConfigureAwait(false);
            CourseItemRepository.ResetItemsAsync(usecase?.CourseItems, token).ConfigureAwait(false);
            CourseContactRepository.ResetItemsAsync(usecase?.CourseContacts, token).ConfigureAwait(false);
            CourseAppointmentRepository.ResetItemsAsync(usecase?.CourseAppointments, token).ConfigureAwait(false);
            AssessmentCategoriesRepository.ResetItemsAsync(usecase?.AssessmentCategories, token).ConfigureAwait(false);
            CourseContactRelationRepository.ResetItemsAsync(usecase?.CourseContactRelations, token).ConfigureAwait(false);

            // AssessmentCategoryResultRepository.ResetItemsAsync(usecase?.AssessmentCategoryResults, token).ConfigureAwait(false);
            // AnswerItemResultRepository.ResetItemsAsync(usecase.AnswerItemResults, token).ConfigureAwait(false);
            return Task.FromResult(true);
        }

        private async Task ClearAllRepositoriesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await AssessmentItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AssessmentCategoriesRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AssessmentCategoryResultRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
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
            await UserProfileItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AssessmentCategoriesRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await CourseContactRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
        }
    }
}
