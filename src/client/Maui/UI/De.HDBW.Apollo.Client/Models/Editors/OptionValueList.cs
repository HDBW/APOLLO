// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class OptionValueList : BaseValue<OptionValue>
    {
        public OptionValueList(
          long? id,
          string lable,
          OptionValue value,
          OptionValue defaultValue,
          IEnumerable<OptionValue> options)
          : base(id, lable, value, defaultValue)
        {
            Options = options;
        }

        public IEnumerable<OptionValue> Options { get; private set; }

        public override string GetValueAsString()
        {
            var value = Value?.GetValueAsString();
            return value ?? string.Empty;
        }
    }
}
