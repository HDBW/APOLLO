// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class Concept
    {
        public string Identifier { get; set; }
        public string UniqueIdentifier { get; set; }
        public string ConceptUri { get; set; }
        public string PreferedLabel { get; set; }
        public List<string> AlternativeLabels { get; set; } = new List<string>();
        public List<Concept> BroaderConcepts { get; set; } = new List<Concept>();
        public List<Concept> NarrowerConcepts { get; set; } = new List<Concept>();

        public string Description { get; set; }
        public string Definition { get; set; }
        public string Version { get; set; }

        public string ScopeNote { get; set; }

        public CultureInfo Language { get; set; }

        public Taxonomy TaxonomieInfo { get; set; }
    }
}
