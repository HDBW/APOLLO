// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;

namespace Apollo.RestService.Apollo.Common.Messages
{
    public class CreateOrUpdateUserRequest
    {
        public User User { get; internal set; }
    }
}
