// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Linq;
using Apollo.Common.Entities;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class FakeTrainingService : ITrainingService
    {
        private readonly Dictionary<Type, Dictionary<string, string>> _mappings = new Dictionary<Type, Dictionary<string, string>>();

        public FakeTrainingService(
            ILogger<FakeTrainingService> logger,
            ICourseItemRepository courseItemRepository,
            ICourseContactRepository courseContactRepository,
            ICourseAppointmentRepository courseAppointmentRepository,
            ICourseContactRelationRepository courseContactRelationRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            IDataBaseConnectionProvider dataBaseConnectionProvider)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            ArgumentNullException.ThrowIfNull(courseContactRepository);
            ArgumentNullException.ThrowIfNull(courseAppointmentRepository);
            ArgumentNullException.ThrowIfNull(courseContactRelationRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);
            ArgumentNullException.ThrowIfNull(dataBaseConnectionProvider);
            Logger = logger;
            CourseItemRepository = courseItemRepository;
            CourseContactRepository = courseContactRepository;
            CourseAppointmentRepository = courseAppointmentRepository;
            CourseContactRelationRepository = courseContactRelationRepository;
            EduProviderItemRepository = eduProviderItemRepository;
            DataBaseConnectionProvider = dataBaseConnectionProvider;

            _mappings.Add(typeof(CourseItem), new Dictionary<string, string>()
            {
                { nameof(Training.TrainingName), nameof(CourseItem.Title) },
                { nameof(Training.Description), nameof(CourseItem.Description) },
                { nameof(Training.ShortDescription), nameof(CourseItem.ShortDescription) },
                { nameof(Training.BenefitList), nameof(CourseItem.Benefits) },
                { nameof(Training.TargetAudience), nameof(CourseItem.TargetGroup) },
                { nameof(Training.TrainingType), nameof(CourseItem.CourseType) },
                { nameof(Training.Price), nameof(CourseItem.Price) },
                { nameof(Training.Tags), nameof(CourseItem.CourseTagType) },
            });

            _mappings.Add(typeof(EduProvider), new Dictionary<string, string>()
            {
                { nameof(Training.CourseProvider), nameof(EduProvider.Name) },
                { nameof(Training.TrainingProvider), nameof(EduProvider.Name) },
            });
        }

        private ILogger Logger { get; }

        private ICourseItemRepository CourseItemRepository { get; }

        private ICourseContactRepository CourseContactRepository { get; }

        private ICourseAppointmentRepository CourseAppointmentRepository { get; }

        private ICourseContactRelationRepository CourseContactRelationRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        private IDataBaseConnectionProvider DataBaseConnectionProvider { get; }

        public Task<CourseItem?> GetTrainingAsync(long id, CancellationToken token)
        {
            return CourseItemRepository.GetItemByIdAsync(id, token);
        }

        public async Task<IEnumerable<string>> SearchSuggesionsAsync(Filter? filter, CancellationToken token)
        {
            var maxItems = 5;
            IEnumerable<string> result = new List<string>();
            if (filter == null)
            {
                var queryResult = await CourseItemRepository.GetItemsAsync(token).ConfigureAwait(false);
                result = queryResult.Take(maxItems).Select(x => x.Title).ToList();
                return result;
            }

            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);

            var expressions = new List<QueryExpression?>();
            foreach (var field in filter.Fields)
            {
                expressions.Add(GetQueryExpressions(typeof(CourseItem), field));
            }

            expressions = expressions.Where(x => x != null && !string.IsNullOrWhiteSpace(x.Query)).ToList();
            if (expressions.Any())
            {
                var query = $"SELECT * FROM CourseItem WHERE {string.Join(filter.IsOrOperator ? " OR " : " AND ", expressions.Select(e => e!.Query))}";
                var queryResult = await asyncConnection.QueryAsync<CourseItem>(query, expressions.Select(x => x!.Parameter).ToArray());
                result = result.Union(queryResult.Select(x => x.Title));
            }

            expressions = new List<QueryExpression?>();
            foreach (var field in filter.Fields)
            {
                expressions.Add(GetQueryExpressions(typeof(EduProvider), field));
            }

            expressions = expressions.Where(x => x != null && !string.IsNullOrWhiteSpace(x.Query)).ToList();
            if (expressions.Any())
            {
                var query = $"SELECT * FROM EduProviderItem WHERE {string.Join(filter.IsOrOperator ? " OR " : " AND ", expressions.Select(e => e!.Query))}";
                var provider = await asyncConnection.QueryAsync<EduProviderItem>(query, expressions.Select(x => x!.Parameter).ToArray());
                result = result.Union(provider.Select(p => p.Name).Distinct());
            }

            return result.Distinct().Take(maxItems);
        }

        public async Task<IEnumerable<CourseItem>> SearchTrainingsAsync(Filter? filter, CancellationToken token)
        {
            IEnumerable<CourseItem> result = new List<CourseItem>();
            if (filter == null)
            {
                result = await CourseItemRepository.GetItemsAsync(token).ConfigureAwait(false);
                result = result.ToList();
                return result;
            }

            var asyncConnection = await DataBaseConnectionProvider.GetConnectionAsync(token).ConfigureAwait(false);

            var expressions = new List<QueryExpression?>();
            foreach (var field in filter.Fields)
            {
                expressions.Add(GetQueryExpressions(typeof(CourseItem), field));
            }

            expressions = expressions.Where(x => x != null && !string.IsNullOrWhiteSpace(x.Query)).ToList();
            if (expressions.Any())
            {
                var query = $"SELECT * FROM CourseItem WHERE {string.Join(filter.IsOrOperator ? " OR " : " AND ", expressions.Select(e => e!.Query))}";
                result = await asyncConnection.QueryAsync<CourseItem>(query, expressions.Select(x => x!.Parameter).ToArray());
            }

            expressions = new List<QueryExpression?>();
            foreach (var field in filter.Fields)
            {
                expressions.Add(GetQueryExpressions(typeof(EduProvider), field));
            }

            expressions = expressions.Where(x => x != null && !string.IsNullOrWhiteSpace(x.Query)).ToList();
            if (expressions.Any())
            {
                var query = $"SELECT * FROM EduProviderItem WHERE {string.Join(filter.IsOrOperator ? " OR " : " AND ", expressions.Select(e => e!.Query))}";
                var provider = await asyncConnection.QueryAsync<EduProviderItem>(query, expressions.Select(x => x!.Parameter).ToArray());
                var providerIds = provider?.Select(x => x.Id).ToArray() ?? Array.Empty<long>();
                var subExpressions = new List<QueryExpression>();
                foreach (var expression in expressions)
                {
                    switch (expression!.FieldName)
                    {
                        case nameof(Training.CourseProvider):
                            subExpressions.Add(new QueryExpression(nameof(CourseItem.CourseProviderId), $"{nameof(CourseItem.CourseProviderId)} In (?)", providerIds));
                            break;

                        case nameof(Training.TrainingProvider):
                            subExpressions.Add(new QueryExpression(nameof(CourseItem.TrainingProviderId), $"{nameof(CourseItem.TrainingProviderId)} In (?)", providerIds));
                            break;
                    }
                }

                query = $"SELECT * FROM CourseItem WHERE {string.Join(filter.IsOrOperator ? " OR " : " AND ", subExpressions.Select(e => e.Query))}";
                query = query.Replace("?", string.Join(",", providerIds));
                var subResult = await asyncConnection.QueryAsync<CourseItem>(query);
                result = result.Union(subResult);
            }

            return result.DistinctBy(x => x.Id);
        }

        record QueryExpression
        {
            public QueryExpression(string fieldName, string query, object parameter)
            {
                Query = query;
                Parameter = parameter;
                FieldName = fieldName;
            }

            public string Query { get; }

            public string FieldName { get; }

            public object Parameter { get; }
        }

        private QueryExpression? GetQueryExpressions(Type source, FieldExpression expression)
        {
            if (!(expression?.Argument?.Any() ?? false) || !(_mappings[source]?.ContainsKey(expression.FieldName) ?? false))
            {
                return null;
            }

            var fieldName = _mappings[source][expression.FieldName];
            switch (expression.Operator)
            {
                case QueryOperator.Equals:
                    return new QueryExpression(expression.FieldName, $"{fieldName} = ?", expression.Argument.Single());
                case QueryOperator.Contains:
                    return new QueryExpression(expression.FieldName, $"{fieldName} LIKE ?", $"%{expression.Argument.Single()}%");
                case QueryOperator.StartsWith:
                    return new QueryExpression(expression.FieldName, $"{fieldName} LiKE ?", $"{expression.Argument.Single()}%");
                case QueryOperator.GreaterThan:
                    return new QueryExpression(expression.FieldName, $"{fieldName} > ?", $"{expression.Argument.Single()}");
                case QueryOperator.LessThan:
                    return new QueryExpression(expression.FieldName, $"{fieldName} < ?", $"{expression.Argument.Single()}");
                case QueryOperator.NotEquals:
                    return new QueryExpression(expression.FieldName, $"{fieldName} <> ?", $"{expression.Argument.Single()}");
                case QueryOperator.In:
                    return new QueryExpression(expression.FieldName, $"{fieldName} In (?)", $"{string.Join(',', expression.Argument)}");
                case QueryOperator.Empty:
                    return new QueryExpression(expression.FieldName, $"{fieldName} NULL", DBNull.Value);
                case QueryOperator.GreaterThanEqualTo:
                    return new QueryExpression(expression.FieldName, $"{fieldName} >= ?", $"{expression.Argument.Single()}");
                case QueryOperator.LessThanEqualTo:
                    return new QueryExpression(expression.FieldName, $"{fieldName} <= ?", $"{expression.Argument.Single()}");
            }

            return null;
        }
    }
}
