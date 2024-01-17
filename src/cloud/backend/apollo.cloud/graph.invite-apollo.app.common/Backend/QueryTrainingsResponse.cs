using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class QueryTrainingsResponse
    {
        public List<Training> Trainings { get; set; }
    }
}
