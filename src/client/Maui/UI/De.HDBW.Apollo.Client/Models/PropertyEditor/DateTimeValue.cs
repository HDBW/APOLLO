// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.PropertyEditor
{
    public class DateTimeValue : BaseValue<DateTime?>
    {
        public DateTimeValue(DateTime? value)
          : base(value)
        {
        }
    }
}
