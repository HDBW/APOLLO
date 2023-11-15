// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.Api
{
    /// <summary>
    /// Implements all Apollo Business functionalities.
    /// </summary>
    public partial class ApolloApi
    {

        /// <summary>
        /// Gets the specific instance of the user.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public Task<User> GetUser(string trainingId)
        {
            return Task.FromResult<User>(new User());
        }


        /// <summary>
        /// Queries for a set of users that match specified criteria.
        /// </summary>
        /// <param name="filter">The filter that specifies trainings to be retrieved.</param>
        /// <returns>List of trainings.</returns>
        public Task<IList<User>> QueryUser(Query query)
        {
            return Task.FromResult<IList<User>>(new List<User>());
        }

        /// <summary>
        /// Creates or Updates the new User instance.
        /// </summary>
        /// <param name="user">If the Id is specified, the update will be performed.</param>
        /// <returns></returns>
        public Task<string> CreateOrUpdateUser(User user)
        {
            return Task.FromResult<string>(Guid.NewGuid().ToString());
        }


        /// <summary>
        /// Delete Users with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of user identifiers.</param>
        /// <returns>The number of deleted users.</returns>
        public Task<int> DeleteUser(int[] deletingIds)
        {
            return Task.FromResult<int>(42);

        }
    }
}
