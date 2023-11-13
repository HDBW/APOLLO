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
        /// note it was changed from internal to public because it was causing issues in the ApiPrincipalFilter class
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
            _logger = logger;
            _dal = dal;
        }

        public async Task InsertTrainings(ICollection<Training> trainings)
        {
            try
            {
                _logger.LogTrace("InsertTrainings method entered.");

                List<ExpandoObject> expoTrainings = Convertor.Convert(trainings);

                // Log information before inserting data.
                _logger.LogInformation($"Inserting {expoTrainings.Count} trainings into the database.");

                await _dal.InsertManyAsync(GetCollectionName("training"), expoTrainings);

                // Log information after a successful insertion.
                _logger.LogInformation($"Inserted {expoTrainings.Count} trainings into the database.");
            }
            catch (Exception ex)
            {
                // Log an error in case of an exception.
                _logger.LogError($"Error in InsertTrainings method: {ex.Message}");

                // Re-throw the exception
                throw;
            }
        }

        public async Task<Training> GetTrainingById(string trainingId)
        {
            try
            {
                _logger.LogTrace($"GetTrainingById method entered for trainingId: {trainingId}");

                var filter = Builders<BsonDocument>.Filter.Eq("_id", trainingId);
                var training = await _dal.GetByIdAsync<Training>(Helpers.GetCollectionName<Training>(), trainingId);

                // Log information after successful retrieval.
                _logger.LogInformation($"Retrieved training with ID: {trainingId}");

                return training;
            }
            catch (Exception ex)
            {
                // Log an error in case of an exception.
                _logger.LogError($"Error in GetTrainingById method: {ex.Message}");

                // Re-throw the exception
                throw;
            }
        }

        public async Task UpdateTrainings(List<Training> updatedTrainings)
        {
            foreach (var updatedTraining in updatedTrainings)
            {
                throw new NotImplementedException();
                //TODO
                //await _dal.UpsertAsync()
                //UpdateTraining(updatedTraining.Id, updatedTraining);
            }
        }

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
