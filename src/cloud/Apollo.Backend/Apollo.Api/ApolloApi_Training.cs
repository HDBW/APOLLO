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

        public ApolloApi()//todo remove constructor.
        {
        }


        /// <summary>
        /// Gets the specific instance of the training.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public virtual async Task<Training> GetTraining(string trainingId)
        {
            try
            {
                _logger.LogTrace($"Entered {nameof(GetTraining)}");

                var training = await _dal.GetByIdAsync<Training>(ApolloApi.GetCollectionName<Training>(), trainingId);

                _logger.LogTrace($"Completed {nameof(GetTraining)}");

                return training;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed execution of {nameof(GetTraining)}: {ex.Message}");
                throw;
                throw new ApolloApiException(ErrorCodes.TrainingErrors.GetTrainingError, "Error while getting training", ex);
            }
        }


        /// <summary>
        /// Retrieves a list of trainings based on a specified query filter.
        /// </summary>
        /// <param name="query">The query object containing filter criteria for the trainings.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of matching Training objects.</returns>
        public virtual async Task<IList<Training>> QueryTrainings(Apollo.Common.Entities.Query query)
        {
            try
            {
                _logger.LogTrace($"{this.User} entered {nameof(QueryTrainings)}");

                // Execute the query 
                var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

                //  convert  results to a list of typed Training objects
                var trainings = Convertor.ToEntityList<Training>(res, Convertor.ToTraining);

                _logger.LogTrace($"{this.User} completed {nameof(QueryTrainings)}");

                return trainings;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, $"{this.User} failed execution of {nameof(QueryTrainings)}: {ex.Message}");

                // Throw an ApolloApiException with the specific error code
                throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsError, "Error while querying trainings", ex);
            }
        }


        /// <summary>
        /// Searches for trainings containing a specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search within the training names.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of Trainings matching the keyword.</returns>
        //public async Task<IList<Training>> SearchTrainingsByKeyword(string keyword)
        //{
        //    try
        //    {
        //        var query = new Apollo.Common.Entities.Query
        //        {
        //            Fields = new List<string> { /* Fields to return */ },
        //            Filter = new Apollo.Common.Entities.Filter
        //            {
        //                Fields = new List<FieldExpression> { new FieldExpression { FieldName = "TrainingName", Operator = QueryOperator.Contains, Argument = new List<object> { keyword } } }
        //            },
        //            Top = 100,
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), null, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
        //        return Convertor.ToEntityList<Training>(res, Convertor.ToTraining);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApolloApiException(ErrorCodes.TrainingErrors.SearchTrainingsByKeywordErr, "Error while searching trainings by keyword", ex);
        //    }
        //}


        /// <summary>
        /// Retrieves trainings within a specified date range.
        /// </summary>
        /// <param name="startDate">Start date of the range.</param>
        /// <param name="endDate">End date of the range.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of Trainings within the date range.</returns>
        //public async Task<IList<Training>> QueryTrainingsByDateRange(DateTime startDate, DateTime endDate)
        //{
        //    try
        //    {
        //        var query = new Apollo.Common.Entities.Query
        //        {
        //            Fields = new List<string> { /* Fields to return */ },
        //            Filter = new Apollo.Common.Entities.Filter
        //            {
        //                Fields = new List<FieldExpression>
        //        {
        //            new FieldExpression { FieldName = "PublishingDate", Operator = QueryOperator.GreaterThanEqualTo, Argument = new List<object> { startDate } },
        //            new FieldExpression { FieldName = "PublishingDate", Operator = QueryOperator.LessThanEqualTo, Argument = new List<object> { endDate } }
        //        }
        //            },
        //            Top = 100,
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Training>(), null, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
        //        return Convertor.ToEntityList<Training>(res, Convertor.ToTraining);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsByDateRangeErr, "Error while querying trainings by date range", ex);
        //    }
        //}


        /// <summary>
        /// Counts the number of trainings provided by a specific provider.
        /// </summary>
        /// <param name="providerId">The identifier of the training provider.</param>
        /// <returns>Task that represents the asynchronous operation, containing the count of trainings.</returns>
        //public async Task<int> CountTrainingsByProvider(string providerId)
        //{
        //    try
        //    {
        //        var query = new Query
        //        {
        //            Filter = new Filter
        //            {
        //                Fields = new List<FieldExpression> { new FieldExpression { FieldName = "ProviderId", Operator = QueryOperator.Equals, Argument = new List<object> { providerId } } },
        //            },
        //            Top = int.MaxValue, // may need to be adjusted to actual data size
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(GetCollectionName<Training>(), null, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
        //        return res.Count;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApolloApiException(ErrorCodes.TrainingErrors.CountTrainingsByProviderErr, "Error while counting trainings by provider", ex);
        //    }
        //}


        /// <summary>
        /// Retrieves a paginated list of trainings based on a specified query filter.
        /// </summary>
        /// <param name="query">The query object containing filter criteria for the trainings.</param>
        /// <param name="pageNumber">Page number for pagination.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of paginated Training objects.</returns>
        public async Task<IList<Training>> QueryTrainingsPaginated(Query query, int pageNumber, int pageSize)
        {
            try
            {
                query.Skip = (pageNumber - 1) * pageSize;
                query.Top = pageSize;

                var res = await _dal.ExecuteQuery(GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
                return Convertor.ToEntityList<Training>(res, Convertor.ToTraining);
            }
            catch (Exception ex)
            {
                throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsPaginatedErr, "Error while querying trainings paginated", ex);
            }
        }


        // <summary>
        /// Retrieves a list of trainings based on a specified query filter, with custom fields selection.
        /// </summary>
        /// <param name="query">The query object containing filter criteria and field selections for the trainings.</param>
        /// <param name="selectedFields">A collection of field names to be included in the result set.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of Trainings with selected fields.</returns>
        public async Task<IList<Training>> QueryTrainingsWithCustomFields(Query query, IEnumerable<string> selectedFields)
        {
            try
            {
                if (selectedFields == null || !selectedFields.Any())
                {
                    selectedFields = new List<string> { /* Default fields */ };
                }

                query.Fields = selectedFields.ToList();

                var res = await _dal.ExecuteQuery(GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
                return Convertor.ToEntityList<Training>(res, Convertor.ToTraining);
            }
            catch (Exception ex)
            {
                throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsWithCustomFieldsErr, "Error while querying trainings with custom fields", ex);
            }
        }


        public async Task<long> GetTotalTrainingCountAsync()
        {
            try
            {
                _logger.LogTrace($"{this.User} entered {nameof(GetTotalTrainingCountAsync)}");

                var filter = Builders<Training>.Filter.Empty;
                var count = await _trainingCollection.CountDocumentsAsync(filter);

                _logger.LogTrace($"{this.User} completed {nameof(GetTotalTrainingCountAsync)}");

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{this.User} failed execution of {nameof(GetTotalTrainingCountAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.TrainingErrors.GetTotalTrainingCountErr, "Error while getting total training count", ex);
            }
        }



        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public virtual async Task<string> InsertTraining(Training training)
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(InsertTraining)}");

                if (String.IsNullOrEmpty(training.Id))
                    training.Id = CreateTrainingId();

                await _dal.InsertAsync(ApolloApi.GetCollectionName<Training>(), Convertor.Convert(training));

                _logger?.LogTrace($"{this.User} completed {nameof(InsertTraining)}");

                return training.Id;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(InsertTraining)}: {ex.Message}");

                // Throw an ApolloApiException with the specific error code
                throw new ApolloApiException(ErrorCodes.TrainingErrors.InsertTrainingErr, "Error while inserting training", ex);
            }
        }



        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training">The training identifier must be specified if the update operation is performed.
        /// If the identifier not specified </param>
        /// <returns>Returns the </returns>
        public virtual async Task<IList<string>> InsertTrainings(ICollection<Training> trainings)
        {
            try
            {
                List<string> ids = new List<string>();

                _logger.LogTrace($"{this.User} entered {nameof(InsertTrainings)}");

                foreach (var training in trainings)
                {
                    var id = CreateTrainingId();
                    ids.Add(id);
                    training.Id = id;
                }

                await _dal.InsertManyAsync(ApolloApi.GetCollectionName<Training>(), trainings.Select(t => Convertor.Convert(t)).ToArray());

                _logger.LogTrace($"{this.User} completed {nameof(InsertTrainings)}");

                return ids;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{this.User} failed execution of {nameof(InsertTrainings)}: {ex.Message}");

                throw new ApolloApiException(ErrorCodes.TrainingErrors.InsertTrainingErr, "Error while creating trainings", ex);
            }
        }




        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training">The training identifier must be specified if the update operation is performed.
        /// If the identifier not specified </param>
        /// <returns>Returns the </returns>
        public virtual Task<string> CreateOrUpdateTraining(Training training)
        {
            try
            {
                _logger.LogTrace($"{this.User} entered {nameof(CreateOrUpdateTraining)}");

                var result = Guid.NewGuid().ToString(); // Assuming you're generating a new ID for the training.

                // Here, you would add the logic to create or update the training in your data store.

                _logger.LogTrace($"{this.User} completed {nameof(CreateOrUpdateTraining)}");

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{this.User} failed execution of {nameof(CreateOrUpdateTraining)}: {ex.Message}");

                throw new ApolloApiException(ErrorCodes.TrainingErrors.CreateOrUpdateTrainingErr, "Error while creating or updating training", ex);
            }
        }


        /// <summary>
        /// Delete Trainings with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of training identifiers.</param>
        /// <returns>The numbe rof deleted trainings.</returns>
        public virtual async Task<long> DeleteTrainings(string[] deletingIds)
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(DeleteTrainings)}");

                var res = await _dal.DeleteManyAsync(GetCollectionName<Training>(), deletingIds);

                _logger?.LogTrace($"{this.User} completed {nameof(DeleteTrainings)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(DeleteTrainings)}: {ex.Message}");

                throw new ApolloApiException(ErrorCodes.TrainingErrors.DeleteTrainingErr, "Error while deleting trainings", ex);
            }
        }
    }
}
