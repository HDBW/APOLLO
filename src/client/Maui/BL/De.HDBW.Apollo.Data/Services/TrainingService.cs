// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Extensions;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public abstract class TrainingService : AbstractSwaggerServiceBase, ITrainingService
    {
        public TrainingService(ILoggerProvider logProvider,  string baseUrl, string authKey, HttpMessageHandler httpClientHandler)
               : base(logProvider.CreateLogger(nameof(TrainingService)), $"{baseUrl}/Training", authKey, httpClientHandler)
        {
        }

        public async Task<IEnumerable<CourseItem>> SearchTrainingsAsync(Filter? filter, CancellationToken token)
        {
            var trainings = await GetTrainingsInternalAsync(filter, token).ConfigureAwait(false);
            return trainings?.Select(x => x.ToCourseItem()).ToList() as IEnumerable<CourseItem> ?? Array.Empty<CourseItem>();
        }

        public async Task<CourseItem?> GetTrainingAsync(long id, CancellationToken token)
        {
            var training = await GetTrainingInternalAsync(id, token).ConfigureAwait(false);
            return training?.ToCourseItem();
        }

        private async Task<IEnumerable<Training>> GetTrainingsInternalAsync(Filter? filter, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            PostTrainingsResponse? result;
            using (var request = filter?.ToHttpContent())
            {
               result = await DoPostAsync<PostTrainingsResponse>(request, token).ConfigureAwait(false);
            }

            return result?.Trainings ?? Array.Empty<Training>();
        }

        private async Task<Training?> GetTrainingInternalAsync(long id,  CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var result = await DoGetAsync<GetTrainingResponse>(id, token);
            return result?.Training;
        }
    }
}
