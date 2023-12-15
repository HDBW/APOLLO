// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
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
            Mappings.Add(typeof(Training), typeof(Training).GetJSONMapping());
        }

        private Dictionary<Type, Dictionary<string, string>> Mappings { get; } = new Dictionary<Type, Dictionary<string, string>>();

        public async Task<IEnumerable<CourseItem>> SearchTrainingsAsync(Filter? filter, CancellationToken token)
        {
            filter = filter ?? new Filter() { Fields = new List<FieldExpression>() };
            SortExpression sortExpression = new SortExpression() { FieldName = nameof(Training.TrainingName), Order = SortOrder.Descending };
            List<string> visibleFields = typeof(Training).GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public).Select(p => p.Name).ToList();
            var response = await GetTrainingsInternalAsync(filter, sortExpression, visibleFields, token).ConfigureAwait(false);
            return response?.Select(x => x.ConvertToCourseItem()).ToList() as IEnumerable<CourseItem> ?? Array.Empty<CourseItem>();
        }

        public async Task<CourseItem?> GetTrainingAsync(long id, CancellationToken token)
        {
            var response = await GetTrainingInternalAsync(id, token).ConfigureAwait(false);
            return response?.ConvertToCourseItem();
        }

        private async Task<IEnumerable<Training>?> GetTrainingsInternalAsync(Filter filter, SortExpression sortExpression, List<string> visibleFields, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var mapping = Mappings[typeof(Training)];

            foreach (var field in filter.Fields)
            {
                field.FieldName = mapping.ContainsKey(field.FieldName) ? mapping[field.FieldName] : field.FieldName;
            }

            visibleFields = visibleFields.Select(f => mapping.ContainsKey(f) ? mapping[f] : f).ToList();
            sortExpression.FieldName = mapping.ContainsKey(sortExpression.FieldName) ? mapping[sortExpression.FieldName] : sortExpression.FieldName;

            var query = new Query();
            query.Filter = filter;
            query.SortExpression = sortExpression;
            query.Fields = visibleFields.Select(f => mapping.ContainsKey(f) ? mapping[f] : f).ToList();
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
