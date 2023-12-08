// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections;

namespace De.HDBW.Apollo.Data.Tests.Helper
{
    public static class Utils
    {
        public static Dictionary<string, string?> MapToDictionary(object source)
        {
            var dictionary = new Dictionary<string, string?>();
            MapToDictionaryInternal(dictionary, source);
            return dictionary;
        }

        private static void MapToDictionaryInternal(
            Dictionary<string, string?> dictionary, object source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var p in properties)
            {
                var key = p.Name;
                object? value = p.GetValue(source, null);

                if (value == null)
                {
                    continue;
                }

                Type valueType = value.GetType();

                if (valueType.IsPrimitive || valueType == typeof(String))
                {
                    dictionary[key] = value.ToString();
                }
                else if (value is IEnumerable)
                {
                    var i = 0;
                    foreach (object o in (IEnumerable)value)
                    {
                        // MapToDictionaryInternal(dictionary, o, key + "[" + i + "]");
                        i++;
                    }
                }
                else
                {
                    // MapToDictionaryInternal(dictionary, value);
                }
            }
        }
    }
}
