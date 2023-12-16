// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class StringValue : BaseValue<string>
    {
        public StringValue(
          long? id,
          string lable,
          string value,
          string defaultValue)
          : base(id, lable, value, defaultValue)
        {
        }
    }
}
