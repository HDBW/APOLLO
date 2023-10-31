// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Security.Claims;
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
        internal ClaimsPrincipal? Principal { get; set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string? User
        {
            get
            {
                string? usr = Principal?.Identity?.Name;
                return String.IsNullOrEmpty(usr)  ? "anonymous" : usr;
            }
        }

        public ApolloApi(MongoDataAccessLayer dal, ILogger<ApolloApi> logger)
        {
            _logger = logger;
            _dal = dal;
        }
    }
}
