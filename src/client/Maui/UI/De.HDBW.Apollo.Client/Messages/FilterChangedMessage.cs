using Invite.Apollo.App.Graph.Common.Backend.Api;

namespace De.HDBW.Apollo.Client.Messages
{
    public class FilterChangedMessage
    {
        public FilterChangedMessage(Filter? filter)
        {
            if (filter?.Fields.Any() ?? false)
            {
                Filter = filter;
            }
        }

        public Filter? Filter { get; }
    }
}
