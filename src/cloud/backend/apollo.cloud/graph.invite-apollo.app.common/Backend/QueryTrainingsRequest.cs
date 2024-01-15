using System;
using System.Collections.Generic;
using System.Text;
using Apollo.Common.Entities;

namespace Invite.Apollo.App.Graph.Common.Backend.RestService.Messages
{
    internal class QueryTrainingsRequest : Query
    {
        Query Query { get; set; }
    }
}
