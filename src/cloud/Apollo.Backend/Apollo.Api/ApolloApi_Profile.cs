// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
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
        /// <param name="profileId"></param>
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
        /// Queries for a set of profiles that match specified criteria.
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


        ///// <summary>
        ///// Inserts a new profile into the system.
        ///// </summary>
        ///// <param name="userId">The id of the user to whom th eprofile will be associated.</param>
        ///// <param name="profile">The Profile object to be inserted.</param>
        ///// <returns>Task that represents the asynchronous operation, containing the unique identifier of the inserted profile.</returns>
        //public virtual async Task<string> InsertProfileAsync(string userId, Profile profile)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(InsertProfileAsync)}");

        //        // Generate a unique profile ID if it's not provided
        //        if (String.IsNullOrEmpty(profile.Id))
        //            profile.Id = CreateProfileId(userId);

        //        // Check if the profile with the same ID already exists before inserting
        //        var existingProfile = await _dal.GetByIdAsync<Profile>(ApolloApi.GetCollectionName<Profile>(), profile.Id);
        //        if (existingProfile != null)
        //        {
        //            // Profile with the same ID already exists, throw an ApolloException with a specific code and message
        //            throw new ApolloApiException(ErrorCodes.ProfileErrors.ProfileAlreadyExists, $"Profile with ID '{profile.Id}' already exists.");
        //        }

        //        await _dal.InsertAsync(ApolloApi.GetCollectionName<Profile>(), Convertor.Convert(profile));

        //        _logger?.LogTrace($"Inserting profile with Id: {profile.Id}");

        //        return profile.Id;
        //    }
        //    catch (ApolloApiException)
        //    {

        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $" failed execution of {nameof(InsertProfileAsync)}: {ex.Message}");

        //        // For other exceptions, throw an ApolloApiException with a general error code and message
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.InsertProfileError, "An error occurred while inserting the profile.", ex);
        //    }
        //}


        /// <summary>
        /// Creates or Updates the new Profile instance.
        /// </summary>
        /// <param name="profile">If the Id is specified, the update will be performed.</param>
        public virtual async Task<string> CreateOrUpdateProfile(string userId, Profile profile)
        {
            try
            {
                List<string> ids = new List<string>();

                _logger?.LogTrace($"Entered {nameof(CreateOrUpdateProfile)}");

                await ValidateProfile(userId, profile);

                Profile? existingProfile;

                // Generate a unique profile ID if it's not provided
                if (String.IsNullOrEmpty(profile.Id))
                {
                    profile.Id = CreateProfileId(userId);
                    existingProfile = null;
                }
                else
                {
                    existingProfile = _dal.GetByIdAsync<Profile>(ApolloApi.GetCollectionName<Profile>(), profile.Id).Result;

                    if(existingProfile == null)
                        throw new ApolloApiException(ErrorCodes.ProfileErrors.ListItemNotfound,
                            $"Profile with ID {profile.Id} not found!");
                }

                //
                //Set ID for different items in a List in case empty or null
                EnsureIds<CareerInfo>(profile!, profile?.CareerInfos, existingProfile?.CareerInfos);
                EnsureIds<EducationInfo>(profile!, profile?.EducationInfos, existingProfile?.EducationInfos);
                EnsureIds<Qualification>(profile!, profile?.Qualifications, existingProfile?.Qualifications);
                EnsureIds<LanguageSkill>(profile!, profile?.LanguageSkills, existingProfile?.LanguageSkills);
                EnsureIds<WebReference>(profile!, profile?.WebReferences, existingProfile?.WebReferences);
                EnsureIds<Occupation>(profile!, profile?.Occupations, existingProfile?.Occupations);

                await _dal.UpsertAsync(GetCollectionName<Profile>(), new List<ExpandoObject> { Convertor.Convert(profile!) });

                _logger?.LogTrace($"Completed {nameof(CreateOrUpdateProfile)}");

                return profile?.Id!;
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

        private async Task ValidateProfile(string userId, Profile profile)
        {
            // Check User Id is exist
            var res = await _dal.IsExistAsync<User>(GetCollectionName<User>(), userId);

            //
            //If Not trough Exception
            if (res == false)
                throw new ApolloApiException(ProfileErrors.CreateOrUpdateProfileUserDoesNotExistError, $"The user {userId} does not exist");

            //
            // Checking Profile Is Null or empty
            if (profile == null)
            {
                throw new ApolloApiException(ErrorCodes.ProfileErrors.ProfileIsNullOrEmpty, $"Object Profile is NULL Or Empty");
            }
        }


        /// <summary>
        /// Delete Profiles with specified Ids.
        /// </summary>
        /// <param name="deletingId">The id of profile identifier.</param>
        /// <returns>The deleted id</returns>
        public virtual async Task<string> DeleteProfile(string deletingId)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(DeleteProfile)}");

                // Call the DAL method to delete the profile by their IDs
                await _dal.DeleteAsync(GetCollectionName<Profile>(), deletingId);

                _logger?.LogTrace($"Completed {nameof(DeleteProfile)}");

                return deletingId;

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(DeleteProfile)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.ProfileErrors.DeleteProfileError, "Error while deleting profiles", ex);
            }
        }


        /// <summary>
        /// Generic method to ensure that objects in a list have unique IDs.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="updatingItems">The list of objects to check and set IDs for.</param>
        /// <remarks>
        /// This method iterates through the list of objects, checks if the ID property is already present,
        /// and generates and sets a new ID if necessary. Optionally, you can add logic here to search for
        /// the ID in the database if needed.
        /// </remarks>
        private void EnsureIds<T>(Profile profile, List<T>? updatingItems, List<T>? existingItems) where T : EntityBase
        {
            if (updatingItems != null)
            {
                foreach (var item in updatingItems)
                {
                    //
                    // Check if ID is not  present, generate and set an ID
                    if (String.IsNullOrEmpty(item.Id))
                    {
                        item.Id = CreateListId(typeof(T).Name);
                    }
                    else
                    {
                        if (existingItems == null || existingItems?.FirstOrDefault(eI => eI.Id == item.Id) == null)
                        {
                            throw new ApolloApiException(
                                ErrorCodes.ProfileErrors.ListItemNotfound,
                                $"Item not found with ID {item.Id} for property {nameof(T)}");
                        }
                        else
                        {
                            // nothing to do.
                        }
                    }
                }
            }
        }
    }
}
