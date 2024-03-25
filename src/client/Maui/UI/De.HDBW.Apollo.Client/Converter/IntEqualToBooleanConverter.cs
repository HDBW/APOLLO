// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.Client.Converter
{
    public class IntEqualToBooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value == BindableProperty.UnsetValue)
            {
                return false;
            }

            if (!int.TryParse(value.ToString(), out int currentIndex) ||
                !int.TryParse(parameter?.ToString(), out int index))
            {
                return false;
            }

            return currentIndex == index;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value == BindableProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            if (!bool.TryParse(value.ToString(), out bool currentValue) ||
                !int.TryParse(parameter?.ToString(), out int index))
            {
                return false;
            }

            return currentValue ? index : Binding.DoNothing;
        }
    }
}
