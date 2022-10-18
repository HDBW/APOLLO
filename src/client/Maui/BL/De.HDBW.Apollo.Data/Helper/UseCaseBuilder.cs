// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Reflection;
using System.Xml.Linq;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;

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
            IMetaDataRepository metadataRepository)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(assessmentItemRepository);
            ArgumentNullException.ThrowIfNull(questiontItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemRepository);
            ArgumentNullException.ThrowIfNull(metaDataMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(answerMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(questionMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(metadataRepository);

            Logger = logger;
            AssessmentItemRepository = assessmentItemRepository;
            QuestiontItemRepository = questiontItemRepository;
            AnswerItemRepository = answerItemRepository;
            MetaDataMetaDataRelationRepository = metaDataMetaDataRelationRepository;
            AnswerMetaDataRelationRepository = answerMetaDataRelationRepository;
            QuestionMetaDataRelationRepository = questionMetaDataRelationRepository;
            MetadataRepository = metadataRepository;
        }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IQuestionItemRepository QuestiontItemRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IMetaDataMetaDataRelationRepository MetaDataMetaDataRelationRepository { get; }

        private IAnswerMetaDataRelationRepository AnswerMetaDataRelationRepository { get; }

        private IQuestionMetaDataRelationRepository QuestionMetaDataRelationRepository { get; }

        private IMetaDataRepository MetadataRepository { get; }

        private ILogger<UseCaseBuilder> Logger { get; }

        public async Task<bool> BuildAsync(UseCase usecase, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = true;
            try
            {
                await ClearAllRepositoriesAsync(token).ConfigureAwait(false);
                var fileName = string.Empty;
                // TODO: Load bin file and de
                switch (usecase)
                {
                    case UseCase.A:
                        fileName = "???";
                        break;
                    default:
                        throw new NotSupportedException($"Usecase {usecase} is not supported by builder.");
                }

                result = await DeserializeSampleDataAndInitalizeRepositoriesAsync("????", token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                result = false;
                Logger?.LogError(ex, $"Unknown error while building usecase {usecase}");
            }

            return result;
        }

        private Task<bool> DeserializeSampleDataAndInitalizeRepositoriesAsync(string fileName, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // TODO: Read bin file from ManifestResourceStream and fill the respoitories.
            // AnswerItemRepository.ResetItemsAsync(items, token).ConfigureAwait(false);
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
        }
    }
}
