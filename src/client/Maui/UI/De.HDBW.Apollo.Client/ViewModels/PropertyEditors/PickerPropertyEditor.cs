// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public class PickerPropertyEditor : BasePropertyEditor, IPropertyEditor<PickerValue?>
    {
        private readonly ObservableCollection<PickerValue> _values = new ObservableCollection<PickerValue>();
        private PickerValue? _value;

        protected PickerPropertyEditor(string label, IEnumerable<PickerValue> values, PickerValue? currentValue)
           : base(label, currentValue)
        {
            _values = new ObservableCollection<PickerValue>(values);
            Value = currentValue;
        }

        public ObservableCollection<PickerValue> Values
        {
            get
            {
                return _values;
            }
        }

        public PickerValue? Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (SetProperty(ref _value, value))
                {
                    HasChanges = true;
                    Data = value;
                    OnValueChanged();
                }
            }
        }

        public static IPropertyEditor Import(string label, IEnumerable<PickerValue> values, PickerValue? currentValue)
        {
            return new PickerPropertyEditor(label, values, currentValue);
        }

        protected virtual void OnValueChanged()
        {
        }
    }
}
