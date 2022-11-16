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
            builder.Entity<Models.Question>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.Question>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            builder.Entity<Models.Question>()
                .HasIndex(t => new { t.ExternalId }).IsUnique();

            //AssessmentAnswer
            builder.Entity<Models.Answer>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.Answer>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            builder.Entity<Models.Question>()
                .HasOne(q => q.Assessment)
                .WithMany(a => a.Questions) //used to be AssessmentQuestions
                .HasForeignKey(q => q.AssessmentId);

            builder.Entity<Models.Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            //AssesmentCategory
            builder.Entity<Models.Category>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.Category>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            //AssesmentScores
            builder.Entity<Models.Scores>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.Scores>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            //AssessmentAsset
            builder.Entity<Models.Asset>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.Asset>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            builder.Entity<Models.Asset>()
                .HasIndex(t => new { t.ExternalId }).IsUnique();
            
            //AssessmentCategory
            builder.Entity<Models.Category>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.Category>()
                .HasIndex(t => new { t.Schema }).IsUnique();


            //AssessmentMetaData
            builder.Entity<Models.MetaData>()
                .HasKey(t => new { t.BackendId });
            builder.Entity<Models.MetaData>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            builder.Entity<Models.MetaData>()
                .HasOne(pt => pt.Asset)
                .WithMany(p => p.MetaDatas)
                .HasForeignKey(pt => pt.AssetId);



            //AnswerHasMetaData
            builder.Entity<AnswerHasMetaData>().HasKey(
                t => new { t.AnswerId, t.MetaDataId });

            builder.Entity<AnswerHasMetaData>()
                .HasOne(pt => pt.Answer)
                .WithMany(p => p.AnswerHasMetaDatas)
                .HasForeignKey(pt => pt.AnswerId);

            builder.Entity<AnswerHasMetaData>()
                .HasOne(pt => pt.MetaData)
                .WithMany(p => p.AnswerHasMetaDatas)
                .HasForeignKey(pt => pt.MetaDataId);



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
        public DbSet<Models.Answer> Answers { get; set; }
        public DbSet<Models.Asset> Assets { get; set; }
        public DbSet<Models.MetaData> MetaDatas { get; set; }
        public DbSet<Models.Category> AssessmentCategories { get; set; }
        public DbSet<Models.Question> Questions { get; set; }
        public DbSet<Models.Scores> Scores { get; set; }

    }
}
