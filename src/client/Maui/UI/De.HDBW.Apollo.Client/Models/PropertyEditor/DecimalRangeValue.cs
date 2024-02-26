// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
namespace De.HDBW.Apollo.Client.Models.PropertyEditor
{
    public class DecimalRangeValue
        : BaseValue<(decimal Start, decimal End)>
    {
        public DecimalRangeValue((decimal Start, decimal End) data)
            : base(data)
        {
        }
    }
}
