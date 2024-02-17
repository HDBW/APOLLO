// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.RestService.Messages
{
   
    public class GetUserResponse
    {
        /// <summary>
        /// Property to hold the retrieved user information
        /// </summary>
        public User User { get; internal set; }
    }
}
