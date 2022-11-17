using Invite.Apollo.App.Graph.Assessment.Models;
using Microsoft.EntityFrameworkCore;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public class AssessmentContext : DbContext
    {
        #region DBSets
        public DbSet<Models.Assessment> Assessments { get; set; }
        public DbSet<Models.Answer> Answers { get; set; }
        public DbSet<Models.Asset> Assets { get; set; }
        public DbSet<Models.MetaData> MetaDatas { get; set; }
        public DbSet<Models.Category> AssessmentCategories { get; set; }
        public DbSet<Models.Question> Questions { get; set; }
        public DbSet<Models.EscoSkill> EscoSkills { get; set; }
        #endregion

        private readonly ILoggerFactory _loggerFactory;

        public AssessmentContext(DbContextOptions<AssessmentContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Assessment
            builder.Entity<Models.Assessment>()
                .HasKey(t => new { t.Id });
            builder.Entity<Models.Assessment>()
                .HasIndex(t => new { t.ExternalId}).IsUnique();

            //Question
            builder.Entity<Models.Question>()
                .HasKey(t => new { t.Id });
            builder.Entity<Models.Question>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            builder.Entity<Models.Question>()
                .HasIndex(t => new { t.ExternalId }).IsUnique();
            builder.Entity<Models.Question>()
                .HasOne(q => q.Assessment)
                .WithMany(a => a.Questions) //used to be AssessmentQuestions
                .HasForeignKey(q => q.AssessmentId);
            builder.Entity<Models.Question>()
                .HasOne(q => q.Category)
                .WithMany(a => a.Questions) //used to be AssessmentQuestions
                .HasForeignKey(q => q.CategoryId);



            //Answer
            builder.Entity<Models.Answer>()
                .HasKey(t => new { t.Id });
            builder.Entity<Models.Answer>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            
            builder.Entity<Models.Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            //Category
            builder.Entity<Models.Category>()
                .HasKey(t => new { t.Id });
            builder.Entity<Models.Category>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            //Asset
            builder.Entity<Models.Asset>()
                .HasKey(t => new { t.Id });
            builder.Entity<Models.Asset>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            builder.Entity<Models.Asset>()
                .HasIndex(t => new { t.ExternalId }).IsUnique();

            //MetaData
            builder.Entity<Models.MetaData>()
                .HasKey(t => new { t.Id });
            builder.Entity<Models.MetaData>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            builder.Entity<Models.MetaData>()
                .HasOne(pt => pt.Asset)
                .WithMany(p => p.MetaDatas)
                .HasForeignKey(pt => pt.AssetId);



            //AnswerHasMetaData
            builder.Entity<AnswerHasMetaData>().HasKey(
                t => new { t.Id });

            builder.Entity<AnswerHasMetaData>().HasIndex(
                t => new { t.AnswerId, t.MetaDataId }).IsUnique();
            //IsClustered() ?

            builder.Entity<AnswerHasMetaData>()
                .HasOne(pt => pt.Answer)
                .WithMany(p => p.AnswerHasMetaDatas)
                .HasForeignKey(pt => pt.AnswerId);

            builder.Entity<AnswerHasMetaData>()
                .HasOne(pt => pt.MetaData)
                .WithMany(p => p.AnswerHasMetaDatas)
                .HasForeignKey(pt => pt.MetaDataId);




            //QuestionHasMetaData
            builder.Entity<QuestionHasMetaData>().HasKey(
                t => new { t.Id });

            builder.Entity<QuestionHasMetaData>().HasIndex(
                t => new { t.QuestionId, t.MetaDataId }).IsUnique();
            //IsClustered() ?

            builder.Entity<QuestionHasMetaData>()
                .HasOne(pt => pt.Question)
                .WithMany(p => p.QuestionHasMetaDatas)
                .HasForeignKey(pt => pt.QuestionId);

            builder.Entity<QuestionHasMetaData>()
                .HasOne(pt => pt.MetaData)
                .WithMany(p => p.QuestionHasMetaDatas)
                .HasForeignKey(pt => pt.MetaDataId);

            //MetaDataHasMetaData
            builder.Entity<MetaDataHasMetaData>().HasKey(
                t => new { t.Id });

            builder.Entity<MetaDataHasMetaData>().HasIndex(
                t => new { t.SourceMetaDataId, t.TargetMetaDataId}).IsUnique();
            //IsClustered() ?

            builder.Entity<MetaDataHasMetaData>()
                .HasOne(pt => pt.SourceMetaData)
                .WithMany(p => p.SourceQuestionHasMetaDatas)
                .HasForeignKey(pt => pt.SourceMetaDataId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MetaDataHasMetaData>()
                .HasOne(pt => pt.TargetMetaData)
                .WithMany(p => p.TargetMetaDataHasMetaDatas)
                .HasForeignKey(pt => pt.TargetMetaDataId)
                .OnDelete(DeleteBehavior.NoAction);


            //Esco
            builder.Entity<EscoSkill>().HasKey(
                t => new { t.Id });
            builder.Entity<Models.EscoSkill>()
                .HasIndex(t => new { t.EscoId }).IsUnique();
            builder.Entity<Models.EscoSkill>()
                .HasIndex(t => new { t.Schema }).IsUnique();


            base.OnModelCreating(builder);
            // Add your customizations after calling base.OnModelCreating(builder);
        }

    }
}
