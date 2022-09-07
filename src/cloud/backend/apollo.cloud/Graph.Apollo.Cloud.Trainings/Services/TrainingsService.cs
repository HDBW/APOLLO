using Graph.Apollo.Cloud.Common;
using Grpc.Core;

namespace Graph.Apollo.Cloud.Trainings.Services
{
    public class TrainingsService : Graph.Apollo.Cloud.Common.Trainings.TrainingsBase
    {
        private readonly ILogger<TrainingsService> _logger;
        private readonly ITrainings _trainings;

        public TrainingsService(ILogger<TrainingsService> logger, ITrainings trainings)
        {
            _logger = logger;
            _trainings = trainings;
        }

        public override Task<TrainingReply> GetTrainings(TrainingRequest request,
            ServerCallContext context)
        {
            var message = _trainings.GetTrainings();
            return Task.FromResult(new TrainingReply { Message = message });
        }
    }
}