// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Apollo.Api
{
    /// <summary>
    /// Implements all Apollo Business functionalities.
    /// </summary>
    public partial class ApolloApi
    {
        private readonly IMongoCollection<Training> _trainingCollection;

        public ApolloApi()
        {
        }


        /// <summary>
        /// Gets the specific instance of the training.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public Task<Training> GetTraining(string trainingId)
        {
            try
            {
                _logger.LogTrace($"{this.User} entered {nameof(GetTraining)}");

                var res = Task.FromResult<Training>(new Training());

                _logger.LogTrace($"{this.User} completed {nameof(GetTraining)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.User} failed execution of {nameof(GetTraining)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of trainings based on a specified query filter.
        /// </summary>
        /// <param name="query">The query object containing filter criteria for the trainings.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of matching Training objects.</returns>
        public async Task<IList<Training>> QueryTrainings(Apollo.Common.Entities.Query query)
        {
            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

            var trainings = Convertor.ToEntityList<Training>(res, Convertor.ToTraining);

            return trainings;
        }

        /// <summary>
        /// Searches for trainings containing a specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search within the training names.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of Trainings matching the keyword.</returns>
        public async Task<IList<Training>> SearchTrainingsByKeyword(string keyword)
        {
            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { /* Fields to return */ },
                Filter = new Apollo.Common.Entities.Filter
                {
                    Fields = new List<FieldExpression> { new FieldExpression { FieldName = "TrainingName", Operator = QueryOperator.Contains, Argument = new List<object> { keyword } } }
                },
                Top = 100,
                Skip = 0
            };

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), null, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
            return Convertor.ToEntityList<Training>(res, Convertor.ToTraining);
        }

        /// <summary>
        /// Retrieves trainings within a specified date range.
        /// </summary>
        /// <param name="startDate">Start date of the range.</param>
        /// <param name="endDate">End date of the range.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of Trainings within the date range.</returns>
        public async Task<IList<Training>> QueryTrainingsByDateRange(DateTime startDate, DateTime endDate)
        {
            var query = new Apollo.Common.Entities.Query
            {
                Fields = new List<string> { /* Fields to return */ },
                Filter = new Apollo.Common.Entities.Filter
                {
                    Fields = new List<FieldExpression>
            {
                new FieldExpression { FieldName = "PublishingDate", Operator = QueryOperator.GreaterThanEqualTo, Argument = new List<object> { startDate } },
                new FieldExpression { FieldName = "PublishingDate", Operator = QueryOperator.LessThanEqualTo, Argument = new List<object> { endDate } }
            }
                },
                Top = 100,
                Skip = 0
            };

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), null, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
            return Convertor.ToEntityList<Training>(res, Convertor.ToTraining);
        }

        /// <summary>
        /// Counts the number of trainings provided by a specific provider.
        /// </summary>
        /// <param name="providerId">The identifier of the training provider.</param>
        /// <returns>Task that represents the asynchronous operation, containing the count of trainings.</returns>
        public async Task<int> CountTrainingsByProvider(string providerId)
        {
            var query = new Apollo.Common.Entities.Query
            {
                Filter = new Apollo.Common.Entities.Filter
                {
                    Fields = new List<FieldExpression> { new FieldExpression { FieldName = "ProviderId", Operator = QueryOperator.Equals, Argument = new List<object> { providerId } } },
                },
                Top = int.MaxValue, // may need to be ajusted to acual data size
                Skip = 0
            };

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), null, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
            return res.Count;
        }

        /// <summary>
        /// Retrieves a paginated list of trainings based on a specified query filter.
        /// </summary>
        /// <param name="query">The query object containing filter criteria for the trainings.</param>
        /// <param name="pageNumber">Page number for pagination.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of paginated Training objects.</returns>
        public async Task<IList<Training>> QueryTrainingsPaginated(Apollo.Common.Entities.Query query, int pageNumber, int pageSize)
        {
            query.Skip = (pageNumber - 1) * pageSize;
            query.Top = pageSize;

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
            var trainings = Convertor.ToEntityList<Training>(res, Convertor.ToTraining);

            return trainings;
        }

        // <summary>
        /// Retrieves a list of trainings based on a specified query filter, with custom fields selection.
        /// </summary>
        /// <param name="query">The query object containing filter criteria and field selections for the trainings.</param>
        /// <param name="selectedFields">A collection of field names to be included in the result set.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of Trainings with selected fields.</returns>
        public async Task<IList<Training>> QueryTrainingsWithCustomFields(Apollo.Common.Entities.Query query, IEnumerable<string> selectedFields)
        {
            if (selectedFields == null || !selectedFields.Any())
            {
                selectedFields = new List<string> { /* Default fields */ };
            }

            query.Fields = selectedFields.ToList();

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
            return Convertor.ToEntityList<Training>(res, training => Convertor.ToTraining(training));
        }


        public async Task<long> GetTotalTrainingCountAsync()
        {
            try
            {
                _logger.LogTrace($"{this.User} entered {nameof(GetTotalTrainingCountAsync)}");

                // Assuming you have a collection or data source of training entities, e.g., _trainingCollection
                var filter = Builders<Training>.Filter.Empty;
                var count = await _trainingCollection.CountDocumentsAsync(filter);

                _logger.LogTrace($"{this.User} completed {nameof(GetTotalTrainingCountAsync)}");

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.User} failed execution of {nameof(GetTotalTrainingCountAsync)}: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public Task<string> InsertTraining(Training training)
        {
            return Task.FromResult<string>(Guid.NewGuid().ToString());
        }


        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public Task<string> CreateOrUpdateTraining(Training training)
        {
            return Task.FromResult<string>(Guid.NewGuid().ToString());
        }


        /// <summary>
        /// Delete Trainings with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of training identifiers.</param>
        /// <returns>The numbe rof deleted trainings.</returns>
        public async Task<long> DeleteTrainings(string[] deletingIds)
        {
            var res = await _dal.DeleteManyAsync(GetCollectionName<Training>(), deletingIds);
            return res;
        }

    }
}
