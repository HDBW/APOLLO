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

        protected PickerPropertyEditor(string label, IEnumerable<PickerValue> values, PickerValue? currentValue, Action<BasePropertyEditor> clearValueAction)
           : base(label, currentValue, clearValueAction)
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

        public static IPropertyEditor Import(string label, IEnumerable<PickerValue> values, PickerValue? currentValue, Action<BasePropertyEditor> clearValueAction)
        {
            return new PickerPropertyEditor(label, values, currentValue, clearValueAction);
        }

        public override void Update(BaseValue? data, bool hasChanges)
        {
            Data = data;
            Value = Data as PickerValue;
            HasChanges = hasChanges;
        }

        public override void Save()
        {
            Data = Value;
        }

        protected virtual void OnValueChanged()
        {
        }
    }
}
