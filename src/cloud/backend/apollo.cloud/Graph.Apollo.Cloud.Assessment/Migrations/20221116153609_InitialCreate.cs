using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invite.Apollo.App.Graph.Assessment.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultLimit = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kldb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscoOccupationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssessmentType = table.Column<int>(type: "int", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disclaimer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    assetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlobUris = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CdnUris = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EscoSkills",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EscoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssessmentId = table.Column<long>(type: "bigint", nullable: true),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscoSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscoSkills_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssessmentId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_AssessmentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AssessmentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetaDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetId = table.Column<long>(type: "bigint", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaDatas_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetaDataHasMetaData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceMetaDataId = table.Column<long>(type: "bigint", nullable: false),
                    TargetMetaDataId = table.Column<long>(type: "bigint", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaDataHasMetaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaDataHasMetaData_MetaDatas_SourceMetaDataId",
                        column: x => x.SourceMetaDataId,
                        principalTable: "MetaDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MetaDataHasMetaData_MetaDatas_TargetMetaDataId",
                        column: x => x.TargetMetaDataId,
                        principalTable: "MetaDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionHasMetaData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    MetaDataId = table.Column<long>(type: "bigint", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionHasMetaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionHasMetaData_MetaDatas_MetaDataId",
                        column: x => x.MetaDataId,
                        principalTable: "MetaDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionHasMetaData_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerHasMetaData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerId = table.Column<long>(type: "bigint", nullable: false),
                    MetaDataId = table.Column<long>(type: "bigint", nullable: false),
                    Ticks = table.Column<long>(type: "bigint", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerHasMetaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerHasMetaData_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerHasMetaData_MetaDatas_MetaDataId",
                        column: x => x.MetaDataId,
                        principalTable: "MetaDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerHasMetaData_AnswerId_MetaDataId",
                table: "AnswerHasMetaData",
                columns: new[] { "AnswerId", "MetaDataId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerHasMetaData_MetaDataId",
                table: "AnswerHasMetaData",
                column: "MetaDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Schema",
                table: "Answers",
                column: "Schema",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentCategories_Schema",
                table: "AssessmentCategories",
                column: "Schema",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ExternalId",
                table: "Assessments",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ExternalId",
                table: "Assets",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Schema",
                table: "Assets",
                column: "Schema",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EscoSkills_AssessmentId",
                table: "EscoSkills",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EscoSkills_EscoId",
                table: "EscoSkills",
                column: "EscoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EscoSkills_Schema",
                table: "EscoSkills",
                column: "Schema",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetaDataHasMetaData_SourceMetaDataId_TargetMetaDataId",
                table: "MetaDataHasMetaData",
                columns: new[] { "SourceMetaDataId", "TargetMetaDataId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetaDataHasMetaData_TargetMetaDataId",
                table: "MetaDataHasMetaData",
                column: "TargetMetaDataId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaDatas_AssetId",
                table: "MetaDatas",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaDatas_Schema",
                table: "MetaDatas",
                column: "Schema",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionHasMetaData_MetaDataId",
                table: "QuestionHasMetaData",
                column: "MetaDataId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionHasMetaData_QuestionId_MetaDataId",
                table: "QuestionHasMetaData",
                columns: new[] { "QuestionId", "MetaDataId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AssessmentId",
                table: "Questions",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CategoryId",
                table: "Questions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExternalId",
                table: "Questions",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Schema",
                table: "Questions",
                column: "Schema",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerHasMetaData");

            migrationBuilder.DropTable(
                name: "EscoSkills");

            migrationBuilder.DropTable(
                name: "MetaDataHasMetaData");

            migrationBuilder.DropTable(
                name: "QuestionHasMetaData");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "MetaDatas");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "AssessmentCategories");

            migrationBuilder.DropTable(
                name: "Assessments");
        }
    }
}
