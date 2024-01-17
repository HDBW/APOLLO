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
