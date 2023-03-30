// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Globalization;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.Converter
{
    public class QuestionDetailsToTitleConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == BindableProperty.UnsetValue || value == null || parameter == null)
            {
                return Binding.DoNothing;
            }

            if (!int.TryParse(parameter.ToString(), out int index))
            {
                return string.Empty;
            }

            var details = value as IList<IInteractiveEntry> ?? new List<IInteractiveEntry>();
            var title = string.Empty;
            if (details.Count > index)
            {
                var item = details[index].Data as QuestionDetailEntry;
                title = item?.Text ?? string.Empty;
            }

            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"Binding two way is not supported in {GetType().Name}");
        }
    }
}
