// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public partial class ComboboxPropertyEditor : PickerPropertyEditor
    {
        [ObservableProperty]
        private bool _isOpen;

        private ComboboxPropertyEditor(string label, IEnumerable<PickerValue> values, PickerValue? currentValue)
            : base(label, values, currentValue)
        {
        }

        public static new IPropertyEditor Import(string label, IEnumerable<PickerValue> values, PickerValue? currentValue)
        {
            return new ComboboxPropertyEditor(label, values, currentValue);
        }

        protected override void OnValueChanged()
        {
            OnPropertyChanged();
            IsOpen = false;
        }
    }
}
