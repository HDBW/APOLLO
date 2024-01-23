// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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
    /// Implements all Apollo Business functionalities.
    /// </summary>
    public partial class ApolloApi
    {

        #region Qualification
        /// <summary>
        /// Queries for a set of qualifications that match specified criteria.
        /// </summary>
        /// <param name="query">The filter that specifies profiles to be retrieved.</param>
        /// <returns>List of profiles.</returns>
        // TODO: More specific exception handeling for this method
        public virtual async Task<IList<Qualification>> QueryQualificationssAsync(string lng, Query query)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(QueryQualificationssAsync)}");

                var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Qualification>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
                var qualis = Convertor.ToEntityList<Qualification>(res, Convertor.ToQualification);

                _logger?.LogTrace($"Completed {nameof(QueryQualificationssAsync)}");

                return qualis;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(QueryQualificationssAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ProfileErrors.GetProfileError, "Error while querying profiles", ex);
            }
        }


        /// <summary>
        /// Creates or Updates the new Profile instance.
        /// </summary>
        /// <param name="profile">If the Id is specified, the update will be performed.</param>
        /// <returns></returns>
        public virtual async Task<List<string>> CreateOrUpdateQualificationAsync(string userId, Qualification profile)
        {
            try
            {
                List<string> ids = new List<string>();

                _logger?.LogTrace($"Entered {nameof(CreateOrUpdateQualificationAsync)}");

                if (profile == null)
                {
                    profile.Id = CreateProfileId(userId);
                }

                var res = await _dal.IsExistAsync<Profile>(GetCollectionName<Profile>(), profile.Id);
                if (res == false)
                    throw new ApolloApiException(ProfileErrors.CreateOrUpdateProfileUserDoesNotExistError, $"The user {userId} does not exist");

                await _dal.UpsertAsync(GetCollectionName<Profile>(), new List<ExpandoObject> { Convertor.Convert(profile) });

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
                throw new ApolloApiException(ErrorCodes.ProfileErrors.CreateOrUpdateProfileError, "An error occurred while creating or updating profiles.", ex);
            }
        }


        /// <summary>
        /// Delete Profiles with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of profile identifiers.</param>
        /// <returns>The number of deleted Profiles</returns>
        public virtual async Task<long> DeleteSkillsAsync(string[] deletingIds)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(DeleteSkillsAsync)}");

                // Call the DAL method to delete the profile by their IDs
                var res = await _dal.DeleteManyAsync(GetCollectionName<Profile>(), deletingIds, throwIfNotDeleted: false);

                _logger?.LogTrace($"Completed {nameof(DeleteSkillsAsync)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(DeleteSkillsAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ProfileErrors.DeleteProfileError, "Error while deleting profiles", ex);
            }
        }
        #endregion

    }
}
