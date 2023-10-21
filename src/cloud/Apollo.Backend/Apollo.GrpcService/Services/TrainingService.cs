// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.GrpcService;
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

       
    }
}
