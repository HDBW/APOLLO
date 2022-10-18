// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models
{
    public class NavigationParameters : Dictionary<NavigationParameter, object>
    {
        public static NavigationParameters FromQueryDictionary(IDictionary<string, object> query)
        {
            var result = new NavigationParameters();
            foreach (var kv in query)
            {
                if (Enum.TryParse(kv.Key, true, out NavigationParameter key))
                {
                    result.Add(key, kv.Value);
                }
            }

            return result;
        }

        public IDictionary<string, object> ToQueryDictionary()
        {
            return this.ToDictionary(k => k.Key.ToString(), v => v.Value);
        }

        public void AddValue<TU>(NavigationParameter key, TU value)
        {
            Remove(key);
            if (value == null)
            {
                return;
            }

            Add(key, value);
        }

        public TU? GetValue<TU>(NavigationParameter key)
        {
            if (!ContainsKey(key))
            {
                return default(TU);
            }

            var value = this[key];
            if (!(value is TU))
            {
                return default(TU);
            }

            return (TU)value;
        }
    }
}
