using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public abstract class BaseValue<T> : BaseValue
    {
        public BaseValue(long? id, string label, T value, T defaultValue)
          : base(id, label)
        {
            Value = value;
            DefaultValue = defaultValue;
        }

        public T Value { get; set; }

        public T DefaultValue { get; set; }

        public override string GetValueAsString()
        {
            return Value == null ? string.Empty : Value.ToString();
        }
    }
}
