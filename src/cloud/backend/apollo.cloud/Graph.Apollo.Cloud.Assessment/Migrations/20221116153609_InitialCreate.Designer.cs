﻿// <auto-generated />
using System;
using Invite.Apollo.App.Graph.Assessment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Invite.Apollo.App.Graph.Assessment.Migrations
{
    [DbContext(typeof(AssessmentContext))]
    [Migration("20221116153609_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Answer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("AnswerType")
                        .HasColumnType("int");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("Schema")
                        .IsUnique();

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.AnswerHasMetaData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AnswerId")
                        .HasColumnType("bigint");

                    b.Property<long>("MetaDataId")
                        .HasColumnType("bigint");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MetaDataId");

                    b.HasIndex("AnswerId", "MetaDataId")
                        .IsUnique();

                    b.ToTable("AnswerHasMetaData");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Assessment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("AssessmentType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Disclaimer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("EscoOccupationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Kldb")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profession")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.ToTable("Assessments");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Asset", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("BlobUris")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CdnUris")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("FileUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.Property<string>("assetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.HasIndex("Schema")
                        .IsUnique();

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResultLimit")
                        .HasColumnType("int");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Schema")
                        .IsUnique();

                    b.ToTable("AssessmentCategories");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.EscoSkill", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("AssessmentId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("EscoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.HasIndex("EscoId")
                        .IsUnique();

                    b.HasIndex("Schema")
                        .IsUnique();

                    b.ToTable("EscoSkills");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.MetaData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AssetId")
                        .HasColumnType("bigint");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("Schema")
                        .IsUnique();

                    b.ToTable("MetaDatas");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.MetaDataHasMetaData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SourceMetaDataId")
                        .HasColumnType("bigint");

                    b.Property<long>("TargetMetaDataId")
                        .HasColumnType("bigint");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TargetMetaDataId");

                    b.HasIndex("SourceMetaDataId", "TargetMetaDataId")
                        .IsUnique();

                    b.ToTable("MetaDataHasMetaData");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AssessmentId")
                        .HasColumnType("bigint");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("QuestionType")
                        .HasColumnType("int");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.HasIndex("Schema")
                        .IsUnique();

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.QuestionHasMetaData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("MetaDataId")
                        .HasColumnType("bigint");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MetaDataId");

                    b.HasIndex("QuestionId", "MetaDataId")
                        .IsUnique();

                    b.ToTable("QuestionHasMetaData");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Answer", b =>
                {
                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.AnswerHasMetaData", b =>
                {
                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.Answer", "Answer")
                        .WithMany("AnswerHasMetaDatas")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.MetaData", "MetaData")
                        .WithMany("AnswerHasMetaDatas")
                        .HasForeignKey("MetaDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("MetaData");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.EscoSkill", b =>
                {
                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.Assessment", null)
                        .WithMany("EscoSkills")
                        .HasForeignKey("AssessmentId");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.MetaData", b =>
                {
                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.Asset", "Asset")
                        .WithMany("MetaDatas")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.MetaDataHasMetaData", b =>
                {
                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.MetaData", "SourceMetaData")
                        .WithMany("SourceQuestionHasMetaDatas")
                        .HasForeignKey("SourceMetaDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.MetaData", "TargetMetaData")
                        .WithMany("TargetMetaDataHasMetaDatas")
                        .HasForeignKey("TargetMetaDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SourceMetaData");

                    b.Navigation("TargetMetaData");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Question", b =>
                {
                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.Assessment", "Assessment")
                        .WithMany("Questions")
                        .HasForeignKey("AssessmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.Category", "Category")
                        .WithMany("Questions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assessment");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.QuestionHasMetaData", b =>
                {
                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.MetaData", "MetaData")
                        .WithMany("QuestionHasMetaDatas")
                        .HasForeignKey("MetaDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invite.Apollo.App.Graph.Assessment.Models.Question", "Question")
                        .WithMany("QuestionHasMetaDatas")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MetaData");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Answer", b =>
                {
                    b.Navigation("AnswerHasMetaDatas");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Assessment", b =>
                {
                    b.Navigation("EscoSkills");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Asset", b =>
                {
                    b.Navigation("MetaDatas");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Category", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.MetaData", b =>
                {
                    b.Navigation("AnswerHasMetaDatas");

                    b.Navigation("QuestionHasMetaDatas");

                    b.Navigation("SourceQuestionHasMetaDatas");

                    b.Navigation("TargetMetaDataHasMetaDatas");
                });

            modelBuilder.Entity("Invite.Apollo.App.Graph.Assessment.Models.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("QuestionHasMetaDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
