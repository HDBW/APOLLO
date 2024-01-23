// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Dynamic;
using Apollo.Common.Entities;
using Microsoft.Extensions.Logging;
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
        /// Gets the specific instance of the user.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public virtual async Task<User> GetUser(string userId)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(GetUser)}");

                var user = await _dal.GetByIdAsync<User>(ApolloApi.GetCollectionName<User>(), userId);

                _logger?.LogTrace($"Completed {nameof(GetUser)}");

                if (user == null)
                {
                    user = await _dal.GetByIdAsync<User>(ApolloApi.GetCollectionName<User>(), userId, "ObjectId");

                    if (user == null)

                        // User not found, throw ApolloException with specific code and message
                        throw new ApolloApiException(ErrorCodes.UserErrors.UserNotFound, $"User with ID '{userId}' not found.");
                }

                return user;
            }
            catch (ApolloApiException)
            {

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed execution of {nameof(GetUser)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.UserErrors.GetUserError, "An error occurred while getting the user.", ex);
            }
        }


        /// <summary>
        /// Queries for a set of users that match specified criteria.
        /// </summary>
        /// <param name="query">The filter that specifies users to be retrieved.</param>
        /// <returns>List of users.</returns>
        // TODO: More specific exception handeling for this method
        public virtual async Task<IList<User>> QueryUsers(Query query)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(QueryUsers)}");

                var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<User>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
                var users = Convertor.ToEntityList<User>(res, Convertor.ToUser);

                _logger?.LogTrace($"Completed {nameof(QueryUsers)}");

                return users;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(QueryUsers)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.UserErrors.QueryUsersError, "Error while querying users", ex);
            }
        }


        /// <summary>
        /// Inserts a new user into the system.
        /// </summary>
        /// <param name="user">The User object to be inserted.</param>
        /// <returns>Task that represents the asynchronous operation, containing the unique identifier of the inserted user.</returns>
        public virtual async Task<string> InsertUser(User user)
        {
            try
            {
                _logger?.LogTrace($"{this.User} entered {nameof(InsertUser)}");

                user.Id = CreateUserId();

                // Check if the user with the same ID already exists before inserting
                var existingUser = await _dal.GetByIdAsync<User>(ApolloApi.GetCollectionName<User>(), user.Id);
                if (existingUser != null)
                {
                    // User with the same ID already exists, throw an ApolloException with a specific code and message
                    throw new ApolloApiException(ErrorCodes.UserErrors.UserAlreadyExists, $"User with ID '{user.Id}' already exists.");
                }

                await _dal.InsertAsync(ApolloApi.GetCollectionName<User>(), Convertor.Convert(user));

                _logger?.LogTrace($"{this.User} completed {nameof(InsertUser)}");
                _logger?.LogTrace($"Inserting user with Id: {user.Id}");

                return user.Id;
            }
            catch (ApolloApiException)
            {

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{this.User} failed execution of {nameof(InsertUser)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.UserErrors.InsertUserError, "An error occurred while inserting the user.", ex);
            }
        }


        /// <summary>
        /// Creates or Updates the new User.
        /// </summary>
        /// <param name="user">If neither Id nor ObjectId is set, the new user will be created.
        /// If the Id or ObjectId is specified, the update will be performed.</param>
        /// <remarks>Please note the update operation with specified Id is is faster and produces lower costs than update operation with ObjectId.</remarks>
        /// <returns>The Id of the user.</returns>
        public virtual async Task<List<string>> CreateOrUpdateUser(ICollection<User> users)
        {
            try
            {
                List<string> ids = new List<string>();

                _logger?.LogTrace($"Entered {nameof(CreateOrUpdateUser)}");

                foreach (var user in users)
                {
                    if (String.IsNullOrEmpty(user.Id) && String.IsNullOrEmpty(user.ObjectId))
                    {
                        user.Id = CreateUserId();
                        await _dal.InsertAsync(ApolloApi.GetCollectionName<User>(), Convertor.Convert(user));
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(user.Id) && !String.IsNullOrEmpty(user.ObjectId))
                        {
                            var existingUser = await _dal.GetByIdAsync<User>(ApolloApi.GetCollectionName<User>(), user.ObjectId, nameof(user.ObjectId));
                            if (existingUser != null)
                            {
                                user.Id = existingUser.Id;
                                await _dal.UpsertAsync(GetCollectionName<User>(), new List<ExpandoObject> { Convertor.Convert(user) });
                            }
                            else
                            {
                                throw new ApolloApiException(UserErrors.CreateOrUpdateUserError, $"The user with the specified ObjectId = {user.ObjectId} does not exist.");
                             }
                        }
                    }
                }

                _logger?.LogTrace($"Completed {nameof(CreateOrUpdateUser)}");

                return ids;
            }
            catch (ApolloApiException)
            {
                // TODO Logging is missing
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(CreateOrUpdateUser)}: {ex.Message}");

                // For other exceptions, throw an ApolloApiException with a general error code and message
                throw new ApolloApiException(ErrorCodes.UserErrors.CreateOrUpdateUserError, "An error occurred while creating or updating users.", ex);
            }
        }


        /// <summary>
        /// Delete Users with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of user identifiers.</param>
        /// <returns>The number of deleted users.</returns>
        public virtual async Task<long> DeleteUsers(string[] deletingIds)
        {
            try
            {
                _logger?.LogTrace($"Entered {nameof(DeleteUsers)}");

                // Call the DAL method to delete the users by their IDs
                var res = await _dal.DeleteManyAsync(GetCollectionName<User>(), deletingIds, throwIfNotDeleted: false);

                _logger?.LogTrace($"Completed {nameof(DeleteUsers)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed execution of {nameof(DeleteUsers)}: {ex.Message}");
                throw new ApolloApiException(ErrorCodes.UserErrors.DeleteUserError, "Error while deleting users", ex);
            }
        }
    }
}
