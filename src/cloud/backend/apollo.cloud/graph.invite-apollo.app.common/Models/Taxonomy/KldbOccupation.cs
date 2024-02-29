// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [Obsolete("Remove")]
    public class KldbOccupation : Occupation
    {
        public KldbOccupation()
        {
            TaxonomyInfo = Taxonomy.KldB2010;
        }

        public string IdLevel { get; set; }
    }
}
