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
