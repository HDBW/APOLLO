// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{

    [Obsolete("Remove")]
    public class UnknownOccupation : Occupation
    {
        public UnknownOccupation()
        {
            TaxonomyInfo = Taxonomy.Unknown;
        }
    }
}
