// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public abstract class BaseValue
    {
        public BaseValue(long? id, string label)
        {
            Id = id;
            Label = label;
        }

        public long? Id { get; private set; }

        public string Label { get; private set; }

        public abstract string GetValueAsString();
    }
}
