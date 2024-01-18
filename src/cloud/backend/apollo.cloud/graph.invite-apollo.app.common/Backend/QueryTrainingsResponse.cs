// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class QueryTrainingsResponse
    {
        public List<Training> Trainings { get; set; }
    }
}
