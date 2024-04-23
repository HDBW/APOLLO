// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Dynamic;
using Apollo.Common.Entities;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Apollo.Api
{
    /// <summary>
    /// Implements all Apollo Business functionalities.
    /// </summary>
    public partial class ApolloApi
    {
        private readonly IMongoCollection<Training> _trainingCollection;
     
        /// <summary>
        /// Gets the specific instance of the training.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public virtual async Task<Training> GetTrainingAsync(string trainingId)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(GetTrainingAsync)}");

                var training = await _dal.GetByIdAsync<Training>(ApolloApi.GetCollectionName<Training>(), trainingId);

                if (training == null)
                {
                    // No matching training found, throw a specific exception
                    throw new ApolloApiException(ErrorCodes.TrainingErrors.GetTrainingError, "Training not found.");
                }

                _logger?.LogTrace($"Completed {nameof(GetTrainingAsync)}");

                return training;
            }
            catch (ApolloApiException)
            {
               
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{this.User} failed execution of {nameof(GetTrainingAsync)}: {ex.Message}");

                // Throw an ApolloApiException with the specific error code and the caught exception
                throw new ApolloApiException(ErrorCodes.TrainingErrors.GetTrainingError, "Error while getting training", ex);
            }

        }



        /// <summary>
        /// Retrieves a list of trainings based on a specified query filter.
        /// </summary>
        /// <param name="query">The query object containing filter criteria for the trainings.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of matching Training objects.</returns>
        public virtual async Task<IList<Training>> QueryTrainingsAsync(Apollo.Common.Entities.Query query)
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(QueryTrainingsAsync)}");

                // var semRes = _smartLib.SearchTrainings(query);
                // Execute the query 
                var res = await _dal.ExecuteQuery<Training>(ApolloApi.GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

                //TODO: Execute Semantic Search

                _logger?.LogTrace($"{this.User} completed {nameof(QueryTrainingsAsync)}");

                return res;
            }
            catch (ApolloApiException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(QueryTrainingsAsync)}: {ex.Message}");

                // Throw a more generic exception for unexpected errors
                throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsError, "An error occurred while querying trainings.", ex);
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




        // <summary>
        /// Retrieves a list of trainings based on a specified query filter, with custom fields selection.
        /// </summary>
        /// <param name="query">The query object containing filter criteria and field selections for the trainings.</param>
        /// <param name="selectedFields">A collection of field names to be included in the result set.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of Trainings with selected fields.</returns>
        //public async Task<IList<Training>> QueryTrainingsWithCustomFields(Query query, IEnumerable<string> selectedFields)
        //{
        //    try
        //    {
        //        if (selectedFields == null || !selectedFields.Any())
        //        {
        //            selectedFields = new List<string> { /* Default fields */ };
        //        }

        //        query.Fields = selectedFields.ToList();

        //        var res = await _dal.ExecuteQuery(GetCollectionName<Training>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

        //        if (res == null)
        //        {
        //            // No results found, throw a specific exception
        //            throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsWithCustomFieldsErr, "No results found for the query.");
        //        }

        //        return Convertor.ToEntityList<Training>(res, Convertor.ToTraining);
        //    }
        //    catch (ApolloApiException)
        //    {
                
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApolloApiException(ErrorCodes.TrainingErrors.QueryTrainingsWithCustomFieldsErr, "Error while querying trainings with custom fields", ex);
        //    }
        //}


        /// <summary>
        /// Asynchronously gets the total count of Training documents in the database.
        /// </summary>
        /// <returns>The total count of Training documents as a long.</returns>
        /// <exception cref="ApolloApiException">Thrown when there is an error during the operation.</exception>
        public async Task<long> GetTotalTrainingCountAsync()
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(GetTotalTrainingCountAsync)}");

                var filter = Builders<Training>.Filter.Empty;
                var count = await _trainingCollection.CountDocumentsAsync(filter);

                if (count < 0)
                {
                    // Negative count is unexpected, throw a specific exception
                    throw new ApolloApiException(ErrorCodes.TrainingErrors.GetTotalTrainingCountErr, "Invalid total training count.");
                }

                _logger?.LogTrace($"{this.User} completed {nameof(GetTotalTrainingCountAsync)}");

                return count;
            }
            catch (ApolloApiException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(GetTotalTrainingCountAsync)}: {ex.Message}");

                // Throw a more generic exception for unexpected errors
                throw new ApolloApiException(ErrorCodes.TrainingErrors.GetTotalTrainingCountErr, "Error while getting total training count", ex);

            }
        }


        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public virtual async Task<string> InsertTrainingAsync(Training training)
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(InsertTrainingAsync)}");

                if (String.IsNullOrEmpty(training.Id))
                    training.Id = CreateTrainingId();

                await _dal.InsertAsync(ApolloApi.GetCollectionName<Training>(), Convertor.Convert(training));

                _logger?.LogTrace($"{this.User} completed {nameof(InsertTrainingAsync)}");

                return training.Id;
            }
            catch (ApolloApiException)
            {
                
                throw;
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                // If the exception is due to a duplicate key error (unique constraint violation),
                // throw a specific exception with an appropriate error code and message.
                throw new ApolloApiException(ErrorCodes.TrainingErrors.InsertTrainingErr, "Duplicate training ID. Training already exists.", ex);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(InsertTrainingAsync)}: {ex.Message}");

                // Throw a more generic exception for unexpected errors
                throw new ApolloApiException(ErrorCodes.GeneralErrors.OperationFailed, "An error occurred while processing the request.", ex);
            }
        }


        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training">The training identifier must be specified if the update operation is performed.
        /// If the identifier not specified </param>
        /// <returns>Returns the </returns>
        public virtual async Task<IList<string>> InsertTrainingsAsync(ICollection<Training> trainings)
        {
            try
            {
                List<string> ids = new List<string>();

                _logger?.LogTrace($"{this.User} entered {nameof(InsertTrainingsAsync)}");

                if (trainings == null || !trainings.Any())
                {
                    // No trainings to insert, throw a specific exception
                    throw new ApolloApiException(ErrorCodes.TrainingErrors.InsertTrainingErr, "No trainings to insert.");
                }

                foreach (var training in trainings)
                {
                    var id = String.IsNullOrEmpty(training.Id) ? CreateTrainingId() : training.Id;
                    ids.Add(id);
                    training.Id = id;
                }

                await _dal.InsertManyAsync(ApolloApi.GetCollectionName<Training>(), trainings.Select(t => Convertor.Convert(t)).ToArray());

                _logger?.LogTrace($"{this.User} completed {nameof(InsertTrainingsAsync)}");

                return ids;
            }
            catch (ApolloApiException)
            {
               
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(InsertTrainingsAsync)}: {ex.Message}");

                // Throw a more generic exception for unexpected errors
                throw new ApolloApiException(ErrorCodes.GeneralErrors.OperationFailed, "An error occurred while processing the request.", ex);
            }
        }


        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training">The training identifier must be specified if the update operation is performed.
        /// If the identifier not specified </param>
        /// <returns>Returns the </returns>
        public virtual async Task<IList<string>> CreateOrUpdateTrainingAsync(ICollection<Training> trainings)
        {
            try
            {
                List<string> ids = new List<string>();

                _logger?.LogTrace($"{this.User} entered {nameof(CreateOrUpdateTrainingAsync)}");

                if (trainings == null || !trainings.Any())
                {
                    throw new ApolloApiException(ErrorCodes.TrainingErrors.CreateOrUpdateTrainingErr, "No training data provided.");
                }



                foreach (var training in trainings)
                {
                    if (string.IsNullOrEmpty(training.Id))
                    {
                        // Generate a new ID for new training
                        training.Id = CreateTrainingId();
                        training.CreatedAt = DateTime.UtcNow;

                        //
                        //Currently it is not possible to ger information of logged in User performs creating
                        // We keep it for future
                        //TO:DO:
                        //training.CreatedBy = "Apollo";
                    }

                    // Add the ID to the list regardless of whether it's new or existing
                    ids.Add(training.Id);
                    training.ChangedAt = DateTime.UtcNow;

                    //
                    //Currently it is not possible to get information of logged in User who perfroms updating
                    // We keep it for future
                    // TO:DO:
                    //training.ChangedBy = "Apollo";

                    // Convert the training object to ExpandoObject
                    //TODO: Cleaner HTML Data out
                    CleanTraining(training);

                    var expandoTraining = Convertor.Convert(training);
                    
                    // Upsert the training
                    await _dal.UpsertAsync(GetCollectionName<Training>(), new List<ExpandoObject> { expandoTraining });
                }

                _logger?.LogTrace($"{this.User} completed {nameof(CreateOrUpdateTrainingAsync)}");

                return ids;
            }
            catch (ApolloApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(CreateOrUpdateTrainingAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.GeneralErrors.OperationFailed, "An error occurred while processing the request.", ex);
            }
        }

        private void CleanTraining(Training training)
        {
            training.ShortDescription = CleanString(training.ShortDescription);
            training.Description = CleanString(training.Description);

            if (!string.IsNullOrEmpty(training.PriceDescription))
            {
                training.PriceDescription = CleanString(training.PriceDescription);
            }

            if (!string.IsNullOrEmpty(training.SubTitle))
            {
                training.SubTitle = CleanString(training.SubTitle);
            }

            //training.BenefitList


            if (training.Content != null)
            {
                for (int i = 0; i < training.Content.Count; i++)
                {
                    training.Content[i] = CleanString(training.Content[i]);
                }
            }
        }

        private static string CleanString(string str)
        {
            str.Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>").Replace("\t", "&emsp;").Replace("  ", " ").Trim();
            var doc = new HtmlDocument();
            doc.LoadHtml(str);
            var unwantedTags = new string[] { "script", "style", "href", "a", "img", "pre", "div", "h" };

            foreach (var tag in unwantedTags)
            {
                var nodes = doc.DocumentNode.DescendantsAndSelf(tag);
                foreach (var node in nodes.ToList())  // ToList() is necessary because the collection is modified in the loop
                {
                    if (node.ParentNode != null)
                    {
                        node.ParentNode.RemoveChild(node, keepGrandChildren: true);
                    }
                }
            }
            // Decode HTML entities
            string decodedHtml = System.Net.WebUtility.HtmlDecode(doc.DocumentNode.OuterHtml);
            return decodedHtml.Trim();
        }


        /// <summary>
        /// Asynchronously deletes multiple Training instances.
        /// </summary>
        /// <param name="deletingIds">An array of identifiers of the Training instances to delete.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the number of deleted Training instances.</returns>
        /// <exception cref="ApolloApiException">Thrown when there is an error during the operation.</exception>
        public virtual async Task<long> DeleteTrainingsAsync(string[] deletingIds)
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(DeleteTrainingsAsync)}");

                var res = await _dal.DeleteManyAsync(GetCollectionName<Training>(), deletingIds);

                _logger?.LogTrace($"{this.User} completed {nameof(DeleteTrainingsAsync)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(DeleteTrainingsAsync)}: {ex.Message}");

                throw new ApolloApiException(ErrorCodes.TrainingErrors.DeleteTrainingErr, "Error while deleting trainings", ex);
            }
        }

        

        /// <summary>
        /// Delete All Trainings with a specified Prover Id.
        /// </summary>
        /// <param name="providerId">The Provider Id whise training need to be deleted</param>
        /// <returns>The number row of deleted trainings.</returns>
        public virtual async Task<List<string>> DeleteProviderTrainingsAsync(string[] providerId)
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(DeleteProviderTrainingsAsync)}");

                var res = await _dal.RemoveTrainingsByProviderIdAsync(GetCollectionName<Training>(), providerId);

                _logger?.LogTrace($"{this.User} completed {nameof(DeleteProviderTrainingsAsync)}");

                 return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"{this.User} failed execution of {nameof(DeleteProviderTrainingsAsync)}: {ex.Message}");

                throw new ApolloApiException(ErrorCodes.TrainingErrors.DeleteTrainingErr, "Error while deleting trainings", ex);
            }
        }

    }
}
