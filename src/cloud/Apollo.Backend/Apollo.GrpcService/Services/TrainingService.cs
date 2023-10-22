// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.GrpcService;
using Apollo.Services.Grpc;
using Grpc.Core;
using static Apollo.Services.Grpc.TrainingService;

namespace Apollo.GrpcService.Services
{
    public class TrainingService : TrainingServiceBase
    {
        private readonly ILogger<TrainingService> _logger;
        public TrainingService(ILogger<TrainingService> logger)
        {
            _logger = logger;
        }

        public override Task<GetTrainingResponse> GetTraining(GetTrainingsRequest request, ServerCallContext context)
        {
            return Task.FromResult<GetTrainingResponse>(new GetTrainingResponse());
        }

        public override Task<QueryTrainingsResponse> QueryTrainings(QueryTrainingsRequest request, ServerCallContext context)
        {
            return Task.FromResult<QueryTrainingsResponse>(new QueryTrainingsResponse());
        }

        public override Task<CreateOrUpdateTrainingResponse> CreateOrUpdateTraining(CreateOrUpdateTrainingRequest request, ServerCallContext context)
        {
            return Task.FromResult<CreateOrUpdateTrainingResponse>(new CreateOrUpdateTrainingResponse { Id = "1"  });
        }

        public override Task<DeleteTrainingsResponse> DeleteTrainings(DeleteTrainingsRequest request, ServerCallContext context)
        {
            return Task.FromResult<DeleteTrainingsResponse>(new DeleteTrainingsResponse {  });

        }

    }
}
