// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
namespace De.HDBW.Apollo.Client.Models.PropertyEditor
{
    public class DateRangeValue
        : BaseValue<(DateTime? Start, DateTime? End)>
    {
        public DateRangeValue((DateTime? Start, DateTime? End) data)
            : base(data)
        {
        }
    }
}
