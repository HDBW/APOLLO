// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Reflection;
using Apollo.Common.Entities;
using De.HDBW.Apollo.Data.Extensions;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.RestService.Messages;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class TrainingService : AbstractSwaggerServiceBase, ITrainingService
    {
        public TrainingService(ILogger<TrainingService> logger, string baseUrl, string authKey, HttpMessageHandler httpClientHandler)
               : base(logger, $"{baseUrl}/{nameof(Training)}", authKey, httpClientHandler)
        {
        }

        public async Task<IEnumerable<CourseItem>> SearchTrainingsAsync(Filter? filter, CancellationToken token)
        {
            filter = filter ?? new Filter() { Fields = new List<FieldExpression>() };
            SortExpression sortExpression = new SortExpression() { FieldName = nameof(Training.TrainingName), Order = SortOrder.Descending };
            List<string> visibleFields = typeof(Training).GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public).Select(p => p.Name).ToList();
            var response = await GetTrainingsInternalAsync(filter, sortExpression, visibleFields, token).ConfigureAwait(false);
            return response?.Trainings?.Select(x => x.ConvertToCourseItem()).ToList() as IEnumerable<CourseItem> ?? Array.Empty<CourseItem>();
        }

        public async Task<CourseItem?> GetTrainingAsync(long id, CancellationToken token)
        {
            var response = await GetTrainingInternalAsync(id, token).ConfigureAwait(false);
            return response?.ConvertToCourseItem();
        }

        private async Task<QueryTrainingsResponse?> GetTrainingsInternalAsync(Filter filter, SortExpression sortExpression, List<string> visibleFields, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var query = new Query();
            query.Filter = filter;
            query.SortExpression = sortExpression;
            query.Fields = visibleFields;
            return await DoPostAsync<QueryTrainingsResponse?>(query, token).ConfigureAwait(false);
        }

        private async Task<Training?> GetTrainingInternalAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await DoGetAsync<Training>(id, token);
        }
    }
}
