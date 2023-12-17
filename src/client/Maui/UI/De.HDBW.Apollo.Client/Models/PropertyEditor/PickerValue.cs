// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.PropertyEditor
{
    public partial class PickerValue : BaseValue
    {
        [ObservableProperty]
        private string _displayValue;

        public PickerValue(string displayValue, object? data)
          : base(data)
        {
            DisplayValue = displayValue;
        }
    }
}
