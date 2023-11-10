﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the request message of the CreateOrUpdateTraining operation.
    /// </summary>
    public class CreateOrUpdateTrainingRequest
    {
        public Training Training { get; set; }
    }
}