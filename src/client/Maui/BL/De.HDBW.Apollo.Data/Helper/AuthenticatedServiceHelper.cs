// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Services;

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
    }
}
