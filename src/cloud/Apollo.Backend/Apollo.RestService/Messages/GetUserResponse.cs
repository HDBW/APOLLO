// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.RestService.Messages
{
    // Define a response message for retrieving a user.
    public class GetUserResponse
    {
        // Property to hold the retrieved user information
        public User User { get; internal set; }
    }
}
