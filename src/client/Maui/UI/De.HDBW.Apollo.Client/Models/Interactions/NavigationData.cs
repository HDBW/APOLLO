namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public record NavigationData(string route, NavigationParameters? parameters)
    {
        public string Route { get; init; } = route;

        public NavigationParameters? Parameters { get; init; } = parameters;
    }
}
