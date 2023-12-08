using De.HDBW.Apollo.Data.Extensions;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public class TrainingServiceTests : AbstractServiceTestSetup<ITrainingService>
    {
        public TrainingServiceTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public async Task CancellationTokenTests()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(TokenSource.Token))
            {
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.SearchTrainingsAsync(null, cts.Token));
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.GetTrainingAsync(1, cts.Token));
            }
        }

        [Fact]
        public async Task GetTrainingAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var training = await Service.GetTrainingAsync(1, TokenSource!.Token);
            Assert.NotNull(training?.Id);

            Assert.Equal(1, training!.Id);

            // var courseItem = training.ToCourseItem();
            // Assert.NotNull(courseItem);

            // var courseAppointment = training.ToCourseAppointment();
            // Assert.NotNull(courseAppointment);

            // var eduProviderItem = training.ToEduProviderItems();
            // Assert.NotNull(eduProviderItem);

            // var courseContactRelation = training.ToCourseContactRelation();
            // Assert.NotNull(courseContactRelation);
        }

        [Fact]
        public async Task SearchTrainingsAsyncWithoutFilterTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var trainings = await Service.SearchTrainingsAsync(null, TokenSource!.Token);
            Assert.NotNull(trainings);
            Assert.Equal(2, trainings.Count());

            // var courseItems = trainings.Select(f => f.ToCourseItem());
            // var courseAppointments = trainings.Select(f => f.ToCourseAppointment());
            // var eduProviderItems = trainings.Select(f => f.ToEduProviderItems());
            // var courseContactRelations = trainings.Select(f => f.ToCourseContactRelation());
        }

        [Fact]
        public async Task SearchTrainingsAsyncWithFilterTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                    new FieldExpression()
                    {
                        FieldName = "trainingName",
                        Argument = new List<string>()
                        {
                            "TestName",
                            "TestName-2",
                        }.ToList<object>(),
                    },
                    new FieldExpression()
                    {
                        FieldName = "id",
                        Argument = new List<string>()
                        {
                            "5",
                            "2",
                        }.ToList<object>(),
                    },
            };

            var filter = new Filter()
            {
                Fields = fields,
            };

            var trainings = await Service.SearchTrainingsAsync(filter, TokenSource!.Token);
            Assert.NotNull(trainings);
            Assert.Equal(2, trainings.Count());

            foreach (var training in trainings)
            {
                var trainingDic = Helper.Utils.MapToDictionary(training);

                bool found = false;
                foreach (var item in fields)
                {
                    var dicKey = trainingDic.Keys.FirstOrDefault(x => x?.ToLower()?.Equals(item?.FieldName?.ToLower()) == true);
                    if (string.IsNullOrWhiteSpace(dicKey))
                    {
                        continue;
                    }

                    if (item?.Argument?.OfType<string>()?.Contains(trainingDic[dicKey] ?? string.Empty) == true)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.True(found, "The service did not return the right training.");
            }
        }

        protected override ITrainingService SetupService(string apiKey, string baseUri, ILoggerProvider provider, HttpMessageHandler httpClientHandler)
        {
            return new TrainingService(provider, baseUri, apiKey, httpClientHandler);
        }

        protected override void CleanupAdditionalServices()
        {
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, ILoggerProvider provider, HttpMessageHandler httpClientHandler)
        {
        }
    }
}
