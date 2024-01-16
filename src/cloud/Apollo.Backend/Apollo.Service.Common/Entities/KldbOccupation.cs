// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
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
