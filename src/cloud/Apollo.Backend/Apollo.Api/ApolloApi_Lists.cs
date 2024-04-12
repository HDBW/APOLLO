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
        private string _defaultLng = "Invariant";

        #region List Methods

        /// <summary>
        /// Returns the list of all available ApolloLists.
        /// </summary>
        /// <returns>The instance of all available <see cref="ApolloList"/>s. Note that only <see cref="ApolloList.Id"/> and <see cref="ApolloList.ItemType"/> are set.</returns>
        /// <exception cref="ApolloApiException">If the list cannot be found.</exception>"
        public async Task<List<ApolloList>> GetAllListsAsync()
        {
            Query query = new Query
            {
                Filter = new Filter(),
            };

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<List>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

            if (res.Any())
            {
                return res.Select(expando =>
                {
                    var minimalInfoList = Convertor.ToApolloList(expando);
                    return new ApolloList
                    {
                        Id = minimalInfoList.Id,
                        ItemType = minimalInfoList.ItemType
                    };
                }).ToList();
            }
            else
            {
                throw new ApolloApiException(ErrorCodes.ListErrors.ListNotFoundError, "Requested Items Not Found");
            }
        }



        /// <summary>
        /// Returns the ApolloList list with all values of the given item type.
        /// </summary>
        /// <param name="itemType">It must be specified if the <see cref="id"/> argument is not specified.</param>
        /// <param name="id">If specified arguments <see cref="itemType"/> are ignored.</param>
        /// <returns>The instance of the <see cref="ApolloList"/> that was requested.
        /// Null if the list cannot be found.</returns>
        /// <exception cref="ApolloApiException">If the list cannot be found and the <see cref="throwIfNotFound"/> is set to true.</exception>"
        public async Task<ApolloList?> GetListAsync(string? itemType = null, string? id = null, bool throwIfNotFound = false)
        {
            // TODO validate if all args correctlly set

            Query query = new Query
            {
                Filter = new Filter(),
            };

            // query?.Fields?.Add(nameof(ApolloListItem.ListItemId));

            if (!String.IsNullOrEmpty(id))
            {
                query?.Filter.Fields.Add(new FieldExpression()
                {
                    Operator = QueryOperator.Equals,
                    Argument = new List<object>() { id },
                    FieldName = "_id"
                });
            }
            else
            {
                //
                // We filter requested ItemType
                query?.Filter.Fields.Add(new FieldExpression()
                {
                    Operator = QueryOperator.Equals,
                    Argument = new List<object>() { itemType! },
                    FieldName = nameof(ApolloList.ItemType)
                });
            }

            // this must return the single item.
            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<List>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

            if (res.FirstOrDefault() != null)
                return Convertor.ToApolloList(res.FirstOrDefault()!);
            else
            {
                if (throwIfNotFound)
                    throw new ApolloApiException(ErrorCodes.ListErrors.ListNotFoundError, "Requested Items Not Found");//todo.
                else
                    return null;
            }
        }


        /// <summary>
        /// Looks up list items by specified criteria.
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="itemType"></param>
        /// <param name="contains"></param>
        /// <param name="returnValuesOnly">By default set on true. It returns the value of the item only.
        /// If FALSE, it returns teh description of the item.</param>
        /// <returns>List of matching items or empty list.</returns>
        public async Task<List<ApolloListItem>> QueryListItemsAsync(string? lng, string itemType, string? contains)
        {
            if (String.IsNullOrEmpty(lng))
                lng = _defaultLng;

            Query query = new Query
            {
                Fields = new List<string>(),
                Filter = new Filter(),
            };

            //
            // Filters the value.
            if (!string.IsNullOrEmpty(contains))
            {
                query.Filter.Fields.Add(new FieldExpression()
                {
                    Operator = QueryOperator.Contains,
                    Argument = new List<object>() { contains! },
                    FieldName = "Items.Value",
                });
            }

            //
            // Filter the ItemType.
            query.Filter.Fields.Add(new FieldExpression()
            {
                Operator = QueryOperator.Equals,
                Argument = new List<object>() { itemType },
                FieldName = "ItemType"
            });

            var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<List>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));

            if (res.Count == 1)
            {
                var apList = Convertor.ToApolloList(res.FirstOrDefault()!);

                List<ApolloListItem> items = apList.Items.Where(i =>
                (lng != null ? i.Lng == lng : true) && (contains != null ? i.Value.ToLower().Contains(contains.ToLower()) : true)).ToList();

                return items;
            }
            else if (res.Count > 0)
            {
                throw new ApolloApiException();//Lucija todo.
            }
            else
                return new List<ApolloListItem>();
        }


        private const string _cDefaultLanguageValue = "Invariant";

        /// <summary>
        /// Creates or Updates the new List of items.
        /// </summary>
        /// <param name="list">If the <see cref="ApolloList.ItemType"/> or <see cref="ApolloList.Id"/>  is specified and exists in the DB, the update will be performed.</param>
        /// <returns></returns>
        public async Task<string> CreateOrUpdateListAsync(ApolloList list)
        {
            try
            {
                string id;

                var existingList = await GetListAsync(itemType: list.ItemType, id: list.Id);

                if (existingList == null)
                {
                    EnsureLangueAndId(list);

                    id = list.Id;

                    await _dal.InsertAsync(ApolloApi.GetCollectionName<List>(), Convertor.Convert(list));
                }
                else
                {
                    id = list.Id = existingList.Id;

                    EnsureLangueAndId(list);

                    await _dal.UpsertAsync(GetCollectionName<List>(), new List<ExpandoObject> { Convertor.Convert(list) });
                }

                return id;
            }
            catch (ApolloApiException ex)
            {
                // Implementing the missing logging for ApolloApiException
                _logger?.LogError(ex, $"ApolloApiException with ErrorCode {ex.ErrorCode} in {nameof(CreateOrUpdateQualificationAsync)}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Logging the exception details
                _logger?.LogError(ex, $"Failed execution of {nameof(CreateOrUpdateQualificationAsync)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.ListErrors.CreateOrUpdateListError, "An error occurred while creating or updating the profiles.", ex);
            }
        }


        /// <summary>
        /// Makes sure that the labguage is set. If not we set it on invariant.
        /// </summary>
        /// <param name="list"></param>
        private static void EnsureLangueAndId(ApolloList list)
        {
            if (String.IsNullOrEmpty(list.Id))
            {
                list.Id = CreateListId($"{nameof(List)}-{list.ItemType}");
            }

            foreach (var lstItem in list.Items)
            {

                if (String.IsNullOrEmpty(lstItem.Lng))
                {
                    lstItem.Lng = _cDefaultLanguageValue;
                }
            }
        }


        /// <summary>
        /// Deletes the ApolloLista with a given identifiers
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<long> DeleteListAsync(string[] ids)
        {
            var cnt = await _dal.DeleteManyAsync(ApolloApi.GetCollectionName<List>(), ids);

            return cnt;
        }

        #endregion
    }
}
