namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class EscoSkill: BaseItem
    {
        public Guid EscoId { get; set; }

        public EscoStatus Status { get; set; }
    }
}
