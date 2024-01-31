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

        #region Internal Helpers
        /// <summary>
        /// Returns the ApolloList list with all values of the given item type.
        /// </summary>
        /// <param name="lng">It must be specified if the <see cref="id"/> argument is not specified.</param>
        /// <param name="itemType">It must be specified if the <see cref="id"/> argument is not specified.</param>
        /// <param name="id">If specified arguments <see cref="lng"/> and <see cref="itemType"/> are ignored.</param>
        /// <returns>The instance of the <see cref="ApolloList"/> that was requested.
        /// Null if the list cannot be found.</returns>
        public async Task<ApolloList> GetListInternalAsync(string? lng = null, string? itemType = null, string? id = null)
        {
            // TODO validate if all args correctlly set

            Query query = new Query
            {
                Filter = new Filter(),
            };

            if (!String.IsNullOrEmpty(id))
            {
                query.Filter.Fields.Add(new FieldExpression()
                {
                    Operator = QueryOperator.Equals,
                    Argument = new List<object>() { id },
                    FieldName = "_id"
                });
            }
            else
            {
                //
                // We filter by requested language.
                if (!String.IsNullOrEmpty(lng))
                {
                    query.Filter.Fields.Add(new FieldExpression()
                    {
                        Operator = QueryOperator.Equals,
                        Argument = new List<object>() { lng },
                        FieldName = "Items.Lng"
                    });
                }

                //
                // We filter requested language.
                query.Filter.Fields.Add(new FieldExpression()
                {
                    Operator = QueryOperator.Equals,
                    Argument = new List<object>() { itemType! },
                    FieldName = nameof(ApolloList.ItemType)
                });
            }

            // this must return the single item.
            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<ApolloList>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

            if (res.FirstOrDefault() != null)
                return Convertor.ToApolloList(res.FirstOrDefault()!);
            else
                return new ApolloList() {  Items= new List<ApolloListItem>(), ItemType = itemType == null?  null! : itemType!};
        }


        /// <summary>
        /// Deletes the ApolloLista with a given identifiers
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<long> DeleteListInternalAsync(string[] ids)
        {   
            var cnt = await _dal.DeleteManyAsync(ApolloApi.GetCollectionName<ApolloList>(), ids);
            
            return cnt;
        }

        /// <summary>
        ///  Used by all typeed query methods that query for list items.
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="itemType"></param>
        /// <param name="contains"></param>
        /// <param name="returnValuesOnly">By default set on true. It returns the value of the item only.
        /// If FALSE, it returns teh description of the item.</param>
        /// <returns></returns>
        public async Task<List<string>> QueryListInternalAsync(string lng, string itemType, string? contains)
        {   
            Query query = new Query
            {
                Fields = new List<string>() { "Value" },
                Filter = new Filter(),
            };
                       
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
                Operator = QueryOperator.Contains,
                Argument = new List<object>() { nameof(ApolloList.ItemType) },
                FieldName = nameof(ApolloList.ItemType),
            });

            //
            // We filter requested language.
            query.Filter.Fields.Add(new FieldExpression()
            {
                Operator = QueryOperator.Equals,
                Argument = new List<object>() { itemType },
                FieldName = "ItemType"
            });

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<ApolloList>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

            var list = Convertor.ToEntityList<string>(res, Convertor.ToListValue);

            return list;
        }


        /// <summary>
        /// Creates or Updates the new List of items.
        /// </summary>
        /// <param name="list">If the Id is specified, the update will be performed.</param>
        /// <returns></returns>
        public async Task<List<string>> CreateOrUpdateListAsync(ApolloList list)
        {
            try
            {
                List<string> ids = new List<string>();

                
                if (String.IsNullOrEmpty(list.Id))
                {
                    list.Id = CreateListId(nameof(Qualification));
                    await _dal.InsertAsync(ApolloApi.GetCollectionName<ApolloList>(), Convertor.Convert(list));
                }
                else
                {
                    var existingList = await _dal.GetByIdAsync<ApolloList>(ApolloApi.GetCollectionName<ApolloList>(), list.Id);

                    if (existingList != null)
                    {
                        list.Id = existingList.Id;
                        await _dal.UpsertAsync(GetCollectionName<ApolloList>(), new List<ExpandoObject> { Convertor.Convert(list) });
                    }
                    else
                    {
                        throw new ApolloApiException(ListErrors.CreateOrUpdateListError, $"The list with the specified Id = {list.Id} does not exist.");
                    }
                }

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
        #endregion

        #region Qualification
        /// <summary>
        /// Gets the list qualifications.
        /// </summary>
        /// <param name="lng">The language of the list items. Optional</param>
        /// <param name="values">The list of values that will be retrieved. If null, the language must be specified. In that case
        /// all values of the given language are returned.</param>
        /// <returns>List of items.</returns>
        public async Task<ApolloList> GetQualificationsListAsync(string? lng)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(QueryQualificationsListAsync)}");

                var list = await GetListInternalAsync(lng, nameof(Qualification));

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
        /// Queries for a list of qualifications.
        /// </summary>
        /// <param name="lng">The language of the list items.</param>
        /// <param name="contains">The filter that specifies profiles to be retrieved.</param>
        /// <returns>List of items.</returns>
        public async Task<List<string>> QueryQualificationsListAsync(string lng, string? contains)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(QueryQualificationsListAsync)}");

                 var list = await  QueryListInternalAsync(lng, nameof(Qualification), contains);
                  
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
        /// Creates or Updates the new List of items.
        /// </summary>
        /// <param name="list">If the Id is specified, the update will be performed.</param>
        /// <returns></returns>
        public virtual async Task<List<string>> CreateOrUpdateQualificationAsync(ApolloList list)
        {
            try
            {
                List<string> ids = new List<string>();

                // TODO Validate if list is null

                _logger?.LogTrace($"Entered {nameof(CreateOrUpdateQualificationAsync)}");

                if (String.IsNullOrEmpty(list.Id))
                {
                    list.Id = CreateListId(nameof(Qualification));
                    await _dal.InsertAsync(ApolloApi.GetCollectionName<ApolloList>(), Convertor.Convert(list));
                }
                else
                {
                    var existingList = await _dal.GetByIdAsync<ApolloList>(ApolloApi.GetCollectionName<ApolloList>(), list.Id);

                    if (existingList != null)
                    {
                        list.Id = existingList.Id;
                        await _dal.UpsertAsync(GetCollectionName<ApolloList>(), new List<ExpandoObject> { Convertor.Convert(list) });
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
        public virtual async Task<long> DeleteQualificationListAsync(string[] deletingIds)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(DeleteSkillsAsync)}");

                // Call the DAL method to delete the profile by their IDs
                var res = await _dal.DeleteManyAsync(GetCollectionName<ApolloList>(), deletingIds, throwIfNotDeleted: false);

                _logger?.LogTrace($"Completed {nameof(DeleteSkillsAsync)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(DeleteQualificationListAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ListErrors.DeleteListError, $"Error while deleting the list", ex);
            }
        }
        #endregion


        #region StaffResponsibility
      
        #endregion
    }
}
