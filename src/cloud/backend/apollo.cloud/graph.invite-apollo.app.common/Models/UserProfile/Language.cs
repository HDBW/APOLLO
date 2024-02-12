// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Lists;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Language
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public ApolloListItem? Niveau { get; set; }

        // CultureInfo get Culture ISO639-2 
        public string Code { get; set; }
    }
}
