// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Reflection;
using De.HDBW.Apollo.Data.Services;

namespace De.HDBW.Apollo.Data.Helper
{
    public static class AuthenticatedServiceHelper
    {
        private static List<(Type ClassType, Type InterfaceType)> authenticatedServices = new List<(Type ClassType, Type InterfaceType)>();

        public static List<(Type ClassType, Type InterfaceType)> GetAuthenticatedServices()
        {
            if (authenticatedServices.Any())
            {
                return authenticatedServices;
            }

            Type abstractAuthorizedSwaggerServiceBase = typeof(AbstractAuthorizedSwaggerServiceBase);

            List<(Type ClassType, Type InterfaceType)> typesDic = new List<(Type ClassType, Type InterfaceType)>();
            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var classtype in types)
            {
                if (classtype.IsSubclassOf(abstractAuthorizedSwaggerServiceBase))
                {
                    if (!classtype.IsInterface && !classtype.IsAbstract)
                    {
                        var interfacetype = classtype.GetInterface($"I{classtype.Name}");
                        if (interfacetype == null)
                        {
                            continue;
                        }

                        typesDic.Add((classtype, interfacetype));
                    }
                }
            }

            authenticatedServices = typesDic;
            return authenticatedServices;
        }

        public static void UpdateAuthorizationHeader(IServiceProvider serviceProvider, List<(Type ClassType, Type InterfaceType)> authenticatedServices, string? authorizationHeader)
        {
            foreach (var type in authenticatedServices)
            {
                var method = type.ClassType.GetMethod(nameof(AbstractAuthorizedSwaggerServiceBase.UpdateAuthorizationHeader));
                if (method == null || method.IsStatic)
                {
                    continue;
                }

                var instance = serviceProvider.GetService(type.InterfaceType);

                if (instance == null)
                {
                    continue;
                }

                method?.Invoke(instance, new object?[] { authorizationHeader });
            }
        }
    }
}
