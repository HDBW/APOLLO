// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Reflection;
using Newtonsoft.Json;

namespace De.HDBW.Apollo.Data.Extensions
{
    internal static class JSONExtesnions
    {
        internal static Dictionary<string, string> GetJSONMapping(this Type modelType)
        {
            var mapping = new Dictionary<string, string>();
            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                mapping.Add(property.Name, attribute?.PropertyName ?? property.Name);
            }

            return mapping;
        }
    }
}
