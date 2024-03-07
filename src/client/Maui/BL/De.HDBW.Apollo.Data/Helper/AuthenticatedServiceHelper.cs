// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Services;

namespace De.HDBW.Apollo.Data.Helper
{
    public static class AuthenticatedServiceHelper
    {
        private static List<Type> _authenticatedServices = new List<Type>();
        private static object _lockObject = new object();

        public static IEnumerable<Type> AuthenticatedServices
        {
            get { return _authenticatedServices; }
        }

        public static void UpdateAuthorizationHeader(IServiceProvider serviceProvider, string? authorizationHeader)
        {
            var serviceTypes = GetAuthenticatedServices();
            foreach (var serviceType in serviceTypes)
            {
                var service = serviceProvider.GetService(serviceType) as IAuthenticatedService;
                service?.UpdateAuthorizationHeader(authorizationHeader);
            }
        }

        private static List<Type> GetAuthenticatedServices()
        {
            lock (_lockObject)
            {
                if (_authenticatedServices.Any())
                {
                    return _authenticatedServices;
                }

                Type abstractAuthorizedSwaggerServiceBase = typeof(AbstractAuthorizedSwaggerServiceBase);

                List<(Type ClassType, Type InterfaceType)> typesDic = new List<(Type ClassType, Type InterfaceType)>();
                var serviceType = typeof(IAuthenticatedService);
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => p.IsPublic && !p.IsAbstract && serviceType.IsAssignableFrom(p)).ToList();
                var interfaces = types.Select(t => t.GetInterfaces().Where(x => x.Name.Contains(t.Name)).FirstOrDefault()).Where(x => x != null).Cast<Type>().ToList();
                _authenticatedServices = interfaces.ToList();
                return _authenticatedServices;
            }
        }
    }
}
