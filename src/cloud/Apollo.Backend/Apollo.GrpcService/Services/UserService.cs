// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Apollo.GrpcService;
using Apollo.Services.Grpc;
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

        public override Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            return Task.FromResult<GetUserResponse>(new GetUserResponse());
        }
        public override Task<QueryUsersResponse> QueryUsers(QueryUsersRequest request, ServerCallContext context)
        {
            return Task.FromResult<QueryUsersResponse>(new QueryUsersResponse());
        }

        public override Task<CreateOrUpdateUserResponse> CreateOrUpdateUser(CreateOrUpdateUserRequest request, ServerCallContext context)
        {
            return Task.FromResult<CreateOrUpdateUserResponse>(new CreateOrUpdateUserResponse());
        }

        public override Task<DeleteUsersResponse> DeleteUsers(DeleteUsersRequest request, ServerCallContext context)
        {
            return Task.FromResult<DeleteUsersResponse>(new DeleteUsersResponse());
        }
    } 
}
