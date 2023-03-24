// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.Data.Tests.Model;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Helper;
public class UseCaseBuilderTests : IDisposable
{
    private readonly ILogger<UseCaseBuilder> _logger;
    private readonly IAssessmentItemRepository _assessmentItemRepository;
    private readonly IAssessmentCategoryRepository _assessmentCategoryRepository;
    private readonly IAssessmentCategoryResultRepository _assessmentCategoryResultRepository;
    private readonly AssessmentScoreRepository _assessmentScoreRepository;
    private readonly IQuestionItemRepository _questionItemRepository;
    private readonly IAnswerItemRepository _answerItemRepository;
    private readonly AnswerItemResultRepository _answerItemResultRepository;
    private readonly IMetaDataMetaDataRelationRepository _metaDataMetaDataRelationRepository;
    private readonly IAnswerMetaDataRelationRepository _answerMetaDataRelationRepository;
    private readonly IQuestionMetaDataRelationRepository _questionMetaDataRelationRepository;
    private readonly IMetaDataRepository _metadataRepository;
    private readonly IUserProfileItemRepository _userProfileItemRepository;
    private readonly ICourseItemRepository _courseItemRepository;
    private readonly ICourseContactRepository _courseContactRepository;
    private readonly ICourseAppointmentRepository _courseAppointmentRepository;
    private readonly ICourseContactRelationRepository _courseContactRelationRepository;
    private readonly IEduProviderItemRepository _eduProviderItemRepository;
    private readonly CategoryRecomendationItemRepository _categoryRecomendationItemRepository;
    private DatabaseTestContext _context;

    public UseCaseBuilderTests()
    {
        _logger = this.SetupLogger<UseCaseBuilder>();
        _context = new DatabaseTestContext(Path.GetTempFileName());

        var connectionProvider = this.SetupDataBaseConnectionProvider(_context);
        _assessmentItemRepository = new AssessmentItemRepository(connectionProvider, this.SetupLogger<AssessmentItemRepository>());
        _assessmentCategoryRepository = new AssessmentCategoryRepository(connectionProvider, this.SetupLogger<AssessmentCategoryRepository>());
        _assessmentCategoryResultRepository = new AssessmentCategoryResultRepository(connectionProvider, this.SetupLogger<AssessmentCategoryResultRepository>());
        _assessmentScoreRepository = new AssessmentScoreRepository(connectionProvider, this.SetupLogger<AssessmentScoreRepository>());
        _questionItemRepository = new QuestionItemRepository(connectionProvider, this.SetupLogger<QuestionItemRepository>());
        _answerItemRepository = new AnswerItemRepository(connectionProvider, this.SetupLogger<AnswerItemRepository>());
        _answerItemResultRepository = new AnswerItemResultRepository(connectionProvider, this.SetupLogger<AnswerItemResultRepository>());
        _metaDataMetaDataRelationRepository = new MetaDataMetaDataRelationRepository(connectionProvider, this.SetupLogger<MetaDataMetaDataRelationRepository>());
        _answerMetaDataRelationRepository = new AnswerMetaDataRelationRepository(connectionProvider, this.SetupLogger<AnswerMetaDataRelationRepository>());
        _questionMetaDataRelationRepository = new QuestionMetaDataRelationRepository(connectionProvider, this.SetupLogger<QuestionMetaDataRelationRepository>());
        _metadataRepository = new MetaDataRepository(connectionProvider, this.SetupLogger<MetaDataRepository>());
        _userProfileItemRepository = new UserProfileItemRepository(connectionProvider, this.SetupLogger<UserProfileItemRepository>());
        _courseItemRepository = new CourseItemRepository(connectionProvider, this.SetupLogger<CourseItemRepository>());
        _courseContactRepository = new CourseContactRepository(connectionProvider, this.SetupLogger<CourseContactRepository>());
        _courseAppointmentRepository = new CourseAppointmentRepository(connectionProvider, this.SetupLogger<CourseAppointmentRepository>());
        _courseContactRelationRepository = new CourseContactRelationRepository(connectionProvider, this.SetupLogger<CourseContactRelationRepository>());
        _eduProviderItemRepository = new EduProviderItemRepository(connectionProvider, this.SetupLogger<EduProviderItemRepository>());
        _categoryRecomendationItemRepository = new CategoryRecomendationItemRepository(connectionProvider, this.SetupLogger<CategoryRecomendationItemRepository>());
    }

    public void Dispose()
    {
        _context.Dispose();
        _context = null;
    }

    [Fact]
    public void TestCreation()
    {
        UseCaseBuilder? useCaseBuilder = null;
        var ctor = typeof(UseCaseBuilder).GetConstructors().FirstOrDefault();
        var parameters = ctor.GetParameters();
        var ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[0].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[1].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[2].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[3].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[4].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, null, null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[5].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[6].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[7].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[8].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[9].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[10].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[11].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, null, null, null, null, null, null, null));
        Assert.Equal(parameters[12].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, null, null, null, null, null, null));
        Assert.Equal(parameters[13].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, null, null, null, null, null));
        Assert.Equal(parameters[14].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, null, null, null, null));
        Assert.Equal(parameters[15].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, null, null, null));
        Assert.Equal(parameters[16].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, _userProfileItemRepository, null, null));
        Assert.Equal(parameters[17].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, _userProfileItemRepository, _eduProviderItemRepository, null));
        Assert.Equal(parameters[18].Name, ex.ParamName);

        useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, _userProfileItemRepository, _eduProviderItemRepository, _categoryRecomendationItemRepository);
        Assert.NotNull(useCaseBuilder);
    }

    [Fact]
    public async Task TestBuildAsyncWithCanceledTokenAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, _userProfileItemRepository, _eduProviderItemRepository, _categoryRecomendationItemRepository);
        using (var cts = new CancellationTokenSource())
        {
            cts.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => { await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, cts.Token).ConfigureAwait(false); });
        }
    }

    [Fact]
    public async Task TestBuildAsyncDisposedTokenAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, _userProfileItemRepository, _eduProviderItemRepository, _categoryRecomendationItemRepository);
        using (var cts = new CancellationTokenSource())
        {
            cts.Dispose();
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, cts.Token).ConfigureAwait(false); });
        }
    }

    [Fact]
    public async Task TestBuildAsyncWithUnknownUseCaseAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, _userProfileItemRepository, _eduProviderItemRepository, _categoryRecomendationItemRepository);
        var token = default(CancellationToken);
        var result = await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, token).ConfigureAwait(false);
        Assert.False(result);
    }

    [Fact]
    public async Task TestBuildAsyncWithUseCaseAAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _assessmentCategoryRepository, _assessmentCategoryResultRepository, _assessmentScoreRepository, _questionItemRepository, _answerItemRepository, _answerItemResultRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _courseContactRelationRepository, _userProfileItemRepository, _eduProviderItemRepository, _categoryRecomendationItemRepository);
        var token = default(CancellationToken);
        var result = await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.A, token).ConfigureAwait(false);
        Assert.True(result);
        var assessments = await _assessmentItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var assessmentCategories = await _assessmentCategoryRepository.GetItemsAsync(token).ConfigureAwait(false);
        var questions = await _questionItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var answerItems = await _answerItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var answerItemsResults = await _answerItemResultRepository.GetItemsAsync(token).ConfigureAwait(false);
        var metaDataMetaDataRelations = await _metaDataMetaDataRelationRepository.GetItemsAsync(token);
        var answerMetaDatas = await _answerMetaDataRelationRepository.GetItemsAsync(token).ConfigureAwait(false);
        var questionMetas = await _questionMetaDataRelationRepository.GetItemsAsync(token).ConfigureAwait(false);
        var metadatas = await _metadataRepository.GetItemsAsync(token).ConfigureAwait(false);
        var courseItems = await _courseItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var courseContacts = await _courseContactRepository.GetItemsAsync(token).ConfigureAwait(false);
        var userProfileItems = await _userProfileItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var eduProviderItems = await _eduProviderItemRepository.GetItemsAsync(token).ConfigureAwait(false);
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
        Assert.True(userProfileItems.Any(), "UserProfiles are empty.");
        Assert.True(eduProviderItems.Any(), "EduProviderItems are empty.");
    }
}
