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
        /// Gets the specific instance of the training.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public Task<Training> GetTraining(string trainingId)
        {
            return Task.FromResult<Training>(new Training());
        }


        /// <summary>
        /// Queries for a set of trainings that match specified criteria.
        /// </summary>
        /// <param name="filter">The filter that specifies trainings to be retrieved.</param>
        /// <returns>List of trainings.</returns>
        public Task<IList<Training>> QueryTrainings(QueryTrainings filter)
        {
            return Task.FromResult<IList<Training>>(new List<Training>());
        }

        /// <summary>
        /// Creates the new Trainng instance 
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public Task<string> CreateOrUpdateTraining(Training training)
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
