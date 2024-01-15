using System;
using System.Collections.Generic;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    public class KldbOccupation : Occupation
    {
        public KldbOccupation()
        {
            TaxonomyInfo = Taxonomy.KldB2010;
        }

        public string IdLevel { get; set; }
    }
}
