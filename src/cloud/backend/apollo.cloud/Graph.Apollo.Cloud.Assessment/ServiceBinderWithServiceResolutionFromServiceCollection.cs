using System.Reflection;
using ProtoBuf.Grpc.Configuration;

namespace Invite.Apollo.App.Graph.Assessment;

public class ServiceBinderWithServiceResolutionFromServiceCollection : ServiceBinder
{
    private readonly IServiceCollection _services;

    public ServiceBinderWithServiceResolutionFromServiceCollection(IServiceCollection services)
    {
        _services = services;
    }

    public override IList<object> GetMetadata(MethodInfo method, Type contractType, Type serviceType)
    {
        var resolvedServiceType = serviceType;
        if (serviceType.IsInterface)
            resolvedServiceType = _services.SingleOrDefault(x => x.ServiceType == serviceType)?.ImplementationType ?? serviceType;

        return base.GetMetadata(method, contractType, resolvedServiceType);
    }
}
