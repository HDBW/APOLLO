using Invite.Apollo.App.Graph.Assessment.Models;
using Microsoft.EntityFrameworkCore;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public class AssessmentContext : DbContext
    {
        public AssessmentContext(DbContextOptions<AssessmentContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.AssessmentQuestion>()
                .HasOne(q => q.Assessment)
                .WithMany(a => a.AssessmentQuestions)
                .HasForeignKey(q => q.AssessmentId);

            modelBuilder.Entity<Models.AssessmentAnswer>()
                .HasOne(a => a.AssessmentQuestion)
                .WithMany(q => q.AssessmentAnswers)
                .HasForeignKey(a => a.AssessmentQuestionId);

            
        }

        public DbSet<Models.Assessment> Assessments { get; set; }
        public DbSet<Models.AssessmentAnswer> Answers { get; set; }
        public DbSet<Models.AssessmentAsset> Assets { get; set; }
        public DbSet<Models.AssessmentMetaData> MetaDatas { get; set; }
        public DbSet<Models.AssessmentCategory> AssessmentCategories { get; set; }
        public DbSet<Models.AssessmentQuestion> Questions { get; set; }
        public DbSet<Models.AssessmentScores> Scores { get; set; }
    }
}
