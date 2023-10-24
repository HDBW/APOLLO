// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.RestService.Messages
{
    /// <summary>
    /// Defines the response message of the CreateOrUpdateTraining operation.
    /// </summary>
    public class CreateOrUpdateTrainingResponse
    {
        /// <summary>
        /// The Id of the created training instance.
        /// </summary>
        public Training Training{ get; set; }
    }
}
