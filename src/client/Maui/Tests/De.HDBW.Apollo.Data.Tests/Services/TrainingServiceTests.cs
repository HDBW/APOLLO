using Apollo.Api;
using Apollo.Common.Entities;
using De.HDBW.Apollo.Data.Services;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.Extensions.Logging;
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
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.SearchTrainingsAsync(null, null, cts.Token));
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Service.GetTrainingAsync(1, cts.Token));
            }
        }

        [Fact]
        public async Task GetTrainingAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            Training training = null;
            try
            {
                training = await Service.GetTrainingAsync(1, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode ErrorCodes.TrainingErrors.GetTrainingError;
                Assert.Equal(ErrorCodes.TrainingErrors.GetTrainingError, ex.ErrorCode);
            }

            Assert.NotNull(training?.Id);
            Assert.Equal("1", training!.Id);

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
            IEnumerable<Training> trainings = null;
            try
            {
                trainings = await Service.SearchTrainingsAsync(null, null, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Search with default filter returns ErrorCodes.TrainingErrors.QueryTrainingsError;
                Assert.NotEqual(ErrorCodes.TrainingErrors.QueryTrainingsError, ex.ErrorCode);
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
                    FieldName = nameof(Training.TrainingName),
                    Argument = new List<object>()
                    {
                        "t",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            var filter = new Filter()
            {
                Fields = fields,
            };

            var visibleFields = new List<string>()
            {
                nameof(Training.Id),
                nameof(Training.TrainingName),
                nameof(Training.ShortDescription),
                nameof(Training.Image)
            };

            IEnumerable<Training> trainings = null;
            try
            {
                trainings = await Service.SearchTrainingsAsync(filter, visibleFields, TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode 101;
                Assert.NotEqual(ErrorCodes.TrainingErrors.QueryTrainingsError, ex.ErrorCode);
            }

            Assert.NotNull(trainings);
            foreach (var training in trainings)
            {
                Assert.False(string.IsNullOrEmpty(training.TrainingName));
                Assert.False(string.IsNullOrEmpty(training.ShortDescription));
            }
        }

        [Fact]
        public async Task FilterTrainingsByNameWithContainsOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.TrainingName),
                    Argument = new List<object>()
                    {
                        "te",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            var trainings = await FilterTrainings(fields);
            Assert.NotEmpty(trainings);
        }

        [Fact]
        public async Task FilterTrainingsByNameWithEqualsOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            string trainingName = "Item 2";
            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.TrainingName),
                    Argument = new List<object>()
                    {
                        trainingName,
                    },

                    Operator = QueryOperator.Equals,
                },
            };

            var trainings = await FilterTrainings(fields);
            Assert.Empty(trainings?.Where(x => !x.TrainingName.Equals(trainingName)));
        }

        [Fact]
        public async Task FilterTrainingsByIdWithEqualsOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                        "SER01",
                    },

                    Operator = QueryOperator.Equals,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByMultipleFieldsAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.ProviderId),
                    Argument = new List<object>()
                    {
                        "h",
                    },

                    Operator = QueryOperator.Contains,
                },
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                        "SER01",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByMultipleFieldsWithOrOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                        "2",
                    },

                    Operator = QueryOperator.Contains,
                },
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                        "SER01",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields, true);
        }

        [Fact]
        public async Task FilterTrainingsByMultipleFieldsWithOrOperatorAndDifferentQueryOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                        "2",
                    },

                    Operator = QueryOperator.Contains,
                },
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                        "SER01",
                    },

                    Operator = QueryOperator.Equals,
                },
            };

            var trainings = await FilterTrainings(fields, true);
        }

        [Fact]
        public async Task FilterTrainingsByIdAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                        "2",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByTrainingTypeAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.TrainingType),
                    Argument = new List<object>()
                    {
                        TrainingType.Hybrid,
                        TrainingType.Online,
                    },

                    Operator = QueryOperator.In,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByProviderIdAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.ProviderId),
                    Argument = new List<object>()
                    {
                        "h",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByDescriptionAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Description),
                    Argument = new List<object>()
                    {
                        "a",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields);
        }


        [Fact]
        public async Task FilterTrainingsByShortDescriptionAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.ShortDescription),
                    Argument = new List<object>()
                    {
                        "o",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByImageAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Image),
                    Argument = new List<object>()
                    {
                        "image",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByTrainingProviderNameWithContainsOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = $"{nameof(Training.TrainingProvider)}.{nameof(Training.TrainingProvider.Name)}",
                    Argument = new List<object>()
                    {
                        "h",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields, true);
        }

        [Fact]
        public async Task FilterTrainingsByCourseProviderNameAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = $"{nameof(Training.CourseProvider)}.{nameof(Training.CourseProvider.Name)}",
                    Argument = new List<object>()
                    {
                        "n",
                    },

                    Operator = QueryOperator.Contains,
                },
            };

            await FilterTrainings(fields, true);
        }

        [Fact]
        public async Task FilterTrainingsByUnpublishingDateWithGreaterThanQueryOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.UnpublishingDate),
                    Argument = new List<object>()
                    {
                        DateTime.Now.AddMonths(-1),
                    },

                    Operator = QueryOperator.GreaterThan,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByUnpublishingDateWithGreaterThanEqualToQueryOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.UnpublishingDate),
                    Argument = new List<object>()
                    {
                        DateTime.Now.AddMonths(-1),
                    },

                    Operator = QueryOperator.GreaterThanEqualTo,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByUnpublishingDateWithBetweenQueryOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);
            var startDate = DateTime.Now.AddYears(-1);
            var endDate = DateTime.Now.AddYears(2);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.UnpublishingDate),
                    Argument = new List<object>()
                    {
                        startDate,
                    },

                    Operator = QueryOperator.GreaterThanEqualTo,
                },
                new FieldExpression()
                {
                    FieldName = nameof(Training.UnpublishingDate),
                    Argument = new List<object>()
                    {
                        endDate,
                    },

                    Operator = QueryOperator.LessThanEqualTo,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByPriceWithGreaterThanEqualToQueryOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Price),
                    Argument = new List<object>()
                    {
                        925,
                    },

                    Operator = QueryOperator.GreaterThanEqualTo,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByPriceAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Price),
                    Argument = new List<object>()
                    {
                        925,
                    },

                    Operator = QueryOperator.GreaterThan,
                },
            };

            await FilterTrainings(fields);
        }

        [Fact]
        public async Task FilterTrainingsByIdWithInQueryOperatorAsyncTest()
        {
            Assert.NotNull(TokenSource);
            Assert.NotNull(Service);

            var fields = new List<FieldExpression>()
            {
                new FieldExpression()
                {
                    FieldName = nameof(Training.Id),
                    Argument = new List<object>()
                    {
                       "Training-E5E2A9B2F805467C990842CE83C97657",
                       "Training-EB0316FB98B84496A9B14C2BB33355C9",
                    },

                    Operator = QueryOperator.In,
                },
            };

            await FilterTrainings(fields);
        }

        private async Task<IEnumerable<Training>> FilterTrainings(List<FieldExpression> fields, bool isOrOperator = false)
        {
            IEnumerable<Training> trainings = null;

            var filter = new Filter()
            {
                IsOrOperator = isOrOperator,
                Fields = fields,
            };

            try
            {
                trainings = await Service.SearchTrainingsAsync(filter, null,TokenSource!.Token);
            }
            catch (ApolloApiException ex)
            {
                // Not existing ids return errorcode 101;
                Assert.NotEqual(ErrorCodes.TrainingErrors.QueryTrainingsError, ex.ErrorCode);
            }

            Assert.NotNull(trainings);
            Assert.NotEmpty(trainings);

            return trainings;
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
