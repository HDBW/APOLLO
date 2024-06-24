// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.Client.Converter
{
    public class UnicodeToImageFontsourceConvert : IValueConverter
    {
        public string? FontFamily { get; set; }

        public Color? Color { get; set; }

        public double? Size { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value == BindableProperty.UnsetValue || !(value is string))
            {
                return Binding.DoNothing;
            }

            var unicode = value as string;
            var source = new FontImageSource() { Glyph = unicode };
            if (!string.IsNullOrWhiteSpace(FontFamily))
            {
                source.FontFamily = FontFamily;
            }

            if (Color != null)
            {
                source.Color = Color;
            }

            if (Size.HasValue)
            {
                source.Size = Size.Value;
            }

            return source;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var source = value as FontImageSource;
            if (source == null || value == BindableProperty.UnsetValue)
            {
                return Binding.DoNothing;
            }

            return source.Glyph;
        }
    }
}