// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class DateRangeValue : BaseValue<DateRange?>
    {
        public DateRangeValue(
          long? id,
          string lable,
          DateRange value,
          DateRange defaultValue)
          : base(id, lable, value, defaultValue)
        {
        }
    }
}
