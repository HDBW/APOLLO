// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Tests.Extensions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Microsoft.Extensions.Logging;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Helper;
public class UseCaseBuilderTests : IDisposable
{
    private readonly ILogger<UseCaseBuilder> _logger;
    private readonly IAssessmentItemRepository _assessmentItemRepository;
    private readonly IQuestionItemRepository _questiontItemRepository;
    private readonly IAnswerItemRepository _answerItemRepository;
    private readonly IMetaDataMetaDataRelationRepository _metaDataMetaDataRelationRepository;
    private readonly IAnswerMetaDataRelationRepository _answerMetaDataRelationRepository;
    private readonly IQuestionMetaDataRelationRepository _questionMetaDataRelationRepository;
    private readonly IMetaDataRepository _metadataRepository;
    private readonly UserProfileItemRepository _userProfileItemRepository;
    private readonly CourseItemRepository _courseItemRepository;
    private readonly CourseContactRepository _courseContactRepository;
    private readonly CourseAppointmentRepository _courseAppointmentRepository;
    private readonly EduProviderItemRepository _eduProviderItemRepository;

    public UseCaseBuilderTests()
    {
        _logger = this.SetupLogger<UseCaseBuilder>();
        _assessmentItemRepository = new AssessmentItemRepository(this.SetupLogger<AssessmentItemRepository>());
        _questiontItemRepository = new QuestionItemRepository(this.SetupLogger<QuestionItemRepository>());
        _answerItemRepository = new AnswerItemRepository(this.SetupLogger<AnswerItemRepository>());
        _metaDataMetaDataRelationRepository = new MetaDataMetaDataRelationRepository(this.SetupLogger<MetaDataMetaDataRelationRepository>());
        _answerMetaDataRelationRepository = new AnswerMetaDataRelationRepository(this.SetupLogger<AnswerMetaDataRelationRepository>());
        _questionMetaDataRelationRepository = new QuestionMetaDataRelationRepository(this.SetupLogger<QuestionMetaDataRelationRepository>());
        _metadataRepository = new MetaDataRepository(this.SetupLogger<MetaDataRepository>());
        _userProfileItemRepository = new UserProfileItemRepository(this.SetupLogger<UserProfileItemRepository>());
        _courseItemRepository = new CourseItemRepository(this.SetupLogger<CourseItemRepository>());
        _courseContactRepository = new CourseContactRepository(this.SetupLogger<CourseContactRepository>());
        _courseAppointmentRepository = new CourseAppointmentRepository(this.SetupLogger<CourseAppointmentRepository>());
        _eduProviderItemRepository = new EduProviderItemRepository(this.SetupLogger<EduProviderItemRepository>());
    }

    public void Dispose()
    {
    }

    [Fact]
    public void TestCreation()
    {
        UseCaseBuilder? useCaseBuilder = null;
        var ctor = typeof(UseCaseBuilder).GetConstructors().FirstOrDefault();
        var parameters = ctor.GetParameters();
        var ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(null, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[0].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, null, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[1].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, null, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[2].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, null, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[3].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, null, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[4].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, null, null, null, null, null, null, null, null));
        Assert.Equal(parameters[5].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, null, null, null, null, null, null, null));
        Assert.Equal(parameters[6].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, null, null, null, null, null, null));
        Assert.Equal(parameters[7].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, null, null, null, null, null));
        Assert.Equal(parameters[8].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, null, null, null, null));
        Assert.Equal(parameters[9].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, null, null, null));
        Assert.Equal(parameters[10].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, null, null));
        Assert.Equal(parameters[11].Name, ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository,_courseAppointmentRepository, _userProfileItemRepository, null));
        Assert.Equal(parameters[12].Name, ex.ParamName);

        useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _userProfileItemRepository, _eduProviderItemRepository);
        Assert.NotNull(useCaseBuilder);
    }

    [Fact]
    public async Task TestBuildAsyncWithCanceledTokenAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _userProfileItemRepository, _eduProviderItemRepository);
        using (var cts = new CancellationTokenSource())
        {
            cts.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => { await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, cts.Token).ConfigureAwait(false); });
        }
    }

    [Fact]
    public async Task TestBuildAsyncDisposedTokenAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _userProfileItemRepository, _eduProviderItemRepository);
        using (var cts = new CancellationTokenSource())
        {
            cts.Dispose();
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => { await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, cts.Token).ConfigureAwait(false); });
        }
    }

    [Fact]
    public async Task TestBuildAsyncWithUnknownUseCaseAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _userProfileItemRepository, _eduProviderItemRepository);
        var token = default(CancellationToken);
        var result = await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.Unknown, token).ConfigureAwait(false);
        Assert.False(result);
    }

    [Fact]
    public async Task TestBuildAsyncWithUseCaseAAsync()
    {
        var useCaseBuilder = new UseCaseBuilder(_logger, _assessmentItemRepository, _questiontItemRepository, _answerItemRepository, _metaDataMetaDataRelationRepository, _answerMetaDataRelationRepository, _questionMetaDataRelationRepository, _metadataRepository, _courseItemRepository, _courseContactRepository, _courseAppointmentRepository, _userProfileItemRepository, _eduProviderItemRepository);
        var token = default(CancellationToken);
        var result = await useCaseBuilder.BuildAsync(SharedContracts.Enums.UseCase.A, token).ConfigureAwait(false);
        Assert.True(result);
        var answer = await _answerItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var questions = await _questiontItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var answerItems = await _answerItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var metaDataMetaDataRelations = await _metaDataMetaDataRelationRepository.GetItemsAsync(token);
        var answerMetaDatas = await _answerMetaDataRelationRepository.GetItemsAsync(token).ConfigureAwait(false);
        var questionMetas = await _questionMetaDataRelationRepository.GetItemsAsync(token).ConfigureAwait(false);
        var metadatas = await _metadataRepository.GetItemsAsync(token).ConfigureAwait(false);
        var courseItems = await _courseItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var courseContacts = await _courseContactRepository.GetItemsAsync(token).ConfigureAwait(false);
        var userProfileItems = await _userProfileItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        var eduProviderItems = await _eduProviderItemRepository.GetItemsAsync(token).ConfigureAwait(false);
        Assert.True(answer.Any(), "Answers are empty.");
        Assert.True(questions.Any(), "Questions are empty.");
        Assert.True(answerItems.Any(), "AnswerItems are empty.");
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
