using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Graph.Invite.Apollo.App.Course.Services
{
    public class GreeterService : Graph.Invite.Apollo.App.Course.Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
