// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.GrpcService;
using Grpc.Core;
using static Apollo.Services.Grpc.UserService;

namespace Apollo.GrpcService.Services
{
    public class UserService : UserServiceBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

       
    }
}
