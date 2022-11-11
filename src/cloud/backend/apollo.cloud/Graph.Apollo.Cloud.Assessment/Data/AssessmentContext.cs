using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using Invite.Apollo.App.Graph.Assessment.Models;
using Microsoft.EntityFrameworkCore;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public class AssessmentContext : DbContext
    {
        public AssessmentContext(DbContextOptions<AssessmentContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Assessment
            builder.Entity<Models.Assessment>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.Assessment>()
                .HasIndex(t => new { t.ExternalId}).IsUnique();

            //AssesmentQuestion
            builder.Entity<Models.AssessmentQuestion>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.AssessmentQuestion>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            builder.Entity<Models.AssessmentQuestion>()
                .HasIndex(t => new { t.ExternalId }).IsUnique();

            //AssessmentAnswer
            builder.Entity<Models.AssessmentAnswer>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.AssessmentAnswer>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            builder.Entity<Models.AssessmentQuestion>()
                .HasOne(q => q.Assessment)
                .WithMany(a => a.AssessmentQuestions)
                .HasForeignKey(q => q.AssessmentId);

            builder.Entity<Models.AssessmentAnswer>()
                .HasOne(a => a.AssessmentQuestion)
                .WithMany(q => q.AssessmentAnswers)
                .HasForeignKey(a => a.AssessmentQuestionId);

            //AssesmentCategory
            builder.Entity<Models.AssessmentCategory>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.AssessmentCategory>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            //AssesmentScores
            builder.Entity<Models.AssessmentScores>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.AssessmentScores>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            //AssessmentAsset
            builder.Entity<Models.AssessmentAsset>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.AssessmentAsset>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            builder.Entity<Models.AssessmentAsset>()
                .HasIndex(t => new { t.ExternalId }).IsUnique();
            
            //AssessmentCategory
            builder.Entity<Models.AssessmentCategory>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.AssessmentCategory>()
                .HasIndex(t => new { t.Schema }).IsUnique();


            //AssessmentMetaData
            builder.Entity<Models.AssessmentMetaData>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.AssessmentMetaData>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            builder.Entity<Models.AssessmentMetaData>()
                .HasOne(pt => pt.AssessmentAsset)
                .WithMany(p => p.AssessmentMetaDatas)
                .HasForeignKey(pt => pt.AssessmentAssetId);



            //AnswerHasMetaData
            builder.Entity<AssessmentAnswerHasMetaData>().HasKey(
                t => new { t.AnswerId, t.AssessmentMetaDataId });

            builder.Entity<AssessmentAnswerHasMetaData>()
                .HasOne(pt => pt.AssessmentAnswer)
                .WithMany(p => p.AssessmentAnswerHasMetaDatas)
                .HasForeignKey(pt => pt.AnswerId);

            builder.Entity<AssessmentAnswerHasMetaData>()
                .HasOne(pt => pt.AssessmentMetaData)
                .WithMany(p => p.AssessmentAnswerHasMetaDatas)
                .HasForeignKey(pt => pt.AssessmentMetaDataId);



            //builder.Entity<UserCompany>()
            //    .HasOne(pt => pt.Company)
            //    .WithMany(p => p.CompaniesUsers)
            //    .HasForeignKey(pt => pt.CompanyId);

            //builder.Entity<UserCompany>()
            //    .HasOne(pt => pt.User)
            //    .WithMany(p => p.CompaniesUsers)
            //    .HasForeignKey(pt => pt.UserId);

            ////Hate this section management of Subscriptions

            //builder.Entity<UserSubs>().HasKey(
            //    t => new { t.UserId, t.SubId });

            //builder.Entity<UserSubs>().HasOne(
            //    pt => pt.Subscription)
            //    .WithMany(p => p.SubscriptionUsers)
            //    .HasForeignKey(pt => pt.SubId);

            //builder.Entity<UserSubs>().HasOne(
            //    pt => pt.User)
            //    .WithMany(p => p.SubscriptionUsers)
            //    .HasForeignKey(pt => pt.UserId);

            ////This creates the association of productgroup has extras
            //builder.Entity<ProductGroupHasExtraGroup>().HasKey(
            //    g => new { g.ExtraGroupId, g.ProductGroupId });

            //builder.Entity<ProductGroupHasExtraGroup>().HasOne(
            //    pg => pg.ProductGroup)
            //    .WithMany(eg => eg.ProductGroupsHasExtraGroups)
            //    .HasForeignKey(pg => pg.ProductGroupId);

            //builder.Entity<ProductGroupHasExtraGroup>().HasOne(
            //    eg => eg.ExtraGroup)
            //    .WithMany(pg => pg.ProductGroupsHasExtraGroups)
            //    .HasForeignKey(eg => eg.ExtraGroupId);

            ////This creates the association of product has extras
            //builder.Entity<ProductHasExtraGroup>().HasKey(
            //    p => new { p.ProductId, p.ExtraGroupId });

            //builder.Entity<ProductHasExtraGroup>().HasOne(
            //    eg => eg.Product)
            //    .WithMany(p => p.ProductHasExtraGroups)
            //    .HasForeignKey(eg => eg.ProductId);

            //builder.Entity<ProductHasExtraGroup>().HasOne(
            //    p => p.ExtraGroup)
            //    .WithMany(eg => eg.ProductHasExtraGroups)
            //    .HasForeignKey(p => p.ExtraGroupId);

            base.OnModelCreating(builder);
            // Add your customizations after calling base.OnModelCreating(builder);
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
