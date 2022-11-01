// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public record NavigationData(string route, NavigationParameters? parameters)
    {
        public string Route { get; init; } = route;

        public NavigationParameters? Parameters { get; init; } = parameters;
    }
}
