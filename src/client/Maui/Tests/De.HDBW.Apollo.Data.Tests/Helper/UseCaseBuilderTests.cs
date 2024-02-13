// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Model;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Helper;
public class UseCaseBuilderTests
{
    [Fact]
    public void TestCreation()
    {
        var logger = this.SetupLogger<UseCaseBuilder>();
        using (var context = new DatabaseTestContext(Path.GetTempFileName(), logger))
        {
            var connectionProvider = this.SetupDataBaseConnectionProvider(context);
            var assessmentItemRepository = new AssessmentItemRepository(connectionProvider, this.SetupLogger<AssessmentItemRepository>());
            var assessmentCategoryRepository = new AssessmentCategoryRepository(connectionProvider, this.SetupLogger<AssessmentCategoryRepository>());
            var assessmentCategoryResultRepository = new AssessmentCategoryResultRepository(connectionProvider, this.SetupLogger<AssessmentCategoryResultRepository>());
            var assessmentScoreRepository = new AssessmentScoreRepository(connectionProvider, this.SetupLogger<AssessmentScoreRepository>());
            var questionItemRepository = new QuestionItemRepository(connectionProvider, this.SetupLogger<QuestionItemRepository>());
            var answerItemRepository = new AnswerItemRepository(connectionProvider, this.SetupLogger<AnswerItemRepository>());
            var answerItemResultRepository = new AnswerItemResultRepository(connectionProvider, this.SetupLogger<AnswerItemResultRepository>());
            var metaDataMetaDataRelationRepository = new MetaDataMetaDataRelationRepository(connectionProvider, this.SetupLogger<MetaDataMetaDataRelationRepository>());
            var answerMetaDataRelationRepository = new AnswerMetaDataRelationRepository(connectionProvider, this.SetupLogger<AnswerMetaDataRelationRepository>());
            var questionMetaDataRelationRepository = new QuestionMetaDataRelationRepository(connectionProvider, this.SetupLogger<QuestionMetaDataRelationRepository>());
            var metadataRepository = new MetaDataRepository(connectionProvider, this.SetupLogger<MetaDataRepository>());
            var courseItemRepository = new CourseItemRepository(connectionProvider, this.SetupLogger<CourseItemRepository>());
            var courseContactRepository = new CourseContactRepository(connectionProvider, this.SetupLogger<CourseContactRepository>());
            var courseAppointmentRepository = new CourseAppointmentRepository(connectionProvider, this.SetupLogger<CourseAppointmentRepository>());
            var courseContactRelationRepository = new CourseContactRelationRepository(connectionProvider, this.SetupLogger<CourseContactRelationRepository>());
            var eduProviderItemRepository = new EduProviderItemRepository(connectionProvider, this.SetupLogger<EduProviderItemRepository>());
            var categoryRecomendationItemRepository = new CategoryRecomendationItemRepository(connectionProvider, this.SetupLogger<CategoryRecomendationItemRepository>());

            UseCaseBuilder useCaseBuilder = null;
            var ctor = typeof(UseCaseBuilder).GetConstructors().FirstOrDefault();
            var parameters = ctor.GetParameters();
            var ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[0].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[1].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[2].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[3].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[4].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, null, null, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[5].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, null, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[6].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, null, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[7].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, null, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[8].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, null, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[9].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, null, null, null, null, null, null, null, null));
            Assert.Equal(parameters[10].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, null, null, null, null, null, null, null));
            Assert.Equal(parameters[11].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, null, null, null, null, null, null));
            Assert.Equal(parameters[12].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, null, null, null, null, null));
            Assert.Equal(parameters[13].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, null, null, null, null));
            Assert.Equal(parameters[14].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, null, null, null));
            Assert.Equal(parameters[15].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, null, null));
            Assert.Equal(parameters[16].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, null, null));
            Assert.Equal(parameters[17].Name, ex.ParamName);

            ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, eduProviderItemRepository, null));
            Assert.Equal(parameters[18].Name, ex.ParamName);

            useCaseBuilder = new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, eduProviderItemRepository, categoryRecomendationItemRepository);
            Assert.NotNull(useCaseBuilder);
        }
    }

    [Fact]
    public async Task TestBuildAsyncWithCanceledTokenAsync()
    {
        var logger = this.SetupLogger<UseCaseBuilder>();
        using (var context = new DatabaseTestContext(Path.GetTempFileName(), logger))
        {
            var connectionProvider = this.SetupDataBaseConnectionProvider(context);
            var assessmentItemRepository = new AssessmentItemRepository(connectionProvider, this.SetupLogger<AssessmentItemRepository>());
            var assessmentCategoryRepository = new AssessmentCategoryRepository(connectionProvider, this.SetupLogger<AssessmentCategoryRepository>());
            var assessmentCategoryResultRepository = new AssessmentCategoryResultRepository(connectionProvider, this.SetupLogger<AssessmentCategoryResultRepository>());
            var assessmentScoreRepository = new AssessmentScoreRepository(connectionProvider, this.SetupLogger<AssessmentScoreRepository>());
            var questionItemRepository = new QuestionItemRepository(connectionProvider, this.SetupLogger<QuestionItemRepository>());
            var answerItemRepository = new AnswerItemRepository(connectionProvider, this.SetupLogger<AnswerItemRepository>());
            var answerItemResultRepository = new AnswerItemResultRepository(connectionProvider, this.SetupLogger<AnswerItemResultRepository>());
            var metaDataMetaDataRelationRepository = new MetaDataMetaDataRelationRepository(connectionProvider, this.SetupLogger<MetaDataMetaDataRelationRepository>());
            var answerMetaDataRelationRepository = new AnswerMetaDataRelationRepository(connectionProvider, this.SetupLogger<AnswerMetaDataRelationRepository>());
            var questionMetaDataRelationRepository = new QuestionMetaDataRelationRepository(connectionProvider, this.SetupLogger<QuestionMetaDataRelationRepository>());
            var metadataRepository = new MetaDataRepository(connectionProvider, this.SetupLogger<MetaDataRepository>());
            var courseItemRepository = new CourseItemRepository(connectionProvider, this.SetupLogger<CourseItemRepository>());
            var courseContactRepository = new CourseContactRepository(connectionProvider, this.SetupLogger<CourseContactRepository>());
            var courseAppointmentRepository = new CourseAppointmentRepository(connectionProvider, this.SetupLogger<CourseAppointmentRepository>());
            var courseContactRelationRepository = new CourseContactRelationRepository(connectionProvider, this.SetupLogger<CourseContactRelationRepository>());
            var eduProviderItemRepository = new EduProviderItemRepository(connectionProvider, this.SetupLogger<EduProviderItemRepository>());
            var categoryRecomendationItemRepository = new CategoryRecomendationItemRepository(connectionProvider, this.SetupLogger<CategoryRecomendationItemRepository>());
            var useCaseBuilder = new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, eduProviderItemRepository, categoryRecomendationItemRepository);
            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();
                await Assert.ThrowsAsync<OperationCanceledException>(async () => { await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, cts.Token); });
            }
        }
    }

    [Fact]
    public async Task TestBuildAsyncDisposedTokenAsync()
    {
        var logger = this.SetupLogger<UseCaseBuilder>();
        using (var context = new DatabaseTestContext(Path.GetTempFileName(), logger))
        {
            var connectionProvider = this.SetupDataBaseConnectionProvider(context);
            var assessmentItemRepository = new AssessmentItemRepository(connectionProvider, this.SetupLogger<AssessmentItemRepository>());
            var assessmentCategoryRepository = new AssessmentCategoryRepository(connectionProvider, this.SetupLogger<AssessmentCategoryRepository>());
            var assessmentCategoryResultRepository = new AssessmentCategoryResultRepository(connectionProvider, this.SetupLogger<AssessmentCategoryResultRepository>());
            var assessmentScoreRepository = new AssessmentScoreRepository(connectionProvider, this.SetupLogger<AssessmentScoreRepository>());
            var questionItemRepository = new QuestionItemRepository(connectionProvider, this.SetupLogger<QuestionItemRepository>());
            var answerItemRepository = new AnswerItemRepository(connectionProvider, this.SetupLogger<AnswerItemRepository>());
            var answerItemResultRepository = new AnswerItemResultRepository(connectionProvider, this.SetupLogger<AnswerItemResultRepository>());
            var metaDataMetaDataRelationRepository = new MetaDataMetaDataRelationRepository(connectionProvider, this.SetupLogger<MetaDataMetaDataRelationRepository>());
            var answerMetaDataRelationRepository = new AnswerMetaDataRelationRepository(connectionProvider, this.SetupLogger<AnswerMetaDataRelationRepository>());
            var questionMetaDataRelationRepository = new QuestionMetaDataRelationRepository(connectionProvider, this.SetupLogger<QuestionMetaDataRelationRepository>());
            var metadataRepository = new MetaDataRepository(connectionProvider, this.SetupLogger<MetaDataRepository>());
            var courseItemRepository = new CourseItemRepository(connectionProvider, this.SetupLogger<CourseItemRepository>());
            var courseContactRepository = new CourseContactRepository(connectionProvider, this.SetupLogger<CourseContactRepository>());
            var courseAppointmentRepository = new CourseAppointmentRepository(connectionProvider, this.SetupLogger<CourseAppointmentRepository>());
            var courseContactRelationRepository = new CourseContactRelationRepository(connectionProvider, this.SetupLogger<CourseContactRelationRepository>());
            var eduProviderItemRepository = new EduProviderItemRepository(connectionProvider, this.SetupLogger<EduProviderItemRepository>());
            var categoryRecomendationItemRepository = new CategoryRecomendationItemRepository(connectionProvider, this.SetupLogger<CategoryRecomendationItemRepository>());
            var useCaseBuilder = new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, eduProviderItemRepository, categoryRecomendationItemRepository);
            using (var cts = new CancellationTokenSource())
            {
                cts.Dispose();
                await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, cts.Token); });
            }
        }
    }

    [Fact]
    public async Task TestBuildAsyncWithUnknownUseCaseAsync()
    {
        var logger = this.SetupLogger<UseCaseBuilder>();
        using (var context = new DatabaseTestContext(Path.GetTempFileName(), logger))
        {
            var connectionProvider = this.SetupDataBaseConnectionProvider(context);
            var assessmentItemRepository = new AssessmentItemRepository(connectionProvider, this.SetupLogger<AssessmentItemRepository>());
            var assessmentCategoryRepository = new AssessmentCategoryRepository(connectionProvider, this.SetupLogger<AssessmentCategoryRepository>());
            var assessmentCategoryResultRepository = new AssessmentCategoryResultRepository(connectionProvider, this.SetupLogger<AssessmentCategoryResultRepository>());
            var assessmentScoreRepository = new AssessmentScoreRepository(connectionProvider, this.SetupLogger<AssessmentScoreRepository>());
            var questionItemRepository = new QuestionItemRepository(connectionProvider, this.SetupLogger<QuestionItemRepository>());
            var answerItemRepository = new AnswerItemRepository(connectionProvider, this.SetupLogger<AnswerItemRepository>());
            var answerItemResultRepository = new AnswerItemResultRepository(connectionProvider, this.SetupLogger<AnswerItemResultRepository>());
            var metaDataMetaDataRelationRepository = new MetaDataMetaDataRelationRepository(connectionProvider, this.SetupLogger<MetaDataMetaDataRelationRepository>());
            var answerMetaDataRelationRepository = new AnswerMetaDataRelationRepository(connectionProvider, this.SetupLogger<AnswerMetaDataRelationRepository>());
            var questionMetaDataRelationRepository = new QuestionMetaDataRelationRepository(connectionProvider, this.SetupLogger<QuestionMetaDataRelationRepository>());
            var metadataRepository = new MetaDataRepository(connectionProvider, this.SetupLogger<MetaDataRepository>());
            var courseItemRepository = new CourseItemRepository(connectionProvider, this.SetupLogger<CourseItemRepository>());
            var courseContactRepository = new CourseContactRepository(connectionProvider, this.SetupLogger<CourseContactRepository>());
            var courseAppointmentRepository = new CourseAppointmentRepository(connectionProvider, this.SetupLogger<CourseAppointmentRepository>());
            var courseContactRelationRepository = new CourseContactRelationRepository(connectionProvider, this.SetupLogger<CourseContactRelationRepository>());
            var eduProviderItemRepository = new EduProviderItemRepository(connectionProvider, this.SetupLogger<EduProviderItemRepository>());
            var categoryRecomendationItemRepository = new CategoryRecomendationItemRepository(connectionProvider, this.SetupLogger<CategoryRecomendationItemRepository>());
            var useCaseBuilder = new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, eduProviderItemRepository, categoryRecomendationItemRepository);
            var token = default(CancellationToken);
            var result = await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, token);
            Assert.False(result);
        }
    }

    [Fact]
    public async Task TestBuildAsyncWithUseCaseAAsync()
    {
        var logger = this.SetupLogger<UseCaseBuilder>();
        using (var context = new DatabaseTestContext(Path.GetTempFileName(), logger))
        {
            var connectionProvider = this.SetupDataBaseConnectionProvider(context);
            var assessmentItemRepository = new AssessmentItemRepository(connectionProvider, this.SetupLogger<AssessmentItemRepository>());
            var assessmentCategoryRepository = new AssessmentCategoryRepository(connectionProvider, this.SetupLogger<AssessmentCategoryRepository>());
            var assessmentCategoryResultRepository = new AssessmentCategoryResultRepository(connectionProvider, this.SetupLogger<AssessmentCategoryResultRepository>());
            var assessmentScoreRepository = new AssessmentScoreRepository(connectionProvider, this.SetupLogger<AssessmentScoreRepository>());
            var questionItemRepository = new QuestionItemRepository(connectionProvider, this.SetupLogger<QuestionItemRepository>());
            var answerItemRepository = new AnswerItemRepository(connectionProvider, this.SetupLogger<AnswerItemRepository>());
            var answerItemResultRepository = new AnswerItemResultRepository(connectionProvider, this.SetupLogger<AnswerItemResultRepository>());
            var metaDataMetaDataRelationRepository = new MetaDataMetaDataRelationRepository(connectionProvider, this.SetupLogger<MetaDataMetaDataRelationRepository>());
            var answerMetaDataRelationRepository = new AnswerMetaDataRelationRepository(connectionProvider, this.SetupLogger<AnswerMetaDataRelationRepository>());
            var questionMetaDataRelationRepository = new QuestionMetaDataRelationRepository(connectionProvider, this.SetupLogger<QuestionMetaDataRelationRepository>());
            var metadataRepository = new MetaDataRepository(connectionProvider, this.SetupLogger<MetaDataRepository>());
            var courseItemRepository = new CourseItemRepository(connectionProvider, this.SetupLogger<CourseItemRepository>());
            var courseContactRepository = new CourseContactRepository(connectionProvider, this.SetupLogger<CourseContactRepository>());
            var courseAppointmentRepository = new CourseAppointmentRepository(connectionProvider, this.SetupLogger<CourseAppointmentRepository>());
            var courseContactRelationRepository = new CourseContactRelationRepository(connectionProvider, this.SetupLogger<CourseContactRelationRepository>());
            var eduProviderItemRepository = new EduProviderItemRepository(connectionProvider, this.SetupLogger<EduProviderItemRepository>());
            var categoryRecomendationItemRepository = new CategoryRecomendationItemRepository(connectionProvider, this.SetupLogger<CategoryRecomendationItemRepository>());
            var useCaseBuilder = new UseCaseBuilder(logger, assessmentItemRepository, assessmentCategoryRepository, assessmentCategoryResultRepository, assessmentScoreRepository, questionItemRepository, answerItemRepository, answerItemResultRepository, metaDataMetaDataRelationRepository, answerMetaDataRelationRepository, questionMetaDataRelationRepository, metadataRepository, courseItemRepository, courseContactRepository, courseAppointmentRepository, courseContactRelationRepository, eduProviderItemRepository, categoryRecomendationItemRepository);
            var token = default(CancellationToken);
            var result = await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.A, token);
            Assert.True(result);
            var assessments = await assessmentItemRepository.GetItemsAsync(token);
            var assessmentCategories = await assessmentCategoryRepository.GetItemsAsync(token);
            var questions = await questionItemRepository.GetItemsAsync(token);
            var answerItems = await answerItemRepository.GetItemsAsync(token);
            var answerItemsResults = await answerItemResultRepository.GetItemsAsync(token);
            var metaDataMetaDataRelations = await metaDataMetaDataRelationRepository.GetItemsAsync(token);
            var answerMetaDatas = await answerMetaDataRelationRepository.GetItemsAsync(token);
            var questionMetas = await questionMetaDataRelationRepository.GetItemsAsync(token);
            var metadatas = await metadataRepository.GetItemsAsync(token);
            var courseItems = await courseItemRepository.GetItemsAsync(token);
            var courseContacts = await courseContactRepository.GetItemsAsync(token);
            var eduProviderItems = await eduProviderItemRepository.GetItemsAsync(token);
            Assert.True(assessments.Any(), "Assessments are empty.");
            Assert.True(assessmentCategories.Any(), "AssessmentCategories are empty.");
            Assert.True(questions.Any(), "Questions are empty.");
            Assert.True(answerItems.Any(), "AnswerItems are empty.");
            Assert.True(answerItemsResults.Any(), "AnswerResults are empty.");
            Assert.False(metaDataMetaDataRelations.Any(), "MetaDataMetaDataRelations are empty.");
            Assert.True(answerMetaDatas.Any(), "AnswerMetaDatas are empty.");
            Assert.True(questionMetas.Any(), "QuestionMetas are empty.");
            Assert.True(metadatas.Any(), "Metadatas are empty.");
            Assert.True(courseItems.Any(), "CourseItems are empty.");
            Assert.True(courseContacts.Any(), "CourseContacts are empty.");
            Assert.True(eduProviderItems.Any(), "EduProviderItems are empty.");
        }
    }
}
