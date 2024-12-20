﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Lists;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class LeadershipSkills
    {
        public bool PowerOfAttorney { get; set; }

        public bool BudgetResponsibility { get; set; }

        public ApolloListItem YearsofLeadership { get; set; }

        public ApolloListItem StaffResponsibility { get; set; }
    }
}
