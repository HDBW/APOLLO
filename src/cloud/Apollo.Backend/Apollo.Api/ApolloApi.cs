// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Dynamic;
using System.Security.Claims;
using Amazon.Runtime.Internal.Util;
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
        //reason of addition: fixes the error for UnitTests
        public ApolloApi()
        {
            //Empty constructor
        }
        #region Fields and Properties


        private readonly ILogger<ApolloApi> _logger;

        private readonly MongoDataAccessLayer _dal;

        private readonly ApolloApiConfig _config;


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
                return string.IsNullOrEmpty(usr) ? "anonymous" : usr;
            }
        }
        #endregion


        /// <summary>
        /// Docuement me please :P
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ApolloApi(MongoDataAccessLayer dal, ILogger<ApolloApi> logger, ApolloApiConfig config)
        {
            _dal = dal;
            _logger = logger;
            _config = config;
            //// Validate the logger. 
            //if (logger == null)
            //{
            //    //We can do this because Damir chose appservices and they have system diagnostics
            //    System.Diagnostics.Debug.WriteLine("DI Logger exception incomming !!!");
            //    //And then we make sure the constructor is not called with a null logger.
            //    ArgumentNullException.ThrowIfNull(logger);
            //}
            //else
            //{
            //    _logger = logger;
            //}

            try
            {
                if (_dal == null)
                {
                    throw new ArgumentNullException(nameof(dal));
                }

                if (_config == null)
                {
                    throw new ArgumentNullException(nameof(config));
                }

                ValidateConfig(_config);

                // Additional initialization or method calls can be added here.
            }
            catch (ApolloApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"An error occurred in ApolloApi constructor: {ex.Message}", ex);
                throw new ApolloApiException(ErrorCodes.GeneralErrors.OperationFailed, "An error occurred while initializing ApolloApi.", ex);
            }
        }


        /// <summary>
        /// Gets the name of the collection for the specified item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetCollectionName(object item)
        {
            //TODO: Why object? 
            try
            {
                //if (item == null)
                //    throw new ArgumentNullException(nameof(item));
                ArgumentNullException.ThrowIfNull(item);

                // Assuming there's another instance or static method to get the name
                return GetCollectionName(item.GetType().Name);
            }
            catch (ApolloApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error in GetCollectionName: {ex.Message}", ex);
                throw new ApolloApiException(ErrorCodes.GeneralErrors.OperationFailed, "An error occurred while getting the collection name.", ex);
            }
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
        private static string? GetCollectionName(string typeName)
        {
            //TODO: The original code was not null safe. I changed to explicit is always better mode. Now that would return null but if you don't want that you can use the original code. And throw a null exception I guess? 
            return $"{typeName?.ToLower()}s";
        }


        /// <summary>
        /// Validates the configuration properties.
        /// </summary>
        private void ValidateConfig(ApolloApiConfig config)
        {
            //// Validation logic to ensure configuration properties are set correctly
            //if (string.IsNullOrWhiteSpace(config.ApiKey))
            //{
            //    throw new InvalidOperationException("API key is not configured.");
            //}

            //if (string.IsNullOrWhiteSpace(config.ServiceUrl))
            //{
            //    throw new InvalidOperationException("Base URL is not configured.");
            //}

            // Add further validation as required for other properties
        }


        /// <summary>
        /// Creates the unique identifier for the new training instance.
        /// </summary>
        /// <returns></returns>
        private string CreateTrainingId()
        {
            try
            {
                return CreateId(nameof(Training));
            }
            catch (ApolloApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error in CreateTrainingId: {ex.Message}", ex);
                throw new ApolloApiException(ErrorCodes.GeneralErrors.OperationFailed, "An error occurred while creating a training ID.", ex);
            }
        }


        /// <summary>
        /// Creates the unique identifier for the new user instance.
        /// </summary>
        /// <returns></returns>
        private string CreateUserId()
        {
            try
            {
                return CreateId(nameof(User));
            }
            catch (ApolloApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error in CreateUserId: {ex.Message}", ex);
                throw new ApolloApiException(ErrorCodes.GeneralErrors.OperationFailed, "An error occurred while creating a user ID.", ex);
            }
        }


        /// <summary>
        /// Creates the unique identifier for the new profile instance.
        /// The profile id contains the same identifier as userId preficed with the profile.
        /// </summary>
        /// <returns></returns>
        private static string CreateProfileId(string userId)
        {
            // Creating the default version of the profile identifier.
            return $"{FormatId(nameof(Profile), userId)}_v01";
        }


        /// <summary>
        /// Creates the Id for list of items.
        /// </summary>
        /// <param name="itemType">The type of the item in the list</param>
        /// <returns></returns>
        private static string CreateListId(string itemType)
        {
            return CreateId($"{itemType}-{nameof(List)}");
        }


        /// <summary>
        /// Creates the unique identifier for the new instance of the specified entity.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private static string CreateId(string entityName)
        {
            return FormatId(entityName, Guid.NewGuid().ToString("N").ToUpper());
        }

        private static string FormatId(string entityName, string id)
        {
            return $"{entityName}-{id}";
        }

    }
}
