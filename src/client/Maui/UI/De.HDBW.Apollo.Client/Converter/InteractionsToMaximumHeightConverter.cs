// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.Client.Models.Interactions;

namespace De.HDBW.Apollo.Client.Converter
{
    public class InteractionsToMaximumHeightConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == BindableProperty.UnsetValue)
            {
                return 0d;
            }

            if (!double.TryParse(parameter?.ToString(), out double rowHeight))
            {
                return 0d;
            }

            var items = value as IEnumerable<InteractionEntry> ?? new List<InteractionEntry>();
            var rows = (double)items.Count() / 2d;
            int rowCount = (int)Math.Ceiling(rows);
            return (double)(rowCount * rowHeight);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Twoway Binding not supported in {GetType().Name}.");
        }
    }
}
