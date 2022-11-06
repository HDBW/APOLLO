// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
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
            IQuestionItemRepository questiontItemRepository,
            IAnswerItemRepository answerItemRepository,
            IMetaDataMetaDataRelationRepository metaDataMetaDataRelationRepository,
            IAnswerMetaDataRelationRepository answerMetaDataRelationRepository,
            IQuestionMetaDataRelationRepository questionMetaDataRelationRepository,
            IMetaDataRepository metadataRepository,
            ICourseItemRepository courseItemRepository,
            IUserProfileRepository userProfileRepository,
            IEduProviderItemRepository eduProviderItemRepository)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(assessmentItemRepository);
            ArgumentNullException.ThrowIfNull(questiontItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemRepository);
            ArgumentNullException.ThrowIfNull(metaDataMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(answerMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(questionMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(metadataRepository);
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            ArgumentNullException.ThrowIfNull(userProfileRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);

            Logger = logger;
            AssessmentItemRepository = assessmentItemRepository;
            QuestiontItemRepository = questiontItemRepository;
            AnswerItemRepository = answerItemRepository;
            MetaDataMetaDataRelationRepository = metaDataMetaDataRelationRepository;
            AnswerMetaDataRelationRepository = answerMetaDataRelationRepository;
            QuestionMetaDataRelationRepository = questionMetaDataRelationRepository;
            MetadataRepository = metadataRepository;
            CourseItemRepository = courseItemRepository;
            UserProfileRepository = userProfileRepository;
            EduProviderItemRepository = eduProviderItemRepository;
        }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IQuestionItemRepository QuestiontItemRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IMetaDataMetaDataRelationRepository MetaDataMetaDataRelationRepository { get; }

        private IAnswerMetaDataRelationRepository AnswerMetaDataRelationRepository { get; }

        private IQuestionMetaDataRelationRepository QuestionMetaDataRelationRepository { get; }

        private IMetaDataRepository MetadataRepository { get; }

        private ICourseItemRepository CourseItemRepository { get; }

        private IUserProfileRepository UserProfileRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

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
                    default:
                        throw new NotSupportedException($"Usecase {usecase} is not supported by builder.");
                }

                result = await DeserializeSampleDataAndInitalizeRepositoriesAsync(fileName, token).ConfigureAwait(false);
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

            AnswerItemRepository.ResetItemsAsync(usecase.AnswerItems, token).ConfigureAwait(false);
            QuestiontItemRepository.ResetItemsAsync(usecase.QuestionItems, token).ConfigureAwait(false);
            AssessmentItemRepository.ResetItemsAsync(usecase.AssessmentItems, token).ConfigureAwait(false);
            MetadataRepository.ResetItemsAsync(usecase.MetaDataItems, token).ConfigureAwait(false);
            AnswerMetaDataRelationRepository.ResetItemsAsync(usecase.AnswerMetaDataRelations, token).ConfigureAwait(false);
            QuestionMetaDataRelationRepository.ResetItemsAsync(usecase.QuestionMetaDataRelations, token).ConfigureAwait(false);
            MetaDataMetaDataRelationRepository.ResetItemsAsync(usecase.MetaDataMetaDataRelations, token).ConfigureAwait(false);
            EduProviderItemRepository.ResetItemsAsync(usecase.EduProviderItems, token).ConfigureAwait(false);
            CourseItemRepository.ResetItemsAsync(usecase.CourseItems, token).ConfigureAwait(false);

            UserProfileRepository.AddItemAsync(new UserProfile() { Id = 1, FirstName = "Adrian", LastName = "Grafenberger", Image = "user1.png", Goal = "Jobsuche" }, token).ConfigureAwait(false);

            // UserProfileRepository.ResetItemsAsync(usecase.UserProfile, token).ConfigureAwait(false);
            return Task.FromResult(true);
        }

        private async Task ClearAllRepositoriesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await AssessmentItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await QuestiontItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AnswerItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await MetaDataMetaDataRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await AnswerMetaDataRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await QuestionMetaDataRelationRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await MetadataRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await CourseItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await EduProviderItemRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
            await UserProfileRepository.ResetItemsAsync(null, token).ConfigureAwait(false);
        }
    }
}
