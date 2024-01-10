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
        public List<string> AlternativeLabels { get; set; }
        public List<Concept> BroaderConcepts { get; set; }
        public List<Concept> NarrowerConcepts { get; set; }

        public string Description { get; set; }
        public string Definition { get; set; }
        public string Version { get; set; }

        public string ScopeNote { get; set; }

        public CultureInfo Language { get; set; }

        public TaxonomyType TaxonomieInfo { get; set; }
    }
}
