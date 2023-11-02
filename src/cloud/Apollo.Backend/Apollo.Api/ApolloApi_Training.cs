// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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
        /// Gets the specific instance of the training.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public Task<T> GetTraining(string trainingId)
        {
            try
            {
                _logger.LogTrace($"{this.User} entered {nameof(GetTraining)}");

                var res = Task.FromResult<T>(new T());

                _logger.LogTrace($"{this.User} completed {nameof(GetTraining)}");

                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.User} failed execution of {nameof(GetTraining)}");
                throw;
            }
        }


        /// <summary>
        /// Queries for a set of trainings that match specified criteria.
        /// </summary>
        /// <param name="filter">The filter that specifies trainings to be retrieved.</param>
        /// <returns>List of trainings.</returns>
        public Task<IList<T>> QueryTrainings(QueryTrainings filter)
        {
            return Task.FromResult<IList<T>>(new List<T>());
        }

        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public Task<string> CreateOrUpdateTraining(T training)
        {
            return Task.FromResult<string>(Guid.NewGuid().ToString());
        }


        /// <summary>
        /// Delete Trainings with specified Ids.
        /// </summary>
        /// <param name="deletingIds">The list of training identifiers.</param>
        /// <returns>The numbe rof deleted trainings.</returns>
        public Task<int> DeleteTrainings(int[] deletingIds)
        {
            return Task.FromResult<int>(42);

        }

    }
}
