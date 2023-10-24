// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Apollo.Api
{
    /// <summary>
    /// Implements all Apollo Business functionalities.
    /// </summary>
    public partial class ApolloApi
    {
        private readonly ILogger<ApolloApi> _logger;

        public ClaimsPrincipal Principal { get; set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string? User
        {
            get
            {
                // TODO GDPR??
                string usr = Principal?.Identity?.Name;
                return String.IsNullOrEmpty(usr)  ? "anonymous" : usr;
            }
        }

        public ApolloApi(ILogger<ApolloApi> logger)
        {
            _logger = logger;
        }
    }
}
