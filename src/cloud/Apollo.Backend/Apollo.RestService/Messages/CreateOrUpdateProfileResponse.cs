// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.RestService.Messages
{
    // Define a response message for creating or updating a user.
    public class CreateOrUpdateProfileResponse
    {
        // Property to store the result of the create/update operation.
        public object Result { get; internal set; }
    }
}
