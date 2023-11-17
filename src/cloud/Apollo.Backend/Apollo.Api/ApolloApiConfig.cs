// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Api
{
    public class ApolloApiConfig
    {
        public string ?ApiKey{ get; set; }
        public string ?ServiceUrl { get; set; }
        // Add other configuration properties as needed


        public void Validate()
        {
            // Validation logic to ensure configuration properties are set correctly
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new InvalidOperationException("API key is not configured.");
            }

            if (string.IsNullOrWhiteSpace(ServiceUrl))
            {
                throw new InvalidOperationException("Base URL is not configured.");
            }

            // Add further validation as required for other properties
        }



    }
}
