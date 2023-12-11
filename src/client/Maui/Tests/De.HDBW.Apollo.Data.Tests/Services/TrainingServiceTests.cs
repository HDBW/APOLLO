using De.HDBW.Apollo.Data.Services;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests.Services
{
    public class TrainingServiceTests : AbstractServiceTestSetup<TrainingService>
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
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(TokenSource!.Token))
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
            CourseItem? training = null;
            try
            {
                training = await Service.GetTrainingAsync(1, TokenSource!.Token).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                // Not existing ids return errorcode 101;
                Assert.Equal(500, ex.StatusCode);
                var error = JsonConvert.DeserializeObject<ServerError>(ex.Response);
                Assert.Equal(101, error.ErrorCode);
            }

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
            IEnumerable<CourseItem>? trainings = null;
            try
            {
                trainings = await Service.SearchTrainingsAsync(null, TokenSource!.Token).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                // Not existing ids return errorcode 101;
                Assert.Equal(500, ex.StatusCode);
                var error = JsonConvert.DeserializeObject<ServerError>(ex.Response);
                Assert.NotEqual(110, error.ErrorCode);
            }

            Assert.NotNull(trainings);
            Assert.Equal(2, trainings!.Count());
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
            IEnumerable<CourseItem>? trainings = null;
            try
            {
                trainings = await Service.SearchTrainingsAsync(filter, TokenSource!.Token);
            }
            catch (ApiException ex)
            {
                // Not existing ids return errorcode 101;
                Assert.Equal(500, ex.StatusCode);
                var error = JsonConvert.DeserializeObject<ServerError>(ex.Response);
                Assert.NotEqual(110, error.ErrorCode);
            }

            Assert.NotNull(trainings);
            Assert.Equal(2, trainings!.Count());
        }

        protected override TrainingService SetupService(string apiKey, string baseUri, ILogger<TrainingService> logger, HttpMessageHandler httpClientHandler)
        {
            return new TrainingService(logger, baseUri, apiKey, httpClientHandler);
        }

        protected override void CleanupAdditionalServices()
        {
        }

        protected override void SetupAdditionalServices(string apiKey, string baseUri, ILogger<TrainingService> logger, HttpMessageHandler httpClientHandler)
        {
        }
    }
}
