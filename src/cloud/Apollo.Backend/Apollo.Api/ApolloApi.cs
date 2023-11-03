// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Dynamic;
using System.Security.Claims;
using Apollo.Common.Entities;
using Daenet.MongoDal;
using Microsoft.Extensions.Logging;


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
            ICollection<ExpandoObject> list = null;

            try
            {
                List<ExpandoObject> expoTrainings = Convertor.Convert(trainings);

                await _dal.InsertManyAsync(GetCollectionName("training"), expoTrainings);
            }
            catch (Exception ex)
            {
                //todo logger.
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

    }
}
