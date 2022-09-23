namespace De.HDBW.Apollo.Client.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class NavigationParameters : Dictionary<NavigationParameter, object>
    {
        public static NavigationParameters FromQueryDictionary(IDictionary<string, object> query)
        {
            var result = new NavigationParameters();
            foreach (var kv in query)
            {
                if (Enum.TryParse<NavigationParameter>(kv.Key, true, out NavigationParameter key))
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
            this.Remove(key);
            this.Add(key, value);
        }

        public TU GetValue<TU>(NavigationParameter key)
        {
            if (!this.ContainsKey(key))
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
