// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Amazon.Runtime.Internal.Util;
using Apollo.Common.Entities;
using Microsoft.Extensions.Logging;
using ZstdSharp.Unsafe;
using static Apollo.Api.ErrorCodes;

namespace Apollo.Api
{
    /// <summary>
    /// TODO
    /// </summary>
    public partial class ApolloApi
    {
        private string _defaultLng = "eng";

        #region Qualification
        /// <summary>
        /// Queries for a set of list items that match specified criteria and qqualification itemtype.
        /// </summary>
        /// <param name="lng">The language of the list items.</param>
        /// <param name="query">The filter that specifies profiles to be retrieved.</param>
        /// <returns>List of item.</returns>
        public virtual async Task<IList<List>> QueryQualificationsListAsync(string lng, Query query)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(QueryQualificationsListAsync)}");

                IsQueryValid(query, throwIfInvalid: true);

                //
                // We filter requested language.
                query.Filter.Fields.Add(new FieldExpression()
                {
                    Operator = QueryOperator.Equals,
                    Argument = new List<object>() { lng },
                    FieldName = "Items.Lng"
                });

                query.Filter.Fields.Add(new FieldExpression()
                {
                    Operator = QueryOperator.Equals,
                    Argument = new List<object>() { nameof(List.ItemType) },
                    FieldName = nameof(List.ItemType),
                });

                var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<List>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

                var list = Convertor.ToEntityList<List>(res, Convertor.ToList);

                _logger?.LogTrace($"Completed {nameof(QueryQualificationsListAsync)}");

                return list;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(QueryQualificationsListAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ListErrors.QueryListError, "Error while querying the list", ex);
            }
        }


        /// <summary>
        /// Creates or Updates the new Profile instance.
        /// </summary>
        /// <param name="list">If the Id is specified, the update will be performed.</param>
        /// <returns></returns>
        public virtual async Task<List<string>> CreateOrUpdateQualificationAsync(List list)
        {
            try
            {
                List<string> ids = new List<string>();

                // TODO Validate if list is null

                _logger?.LogTrace($"Entered {nameof(CreateOrUpdateQualificationAsync)}");

                if (String.IsNullOrEmpty(list.Id))
                {
                    list.Id = CreateListId(nameof(Qualification));
                    await _dal.InsertAsync(ApolloApi.GetCollectionName<List>(), Convertor.Convert(list));
                }
                else
                {
                    var existingList = await _dal.GetByIdAsync<List>(ApolloApi.GetCollectionName<List>(), list.Id);

                    if (existingList != null)
                    {
                        list.Id = existingList.Id;
                        await _dal.UpsertAsync(GetCollectionName<List>(), new List<ExpandoObject> { Convertor.Convert(list) });
                    }
                    else
                    {
                        throw new ApolloApiException(ListErrors.CreateOrUpdateListError, $"The list with the specified Id = {list.Id} does not exist.");
                    }
                }            

           
                _logger?.LogTrace($"Completed {nameof(CreateOrUpdateQualificationAsync)}");

                return ids;
            }
            catch (ApolloApiException)
            {
                //todo. Logging not implemented
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(CreateOrUpdateQualificationAsync)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.ListErrors.CreateOrUpdateListError, "An error occurred while creating or updating profiles.", ex);
            }
        }


        /// <summary>
        /// Delete Profiles with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of profile identifiers.</param>
        /// <returns>The number of deleted Profiles</returns>
        public virtual async Task<long> DeleteQualificationAsync(string[] deletingIds)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(DeleteSkillsAsync)}");

                // Call the DAL method to delete the profile by their IDs
                var res = await _dal.DeleteManyAsync(GetCollectionName<List>(), deletingIds, throwIfNotDeleted: false);

                _logger?.LogTrace($"Completed {nameof(DeleteSkillsAsync)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(DeleteQualificationAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ListErrors.DeleteListError, $"Error while deleting the list", ex);
            }
        }
        #endregion
    }
}
