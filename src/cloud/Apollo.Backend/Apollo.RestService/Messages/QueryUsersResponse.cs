// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Services.Grpc;

public class QueryUsersResponse
{
    public QueryUsersResponse(object users)
    {
    }

    public List<Apollo.Common.Entities.User> Users { get; set; }
    // ... other properties or methods
}
