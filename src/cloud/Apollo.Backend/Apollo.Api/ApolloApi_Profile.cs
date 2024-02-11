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

        /// <summary>
        /// <summary>
        /// Gets the specific instance of the profile.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public virtual async Task<Profile> GetProfile(string profileId)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(GetProfile)}");

                var profile = await _dal.GetByIdAsync<Profile>(ApolloApi.GetCollectionName<Profile>(), profileId);

                _logger?.LogTrace($"Completed {nameof(GetProfile)}");//todo..

                if (profile == null)
                {
                    // Profile not found, throw ApolloException with specific code and message
                    throw new ApolloApiException(ErrorCodes.ProfileErrors.ProfileNotFound, $"Profile with ID '{profileId}' not found.");
                }

                return profile;
            }
            catch (ApolloApiException)
            {

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed execution of {nameof(GetProfile)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.ProfileErrors.GetProfileError, "An error occurred while getting the profile.", ex);
            }
        }


        /// <summary>
        /// Queries for a set of prfiles that match specified criteria.
        /// </summary>
        /// <param name="query">The filter that specifies profiles to be retrieved.</param>
        /// <returns>List of profiles.</returns>
        // TODO: More specific exception handeling for this method
        public virtual async Task<IList<Profile>> QueryProfilesAsync(Query query)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(QueryProfilesAsync)}");

                var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Profile>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
                var profiles = Convertor.ToEntityList<Profile>(res, Convertor.ToProfile);

                _logger?.LogTrace($"Completed {nameof(QueryProfilesAsync)}");

                return profiles;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(QueryProfilesAsync)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ProfileErrors.GetProfileError, "Error while querying profiles", ex);
            }
        }


        /// <summary>
        /// Inserts a new profile into the system.
        /// </summary>
        /// <param name="userId">The id of the user to whom th eprofile will be associated.</param>
        /// <param name="profile">The Profile object to be inserted.</param>
        /// <returns>Task that represents the asynchronous operation, containing the unique identifier of the inserted profile.</returns>
        public virtual async Task<string> InsertProfileAsync(string userId, Profile profile)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(InsertProfileAsync)}");

                // Generate a unique profile ID if it's not provided
                if (String.IsNullOrEmpty(profile.Id))
                    profile.Id = CreateProfileId(userId);

                // Check if the profile with the same ID already exists before inserting
                var existingProfile = await _dal.GetByIdAsync<Profile>(ApolloApi.GetCollectionName<Profile>(), profile.Id);
                if (existingProfile != null)
                {
                    // Profile with the same ID already exists, throw an ApolloException with a specific code and message
                    throw new ApolloApiException(ErrorCodes.ProfileErrors.ProfileAlreadyExists, $"Profile with ID '{profile.Id}' already exists.");
                }

                await _dal.InsertAsync(ApolloApi.GetCollectionName<Profile>(), Convertor.Convert(profile));

                _logger?.LogTrace($"Inserting profile with Id: {profile.Id}");

                return profile.Id;
            }
            catch (ApolloApiException)
            {

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $" failed execution of {nameof(InsertProfileAsync)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.ProfileErrors.InsertProfileError, "An error occurred while inserting the profile.", ex);
            }
        }


        /// <summary>
        /// Creates or Updates the new Profile instance.
        /// </summary>
        /// <param name="profile">If the Id is specified, the update will be performed.</param>
        /// <returns></returns>
        public virtual async Task<List<string>> CreateOrUpdateProfile(string userId, Profile profile)
        {
            try
            {
                List<string> ids = new List<string>();

                // TODO Validate if profile is null
                // TODO Validate if userId exists.
                _logger?.LogTrace($"Entered {nameof(CreateOrUpdateProfile)}");

                if (profile == null)
                {
                    throw new ApolloApiException(ErrorCodes.ProfileErrors.ProfileIsNullOrEmpty, $"Object Profile is NULL Or Empty");
                }

                // Generate a unique profile ID if it's not provided
                if (String.IsNullOrEmpty(profile.Id))
                {
                    profile.Id = CreateProfileId(userId);
                }
                else
                {
                    //
                    //Check User Id is exist
                    var res = await _dal.IsExistAsync<Profile>(GetCollectionName<Profile>(), profile?.Id);
                    //
                    //If Not trough Exception
                    if (res == false)
                        throw new ApolloApiException(ProfileErrors.CreateOrUpdateProfileUserDoesNotExistError, $"The user {userId} does not exist");
                }

                await _dal.UpsertAsync(GetCollectionName<Profile>(), new List<ExpandoObject> { Convertor.Convert(profile) });

                _logger?.LogTrace($"Completed {nameof(CreateOrUpdateProfile)}");

                return ids;
            }
            catch (ApolloApiException)
            {
                //todo. Logging not implemented
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(CreateOrUpdateProfile)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.ProfileErrors.CreateOrUpdateProfileError, "An error occurred while creating or updating profiles.", ex);
            }
        }


        /// <summary>
        /// Delete Profiles with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of profile identifiers.</param>
        /// <returns>The number of deleted Profiles</returns>
        public virtual async Task<long> DeleteProfiles(string[] deletingIds)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(DeleteProfiles)}");

                // Call the DAL method to delete the profile by their IDs
                var res = await _dal.DeleteManyAsync(GetCollectionName<Profile>(), deletingIds, throwIfNotDeleted: false);

                _logger?.LogTrace($"Completed {nameof(DeleteProfiles)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(DeleteProfiles)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ProfileErrors.DeleteProfileError, "Error while deleting profiles", ex);
            }
        }

    }
}
