// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Dynamic;
using System.Security.Claims;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Apollo.Api
{
    /// <summary>
    /// Implements all Apollo Business functionalities.
    /// </summary>
    public partial class ApolloApi
    {
        private readonly ILogger<ApolloApi> _logger;


        private readonly MongoDataAccessLayer _dal;


        /// <summary>
        /// Set by <see cref="ApiPrincipalFilter"/>.
        /// </summary>
        public ClaimsPrincipal? Principal { get; set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string? User
        {
            get
            {
                string? usr = Principal?.Identity?.Name;
                return String.IsNullOrEmpty(usr) ? "anonymous" : usr;
            }
        }
        public ApolloApi(MongoDataAccessLayer dal, ILogger<ApolloApi> logger)
        {
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Inserts a list of trainings.
        /// </summary>
        /// <param name="trainings">The list of trainings to insert.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task InsertTrainings(List<Training> trainings)
        {
            try
            {
                _logger.LogTrace($"{this.User} entered {nameof(InsertTrainings)}");

                // Convert API trainings to DAL trainings
                var dalTrainings = Convertor.Convert(trainings);

                // Insert DAL trainings
                foreach (var dalTraining in dalTrainings)
                {
                    await _dal.InsertAsync(GetCollectionName<Training>(), dalTraining);
                }

                _logger.LogTrace($"{this.User} completed {nameof(InsertTrainings)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.User} failed execution of {nameof(InsertTrainings)}");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing training.
        /// </summary>
        /// <param name="training">The updated training data.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        //public async Task UpdateTraining(Training training)
        //{
        //    try
        //    {
        //        _logger.LogTrace($"{this.User} entered {nameof(UpdateTraining)}");

        //        // Convert API training to DAL training
        //        var dalTraining = Convertor.Convert(training);

        //        // Update DAL training
        //        await _dal.UpdateAsync(GetCollectionName<Training>(), dalTraining);

        //        _logger.LogTrace($"{this.User} completed {nameof(UpdateTraining)}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"{this.User} failed execution of {nameof(UpdateTraining)}");
        //        throw;
        //    }
        //}



        public async Task DeleteTrainings(List<string> trainingIds)
        {
            // Perform validation and exception handling if needed.

            foreach (var trainingId in trainingIds)
            {
                // Delete the training with the specified ID.
                await DeleteTrainingById(trainingId);
            }
        }

        private async Task DeleteTrainingById(string trainingId)
        {
         
            string collectionName = "Trainings"; 

            try
            {
                // Use your DAL (Data Access Layer) methods to perform the deletion.
                await _dal.DeleteAsync(collectionName, trainingId);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error deleting training with ID {trainingId}: {ex.Message}");
                throw; // Propagate the exception to the calling code.
            }
        }

        public async Task InsertUsers(ICollection<User> users)
        {
            try
            {
                _logger.LogTrace("InsertUsers method entered.");

                List<ExpandoObject> expoUsers = Convertor.Convert(users);

                // Log information before inserting data.
                _logger.LogInformation($"Inserting {expoUsers.Count} users into the database.");

                await _dal.InsertManyAsync(GetCollectionName("user"), expoUsers);

                // Log information after a successful insertion.
                _logger.LogInformation($"Inserted {expoUsers.Count} users into the database.");
            }
            catch (Exception ex)
            {
                // Log an error in case of an exception.
                _logger.LogError($"Error in InsertUsers method: {ex.Message}");

                // Re-throw the exception
                throw;
            }
        }

        public async Task<User> GetUserById(string userId)
        {
            try
            {
                _logger.LogTrace($"GetUserById method entered for UserId: {userId}");

                // Invoke the corresponding DAL method to get the user by Id.
                var user = await _dal.GetByIdAsync<User>(Helpers.GetCollectionName<User>(), userId);

                // Log information after a successful retrieval.
                _logger.LogInformation($"Retrieved user by UserId: {userId}");

                return user;
            }
            catch (Exception ex)
            {
                // Log an error in case of an exception.
                _logger.LogError($"Error in GetUserById method for UserId: {userId}, Error: {ex.Message}");

                // Re-throw the exception.
                throw;
            }
        }



        /// <summary>
        /// Gets the name of the collection for the specified item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetCollectionName(object item)
        {
            return GetCollectionName(item.GetType().Name);
        }

        /// <summary>
        /// Gets the name of the collection from type. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetCollectionName<T>()
        {
            return GetCollectionName(typeof(T).Name);
        }

        /// <summary>
        /// Creates the name of the collection from the given type name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static string GetCollectionName(string typeName)
        {
            return $"{typeName.ToLower()}s";
        }


        // Added helpers class here to remove error
        // TODO: Add reference to original Helpers class 
        private class Helpers
        {
            internal static string GetCollectionName<T>()
            {
                return ApolloApi.GetCollectionName<T>();
            }

            internal static void LogError(ILogger logger, string message)
            {
                logger.LogError(message);
            }

            internal static void LogInformation(ILogger logger, string message)
            {
                logger.LogInformation(message);
            }

            internal static void LogTrace(ILogger logger, string message)
            {
                logger.LogTrace(message);
            }

            internal static void LogExceptionAndThrow(ILogger logger, string errorMessage, Exception exception)
            {
                logger.LogError($"{errorMessage}: {exception.Message}");
                throw exception;
            }
        }
    }
}
