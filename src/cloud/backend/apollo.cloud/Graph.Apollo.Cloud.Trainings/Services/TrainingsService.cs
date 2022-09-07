using Google.Protobuf.WellKnownTypes;
using Graph.Apollo.Cloud.Common;
using Grpc.Core;

namespace Graph.Apollo.Cloud.Trainings.Services
{
    public class TrainingsService : Graph.Apollo.Cloud.Common.Trainings.TrainingsBase
    {
        private readonly ILogger<TrainingsService> _logger;
        public TrainingsService(ILogger<TrainingsService> logger)
        {
            _logger = logger;
        }

        public override async Task<GetTrainingsResponse> GetTrainings(Empty request, ServerCallContext context)
        {

        }
    }
}