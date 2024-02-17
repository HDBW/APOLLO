// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.Common.Entities;

public class QueryProfilesResponse
{
    public QueryProfilesResponse(object profiles)
    {
        Profiles = profiles as List<Apollo.Common.Entities.Profile> ?? new List<Apollo.Common.Entities.Profile>();
    }

    public List<Apollo.Common.Entities.Profile> Profiles { get; set; }
   
}

