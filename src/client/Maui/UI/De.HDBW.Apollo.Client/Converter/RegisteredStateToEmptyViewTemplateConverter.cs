// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.Client.Converter
{
    public class RegisteredStateToEmptyViewTemplateConverter : IValueConverter
    {
        public DataTemplate? RegisteredUserTemplate { get; set; }

        public DataTemplate? UnregisteredUserTemplate { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == BindableProperty.UnsetValue || value == null || !(value is bool))
            {
                return Binding.DoNothing;
            }

            switch ((bool)value)
            {
                case true:
                    return RegisteredUserTemplate;
                case false:
                    return UnregisteredUserTemplate;
            }
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Binding two way is not supported in {GetType().Name}");
        }
    }
}
