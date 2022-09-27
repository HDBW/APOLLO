namespace De.HDBW.Apollo.Client.Converter
{
    using System.Globalization;
    using SkiaSharp.Extended.UI.Controls;
    using SkiaSharp.Extended.UI.Controls.Converters;

    public class StringToLottieImageSourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == BindableProperty.UnsetValue || (value == null))
            {
                return Binding.DoNothing;
            }

            var stringValue = value.ToString();

            SKLottieImageSource? result = null;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return result;
            }

            var converter = new SKLottieImageSourceConverter();
            var source = converter.ConvertFrom(stringValue) as SKLottieImageSource;
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("TwoWayBinding not supported");
        }
    }
}
