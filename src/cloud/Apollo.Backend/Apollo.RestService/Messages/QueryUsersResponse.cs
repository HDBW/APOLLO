// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

public class QueryUsersResponse
{
    public QueryUsersResponse(object users)
    {
        Users = users as List<Apollo.Common.Entities.User> ?? new List<Apollo.Common.Entities.User>();
    }

    public List<Apollo.Common.Entities.User> Users { get; set; }
    // ... other properties or methods
}

