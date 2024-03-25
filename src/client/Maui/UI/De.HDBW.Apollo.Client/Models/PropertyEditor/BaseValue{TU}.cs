// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.PropertyEditor
{
    public abstract class BaseValue<TU> : BaseValue
    {
        public BaseValue(TU value)
          : base(value)
        {
        }

        public TU? Value
        {
            get
            {
                return (TU?)Data;
            }

            set
            {
                Data = value;
            }
        }
    }
}
