// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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
