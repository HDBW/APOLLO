// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;
using Newtonsoft.Json.Linq;

namespace Apollo.RestService.Messages
{
    public class GetProfileResponse
    {
        /// <summary>
        ///  Will return response as type profile
        /// </summary>
        public Profile? Profile { get; set; }
    }
}
