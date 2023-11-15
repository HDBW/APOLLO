// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.



using Apollo.Common.Entities;

public class QueryUsersResponse
{
    public QueryUsersResponse(object users)
    {
            
    }

    public Query Query { get; set; }

    public List<Apollo.Common.Entities.User> Users { get; set; }
    // ... other properties or methods
}
