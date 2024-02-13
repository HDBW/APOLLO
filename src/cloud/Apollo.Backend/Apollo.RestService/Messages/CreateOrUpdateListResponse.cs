﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the response message of the CreateOrUpdateTraining operation.
    /// </summary>
    public class CreateOrUpdateListResponse
    {
        /// <summary>
        /// The set of list items.
        /// </summary>
        public ApolloList? Result{ get; set; }
    }
}