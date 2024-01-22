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
    /// TODO
    /// </summary>
    public partial class ApolloApi
    {

        //#region Qualification
        ///// <summary>
        ///// Queries for a set of prfiles that match specified criteria.
        ///// </summary>
        ///// <param name="query">The filter that specifies profiles to be retrieved.</param>
        ///// <returns>List of profiles.</returns>
        //// TODO: More specific exception handeling for this method
        //public virtual async Task<IList<Profile>> QueryQualificationsAsync(Query query)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(QuerySkillsAsync)}");

        //        var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Profile>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
        //        var profiles = Convertor.ToEntityList<Profile>(res, Convertor.ToProfile);

        //        _logger?.LogTrace($"Completed {nameof(QuerySkillsAsync)}");

        //        return profiles;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(QuerySkillsAsync)}: {ex.Message}");
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.GetProfileError, "Error while querying profiles", ex);
        //    }
        //}


        ///// <summary>
        ///// Creates or Updates the new Profile instance.
        ///// </summary>
        ///// <param name="profile">If the Id is specified, the update will be performed.</param>
        ///// <returns></returns>
        //public virtual async Task<List<string>> CreateOrUpdateSkill(string userId, Profile profile)
        //{
        //    try
        //    {
        //        List<string> ids = new List<string>();

        //        // TODO Validate if profile is null
        //        // TODO Validate if userId exists.
        //        _logger?.LogTrace($"Entered {nameof(CreateOrUpdateProfile)}");

        //        if (profile == null)
        //        {
        //            profile.Id = CreateProfileId(userId);
        //        }

        //        var res = await _dal.IsExistAsync<Profile>(GetCollectionName<Profile>(), profile.Id);
        //        if (res == false)
        //            throw new ApolloApiException(ProfileErrors.CreateOrUpdateProfileUserDoesNotExistError, $"The user {userId} does not exist");

        //        await _dal.UpsertAsync(GetCollectionName<Profile>(), new List<ExpandoObject> { Convertor.Convert(profile) });

        //        _logger?.LogTrace($"Completed {nameof(CreateOrUpdateSkill)}");

        //        return ids;
        //    }
        //    catch (ApolloApiException)
        //    {
        //        //todo. Logging not implemented
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(CreateOrUpdateSkill)}: {ex.Message}");

        //        // For other exceptions, throw an ApolloApiException with a general error code and message
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.CreateOrUpdateProfileError, "An error occurred while creating or updating profiles.", ex);
        //    }
        //}


        ///// <summary>
        ///// Delete Profiles with specified Ids.
        ///// </summary>
        ///// <param name="deletingIds">The list of profile identifiers.</param>
        ///// <returns>The number of deleted Profiles</returns>
        //public virtual async Task<long> DeleteSkillsAsync(string[] deletingIds)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(DeleteSkillsAsync)}");

        //        // Call the DAL method to delete the profile by their IDs
        //        var res = await _dal.DeleteManyAsync(GetCollectionName<Profile>(), deletingIds, throwIfNotDeleted: false);

        //        _logger?.LogTrace($"Completed {nameof(DeleteSkillsAsync)}");

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(DeleteSkillsAsync)}: {ex.Message}");
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.DeleteProfileError, "Error while deleting profiles", ex);
        //    }
        //}
        //#endregion


        //#region Skill
        ///// <summary>
        ///// Queries for a set of prfiles that match specified criteria.
        ///// </summary>
        ///// <param name="query">The filter that specifies profiles to be retrieved.</param>
        ///// <returns>List of profiles.</returns>
        //// TODO: More specific exception handeling for this method
        //public virtual async Task<IList<Profile>> QuerySkillsAsync(Query query)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(QuerySkillsAsync)}");

        //        var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Profile>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
        //        var profiles = Convertor.ToEntityList<Profile>(res, Convertor.ToProfile);

        //        _logger?.LogTrace($"Completed {nameof(QuerySkillsAsync)}");

        //        return profiles;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(QuerySkillsAsync)}: {ex.Message}");
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.GetProfileError, "Error while querying profiles", ex);
        //    }
        //}


        ///// <summary>
        ///// Creates or Updates the new Profile instance.
        ///// </summary>
        ///// <param name="profile">If the Id is specified, the update will be performed.</param>
        ///// <returns></returns>
        //public virtual async Task<List<string>> CreateOrUpdateSkill(string userId, Profile profile)
        //{
        //    try
        //    {
        //        List<string> ids = new List<string>();

        //        // TODO Validate if profile is null
        //        // TODO Validate if userId exists.
        //        _logger?.LogTrace($"Entered {nameof(CreateOrUpdateProfile)}");

        //        if (profile == null)
        //        {
        //            profile.Id = CreateProfileId(userId);
        //        }

        //        var res = await _dal.IsExistAsync<Profile>(GetCollectionName<Profile>(), profile.Id);
        //        if (res == false)
        //            throw new ApolloApiException(ProfileErrors.CreateOrUpdateProfileUserDoesNotExistError, $"The user {userId} does not exist");

        //        await _dal.UpsertAsync(GetCollectionName<Profile>(), new List<ExpandoObject> { Convertor.Convert(profile) });

        //        _logger?.LogTrace($"Completed {nameof(CreateOrUpdateSkill)}");

        //        return ids;
        //    }
        //    catch (ApolloApiException)
        //    {
        //        //todo. Logging not implemented
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(CreateOrUpdateSkill)}: {ex.Message}");

        //        // For other exceptions, throw an ApolloApiException with a general error code and message
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.CreateOrUpdateProfileError, "An error occurred while creating or updating profiles.", ex);
        //    }
        //}


        ///// <summary>
        ///// Delete Profiles with specified Ids.
        ///// </summary>
        ///// <param name="deletingIds">The list of profile identifiers.</param>
        ///// <returns>The number of deleted Profiles</returns>
        //public virtual async Task<long> DeleteSkillsAsync(string[] deletingIds)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(DeleteSkillsAsync)}");

        //        // Call the DAL method to delete the profile by their IDs
        //        var res = await _dal.DeleteManyAsync(GetCollectionName<Profile>(), deletingIds, throwIfNotDeleted: false);

        //        _logger?.LogTrace($"Completed {nameof(DeleteSkillsAsync)}");

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(DeleteSkillsAsync)}: {ex.Message}");
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.DeleteProfileError, "Error while deleting profiles", ex);
        //    }
        //}
        //#endregion

        //#region Occupation
        ///// <summary>
        ///// Queries for a set of prfiles that match specified criteria.
        ///// </summary>
        ///// <param name="query">The filter that specifies profiles to be retrieved.</param>
        ///// <returns>List of profiles.</returns>
        //// TODO: More specific exception handeling for this method
        //public virtual async Task<IList<Profile>> QueryOccupationsAsync(Query query)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(QueryOccupationsAsync)}");

        //        var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<Profile>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
        //        var profiles = Convertor.ToEntityList<Profile>(res, Convertor.ToProfile);

        //        _logger?.LogTrace($"Completed {nameof(QueryOccupationsAsync)}");

        //        return profiles;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(QueryOccupationsAsync)}: {ex.Message}");
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.GetProfileError, "Error while querying profiles", ex);
        //    }
        //}


        ///// <summary>
        ///// Creates or Updates the new Profile instance.
        ///// </summary>
        ///// <param name="profile">If the Id is specified, the update will be performed.</param>
        ///// <returns></returns>
        //public virtual async Task<List<string>> CreateOrUpdateOccupation(string userId, Profile profile)
        //{
        //    try
        //    {
        //        List<string> ids = new List<string>();

        //        // TODO Validate if profile is null
        //        // TODO Validate if userId exists.
        //        _logger?.LogTrace($"Entered {nameof(CreateOrUpdateProfile)}");

        //        if (profile == null)
        //        {
        //            profile.Id = CreateProfileId(userId);
        //        }

        //        var res = await _dal.IsExistAsync<Profile>(GetCollectionName<Profile>(), profile.Id);
        //        if (res == false)
        //            throw new ApolloApiException(ProfileErrors.CreateOrUpdateProfileUserDoesNotExistError, $"The user {userId} does not exist");

        //        await _dal.UpsertAsync(GetCollectionName<Profile>(), new List<ExpandoObject> { Convertor.Convert(profile) });
              
        //        _logger?.LogTrace($"Completed {nameof(CreateOrUpdateProfile)}");

        //        return ids;
        //    }
        //    catch (ApolloApiException)
        //    {
        //        //todo. Logging not implemented
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(CreateOrUpdateProfile)}: {ex.Message}");

        //        // For other exceptions, throw an ApolloApiException with a general error code and message
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.CreateOrUpdateProfileError, "An error occurred while creating or updating profiles.", ex);
        //    }
        //}


        ///// <summary>
        ///// Delete Profiles with specified Ids.
        ///// </summary>
        ///// <param name="deletingIds">The list of profile identifiers.</param>
        ///// <returns>The number of deleted Profiles</returns>
        //public virtual async Task<long> DeleteOccpationAsync(string[] deletingIds)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(DeleteProfiles)}");

        //        // Call the DAL method to delete the profile by their IDs
        //        var res = await _dal.DeleteManyAsync(GetCollectionName<Profile>(), deletingIds, throwIfNotDeleted: false);

        //        _logger?.LogTrace($"Completed {nameof(DeleteProfiles)}");

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(DeleteProfiles)}: {ex.Message}");
        //        throw new ApolloApiException(ErrorCodes.ProfileErrors.DeleteProfileError, "Error while deleting profiles", ex);
        //    }
        //}
        //#endregion

    }
}
