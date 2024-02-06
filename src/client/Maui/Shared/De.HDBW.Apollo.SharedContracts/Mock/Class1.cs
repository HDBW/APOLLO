// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Newtonsoft.Json.Linq;
using ProtoBuf;

namespace De.HDBW.Apollo.SharedContracts.Mock
{
    public class OccupationSuggestionRequest
    {
        public string Input { get; set; }

        public string CorrelationId { get; set; }
    }

    public class OccupationSuggestionResponse
    {
        public List<OccupationTerm?> OccupationSuggestions { get; set; } = new List<OccupationTerm?>();
    }

    public class OccupationTerm
    {
        public string? Id { get; set; }

        public string Title { get; set; }

        public string? OccupationId { get; set; }

        public string? CultureName { get; set; }
    }
}
