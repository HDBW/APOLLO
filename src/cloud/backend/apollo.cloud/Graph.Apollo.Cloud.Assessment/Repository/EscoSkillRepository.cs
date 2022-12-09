using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public class EscoSkillRepository : RepositoryBase<Models.EscoSkill>, IEscoSkillRepository
    {
        public EscoSkillRepository(AssessmentContext context) : base(context)
        {
        }
    }

    public interface IEscoSkillRepository : IRepository<Models.EscoSkill>
    {
    }
}
