// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class TrainingService : AbstractSwaggerServiceBase, ITrainingService
    {
        public TrainingService(ILogger<TrainingService> logger, string baseUrl, string authKey, HttpMessageHandler httpClientHandler)
               : base(logger, new Uri(new Uri($"{baseUrl.TrimEnd('/')}/"), $"{nameof(Training)}"), authKey, httpClientHandler)
        {
        }

        public async Task<IEnumerable<Training>> SearchSuggesionsAsync(Filter? filter, int? skip, int? top, CancellationToken token)
        {
           var results = await SearchTrainingsAsync(filter, null, skip, top, token);
           return results ?? Array.Empty<Training>();
        }

        public async Task<IEnumerable<Training>> SearchTrainingsAsync(Filter? filter, List<string>? visibleFields, int? skip, int? top, CancellationToken token)
        {
            filter = filter ?? new Filter() { Fields = new List<FieldExpression>() };
            var sortExpression = new SortExpression() { FieldName = nameof(Training.TrainingName), Order = SortOrder.Descending };
            visibleFields = visibleFields ?? new List<string>() { nameof(Training.Id), nameof(Training.TrainingName) };
            var response = await GetTrainingsInternalAsync(filter, sortExpression, visibleFields, skip, top, token).ConfigureAwait(false);
            return response ?? Array.Empty<Training>();
        }

        public async Task<Training?> GetTrainingAsync(string id, CancellationToken token)
        {
            var response = await GetTrainingInternalAsync(id, token).ConfigureAwait(false);
            return response;
        }

        private async Task<IEnumerable<Training>?> GetTrainingsInternalAsync(Filter filter, SortExpression sortExpression, List<string> visibleFields, int? skip, int? top, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            filter.Fields = (filter.Fields != null)
                ? filter.Fields
                : new List<FieldExpression>();

            visibleFields = visibleFields != null
                ? visibleFields
                : new List<string>();

            var query = new QueryTrainingsRequest();
            query.Filter = filter;
            query.SortExpression = sortExpression;
            query.Fields = visibleFields;
            query.Skip = skip ?? 0;
            query.Top = top ?? 100;
            var response = await DoPostAsync<QueryTrainingsResponse>(query, token).ConfigureAwait(false);
            return response?.Trainings ?? new List<Training>();
        }

        private async Task<Training?> GetTrainingInternalAsync(string id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await DoGetAsync<Training>(id, token);
        }
    }
}
