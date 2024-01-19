﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Dynamic;
using Apollo.Common.Entities;
using Microsoft.Extensions.Logging;

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
        /// Retrieves users with a specific goal.
        /// </summary>
        /// <param name="goal">The goal to filter users by.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of User objects with the specified goal.</returns>
        // NOTE: Goal field doesnt exist in user entity anymore
        //public async Task<IList<User>> QueryUsersByGoal(string goal)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(QueryUsersByGoal)}");

        //        var queryFilter = new Filter
        //        {
        //            Fields = new List<FieldExpression> { new FieldExpression { FieldName = "Goal", Operator = QueryOperator.Equals, Argument = new List<object> { goal } } }
        //        };

        //        var query = new Query
        //        {
        //            Fields = new List<string>(),
        //            Filter = queryFilter,
        //            Top = 100,
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(ApolloApi.GetCollectionName<User>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, Convertor.ToDaenetSortExpression(query.SortExpression));
        //        var users = Convertor.ToEntityList<User>(res, Convertor.ToUser);

        //        _logger?.LogTrace($"Completed {nameof(QueryUsersByGoal)}");

        //        if (users.Count == 0)
        //        {
        //            // No users found with the specified goal, throw an ApolloException with a specific code and message
        //            throw new ApolloApiException(ErrorCodes.UserErrors.NoUsersFoundByGoal, $"No users found with goal '{goal}'.", new Exception("Exeption while quering users by goal"));
        //        }

        //        return users;
        //    }
        //    catch (ApolloApiException)
        //    {

        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Failed execution of {nameof(QueryUsersByGoal)}: {ex.Message}");

        //        // For other exceptions, throw an ApolloApiException with a general error code and message
        //        throw new ApolloApiException(ErrorCodes.UserErrors.QueryUsersByGoalError, "An error occurred while querying users by goal.", ex);
        //    }
        //}


        /// <summary>
        /// Searches for users based on a keyword in their names.
        /// </summary>
        /// <param name="keyword">The keyword to search in user's names.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of User objects that match the keyword.</returns>
        //public async Task<IList<User>> QueryUsersByKeyword(string keyword)
        //{
        //    try
        //    {
        //        var query = new Query
        //        {
        //            // Specifying which fields to return in the query results
        //            Fields = new List<string> { "Name", "Email", "ObjectId", "Upn", "ContactInfos", "Birthdate" },
        //            Filter = new Filter
        //            {
        //                Fields = new List<FieldExpression>
        //        {
        //            new FieldExpression { FieldName = "Name", Operator = QueryOperator.Contains, Argument = new List<object> { keyword } },
        //            new FieldExpression { FieldName = "Email", Operator = QueryOperator.Contains, Argument = new List<object> { keyword } }
        //            // You can add other fields to be queried if necessary
        //        }
        //            },
        //            Top = 100,
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(GetCollectionName<User>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);

        //        if (res == null || !res.Any())
        //        {
        //            // No results found, throw an ApolloException with a specific code and message
        //            throw new ApolloApiException(ErrorCodes.UserErrors.NoUsersFoundByKeyword, $"No users found with keyword '{keyword}'.");
        //        }

        //        return Convertor.ToEntityList<User>(res, Convertor.ToUser);
        //    }
        //    catch (ApolloApiException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        // For other exceptions, throw an ApolloApiException with a general error code and message
        //        throw new ApolloApiException(ErrorCodes.UserErrors.QueryUsersByKeywordError, "Error while querying users by keyword", ex);
        //    }
        //}


        /// <summary>
        /// Retrieves users based on multiple search criteria including name, email, and profile.
        /// </summary>
        /// <param name="name">The first name to filter by.</param>
        /// <param name="email">The last name to filter by.</param>
        /// <param name="profileCriteria">The goal to filter by.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of User objects that match the specified criteria.</returns>
        //public async Task<IList<User>> QueryUsersByMultipleCriteria(string name, string email, string profileCriteria)
        //{
        //    try
        //    {
        //        var query = new Query
        //        {
        //            Fields = new List<string> { "Name", "Email", "Profile" },
        //            Filter = new Filter
        //            {
        //                Fields = new List<FieldExpression>
        //        {
        //            new FieldExpression { FieldName = "Name", Operator = QueryOperator.Equals, Argument = new List<object> { name } },
        //            new FieldExpression { FieldName = "Email", Operator = QueryOperator.Equals, Argument = new List<object> { email } },
        //            new FieldExpression { FieldName = "Profile.SomeProfileField", Operator = QueryOperator.Equals, Argument = new List<object> { profileCriteria } }
        //        }
        //            },
        //            Top = 100,
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(GetCollectionName<User>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);

        //        if (res == null || !res.Any())
        //        {
        //            // No results found, throw an ApolloException with specific code and message
        //            throw new ApolloApiException(ErrorCodes.UserErrors.QueryUsersByMultipleCriteriaError, "No users found matching the criteria.");
        //        }

        //        return Convertor.ToEntityList<User>(res, Convertor.ToUser);
        //    }
        //    catch (ApolloApiException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        // For other exceptions, throw an ApolloApiException with a general error code and message
        //        throw new ApolloApiException(ErrorCodes.UserErrors.QueryUsersByMultipleCriteriaError, "Error while querying users by multiple criteria", ex);
        //    }
        //}


   
        /// <summary>
        /// Retrieves users by their first name.
        /// </summary>
        /// <param name="firstName">The first name to filter users by.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of User objects with the specified first name.</returns>
        //public async Task<IList<User>> QueryUsersByFirstName(string firstName)
        //{
        //    try
        //    {
        //        var query = new Query
        //        {
        //            Fields = new List<string> { /* Add field names to return here */ },
        //            Filter = new Filter
        //            {
        //                Fields = new List<FieldExpression> { new FieldExpression { FieldName = "FirstName", Operator = QueryOperator.Equals, Argument = new List<object> { firstName } } }
        //            },
        //            Top = 100,
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(GetCollectionName<User>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
        //        return Convertor.ToEntityList<User>(res, Convertor.ToUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApolloApiException(ErrorCodes.UserErrors.QueryUsersByFirstNameError, "Error while querying users by first name", ex);
        //    }
        //}


        /// <summary>
        /// Retrieves users by their last name.
        /// </summary>
        /// <param name="lastName">The last name to filter users by.</param>
        /// <returns>Task that represents the asynchronous operation, containing a list of User objects with the specified last name.</returns>
        //public async Task<IList<User>> QueryUsersByLastName(string lastName)
        //{
        //    try
        //    {
        //        _logger?.LogTrace($"Entered {nameof(QueryUsersByLastName)}");

        //        var query = new Query
        //        {
        //            Fields = new List<string>(), // Specify the required fields
        //            Filter = new Filter
        //            {
        //                Fields = new List<FieldExpression>
        //        {
        //            new FieldExpression { FieldName = "LastName", Operator = QueryOperator.Equals, Argument = new List<object> { lastName } }
        //        }
        //            },
        //            Top = 100,
        //            Skip = 0
        //        };

        //        var res = await _dal.ExecuteQuery(GetCollectionName<User>(), query.Fields, Convertor.ToDaenetQuery(query.Filter), query.Top, query.Skip, null);
        //        return Convertor.ToEntityList<User>(res, Convertor.ToUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, $"Failed execution of {nameof(QueryUsersByLastName)}: {ex.Message}");
        //        throw new ApolloApiException(ErrorCodes.UserErrors.QueryUsersByLastNameError, "Error while querying users by last name", ex);
        //    }
        //}


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

                // Generate a unique user ID if it's not provided
                if (String.IsNullOrEmpty(user.Id) && String.IsNullOrEmpty(user.ObjectId))
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
        /// Creates or Updates the new User instance.
        /// </summary>
        /// <param name="user">If the Id is specified, the update will be performed.</param>
        /// <returns></returns>
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
                                await _dal.UpsertAsync(GetCollectionName<User>(),new List<ExpandoObject> { Convertor.Convert(user) } );
                            }
                            else
                            {
                                user.Id = CreateUserId();
                                await _dal.InsertAsync(ApolloApi.GetCollectionName<User>(), Convertor.Convert(user)); ;
                            }
                        }
                    }
                }



                _logger?.LogTrace($"Completed {nameof(CreateOrUpdateUser)}");

                return ids;
            }
            catch (ApolloApiException)
            {

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
