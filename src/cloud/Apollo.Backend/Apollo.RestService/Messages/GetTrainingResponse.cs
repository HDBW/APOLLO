// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

namespace Apollo.RestService.Messages
{
    public class GetTrainingResponse
    {
        public T Training { get; set; }
    }
}
