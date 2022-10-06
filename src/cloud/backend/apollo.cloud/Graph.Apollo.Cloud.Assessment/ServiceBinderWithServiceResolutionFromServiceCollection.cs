// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Meta;

namespace Graph.Apollo.Cloud.Assessment;

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
