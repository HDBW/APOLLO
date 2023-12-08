using De.HDBW.Apollo.Data.Extensions;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    //[TestClass]
    //public class TrainingServiceTests : AbstractServiceTestSetup<ITrainingService>
    //{
    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        SetupDefaults();
    //    }

    //    [TestCleanup]
    //    public void CleanUp()
    //    {
    //        Cleanup();
    //    }

    //    protected override void CleanupAdditionalServices()
    //    {
    //    }

    //    protected override void SetupAdditionalServices(ILoggerProvider provider, HttpMessageHandler httpClientHandler)
    //    {
    //    }

    //    [Fact]
    //    public async Task CancellationTokenTests()
    //    {
    //        Assert.IsNotNull(TokenSource, "TokenSource is null.");
    //        Assert.IsNotNull(Service, "Service is null.");
    //        TokenSource.Cancel();
    //        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => Service.SearchTrainingsAsync(null, TokenSource.Token), "Service did not cancel while searching.");
    //        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => Service.GetTrainingAsync(1, TokenSource.Token), "Service did not cancel when getting training.");
    //    }

    //    [Fact]
    //    public async Task GetTrainingAsyncTest()
    //    {
    //        Assert.IsNotNull(TokenSource, "TokenSource is null.");
    //        Assert.IsNotNull(Service, "Service is null.");

    //        var training = await Service.GetTrainingAsync(1, TokenSource.Token);
    //        Assert.IsNotNull(training, "Service did not return any training.");
    //        Assert.IsNotNull(training.Id, "Service did not return id for training.");

    //        Assert.AreEqual(1, training.Id.TryToLong(), "Service did not return the right training.");

    //        var courseItem = training.ToCourseItem();
    //        Assert.IsNotNull(courseItem, "CourseItem is null");

    //        var courseAppointment = training.ToCourseAppointment();
    //        Assert.IsNotNull(courseAppointment, "CourseAppointment is null");

    //        var eduProviderItem = training.ToEduProviderItems();
    //        Assert.IsNotNull(eduProviderItem, "EduProviderItem is null");

    //        var courseContactRelation = training.ToCourseContactRelation();
    //        Assert.IsNotNull(courseContactRelation, "CourseContactRelation is null");
    //    }

    //    [Fact]
    //    public async Task SearchTrainingsAsyncWithoutFilterTest()
    //    {
    //        Assert.IsNotNull(TokenSource, "TokenSource is null.");
    //        Assert.IsNotNull(Service, "Service is null.");

    //        var trainings = await Service.SearchTrainingsAsync(null, TokenSource.Token);
    //        Assert.IsNotNull(trainings, "Service did not return any training.");
    //        Assert.AreEqual(2, trainings.Count(), "Service did not return one training.");

    //        var courseItems = trainings.Select(f => f.ToCourseItem());
    //        var courseAppointments = trainings.Select(f => f.ToCourseAppointment());
    //        var eduProviderItems = trainings.Select(f => f.ToEduProviderItems());
    //        var courseContactRelations = trainings.Select(f => f.ToCourseContactRelation());
    //    }

    //    [Fact]
    //    public async Task SearchTrainingsAsyncWithFilterTest()
    //    {
    //        Assert.IsNotNull(TokenSource, "TokenSource is null.");
    //        Assert.IsNotNull(Service, "Service is null.");

    //        var fields = new List<FieldExpression>() {
    //                new FieldExpression() {
    //                    FieldName = "trainingName",
    //                    Argument = new List<string>()
    //                    {
    //                        "TestName",
    //                        "TestName-2"
    //                    }.ToList<object>(),
    //                },
    //                new FieldExpression() {
    //                    FieldName = "id",
    //                    Argument = new List<string>()
    //                    {
    //                        "5",
    //                        "2"
    //                    }.ToList<object>(),
    //                }
    //        };

    //        var filter = new Filter()
    //        {
    //            Fields = fields
    //        };

    //        var trainings = await Service.SearchTrainingsAsync(filter, TokenSource.Token);
    //        Assert.IsNotNull(trainings, "Service did not return any training.");
    //        Assert.AreEqual(2, trainings.Count(), "Service did not return one training.");

    //        foreach (var training in trainings)
    //        {
    //            var trainingDic = Utils.MapToDictionary(training);

    //            bool found = false;
    //            foreach (var item in fields)
    //            {
    //                var dicKey = trainingDic.Keys.FirstOrDefault(x => x?.ToLower()?.Equals(item?.FieldName?.ToLower()) == true);
    //                if (string.IsNullOrWhiteSpace(dicKey))
    //                {
    //                    continue;
    //                }

    //                if(item?.Argument?.OfType<string>()?.Contains(trainingDic[dicKey] ?? string.Empty) == true)
    //                {
    //                    found = true;
    //                    break;
    //                }
    //            }

    //            Assert.IsTrue(found, "The service did not return the right training.");
    //        }
    //    }

    //    protected override ITrainingService SetupService(ILoggerProvider provider, HttpMessageHandler httpClientHandler)
    //    {
    //        return new TrainingService(provider, BaseUri, string.Empty, httpClientHandler);
    //    }
    //}
}
