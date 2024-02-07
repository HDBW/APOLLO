// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Reflection;
using Apollo.Common.Entities;
using De.HDBW.Apollo.Data.Extensions;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class TrainingService : AbstractSwaggerServiceBase, ITrainingService
    {
        public TrainingService(ILogger<TrainingService> logger, string baseUrl, string authKey, HttpMessageHandler httpClientHandler)
               : base(logger, new Uri(new Uri($"{baseUrl.TrimEnd('/')}/"), $"{nameof(Training)}"), authKey, httpClientHandler)
        {
        }

        public async Task<IEnumerable<Training>> SearchSuggesionsAsync(Filter? filter, CancellationToken token)
        {
           var results = await SearchTrainingsAsync(filter, null, token);
           return results ?? Array.Empty<Training>();
        }

        public async Task<IEnumerable<Training>> SearchTrainingsAsync(Filter? filter, List<string>? visibleFields, CancellationToken token)
        {
            filter = filter ?? new Filter() { Fields = new List<FieldExpression>() };
            var sortExpression = new SortExpression() { FieldName = nameof(Training.TrainingName), Order = SortOrder.Descending };
            visibleFields = visibleFields ?? new List<string>() { nameof(Training.Id), nameof(Training.TrainingName) };
            var response = await GetTrainingsInternalAsync(filter, sortExpression, visibleFields, token).ConfigureAwait(false);
            return response ?? Array.Empty<Training>();
        }

        public async Task<Training?> GetTrainingAsync(long id, CancellationToken token)
        {
            var response = await GetTrainingInternalAsync(id, token).ConfigureAwait(false);
            return response;
        }

        private async Task<IEnumerable<Training>?> GetTrainingsInternalAsync(Filter filter, SortExpression sortExpression, List<string> visibleFields, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            filter.Fields = (filter.Fields != null)
                ? filter.Fields
                : new List<FieldExpression>();

            visibleFields = visibleFields != null
                ? visibleFields
                : new List<string>();

            var query = new Query();
            query.Filter = filter;
            query.SortExpression = sortExpression;
            query.Fields = visibleFields;
            var response = await DoPostAsync<QueryResponse>(query, token).ConfigureAwait(false);
            return response?.Trainings ?? Array.Empty<Training>();
        }

        private async Task<Training?> GetTrainingInternalAsync(long id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await DoGetAsync<Training>(id, token);
        }
    }
}
