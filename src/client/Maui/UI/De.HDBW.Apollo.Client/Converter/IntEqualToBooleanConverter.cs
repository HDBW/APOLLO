﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.Client.Converter
{
    public class IntEqualToBooleanConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == BindableProperty.UnsetValue)
            {
                return null;
            }

            if (!bool.TryParse(value.ToString(), out bool currentValue) ||
                !int.TryParse(parameter?.ToString(), out int index))
            {
                return false;
            }

            return currentValue ? index : null;
        }
    }
}