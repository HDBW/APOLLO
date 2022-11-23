// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.Client.Converter
{
    public class SingleItemListConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == BindableProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            var result = new List<object>();

            if (value != null)
            {
                result.Add(value);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Binding two way is not supported in {GetType().Name}");
        }
    }
}
