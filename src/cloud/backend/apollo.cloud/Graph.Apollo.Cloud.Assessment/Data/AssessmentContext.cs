using Microsoft.EntityFrameworkCore;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public class AssessmentContext : DbContext
    {
        public AssessmentContext(DbContextOptions<AssessmentContext> options) : base(options)
        {
            
        }

        public DbSet<Models.Assessment> Assessments { get; set; }
        public DbSet<Models.Answer> Answers { get; set; }
        public DbSet<Models.Asset> Assets { get; set; }
        public DbSet<Models.MetaData> MetaDatas { get; set; }
        public DbSet<Models.AssessmentCategory> AssessmentCategories { get; set; }
        public DbSet<Models.Question> Questions { get; set; }
        public DbSet<Models.Scores> Scores { get; set; }

    }
}
