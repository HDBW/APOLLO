// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public partial class ListPropertyEditor : BasePropertyEditor
    {
        private ObservableCollection<SelectionValue> _values = new ObservableCollection<SelectionValue>();

        [ObservableProperty]
        private SelectionValue? _value;

        [ObservableProperty]
        private bool _isSingleSelect;

        protected ListPropertyEditor(string label, IEnumerable<PickerValue> values, bool isSingleSelect, Action<BasePropertyEditor> clearValueAction)
           : base(label, clearValueAction)
        {
            _isSingleSelect = isSingleSelect;
            _values = new ObservableCollection<SelectionValue>(values.Select(x => new SelectionValue(x.DisplayValue, x, OnSelectionChanged)));
        }

        public ObservableCollection<SelectionValue> Values
        {
            get
            {
                return _values;
            }
        }

        public static IPropertyEditor Import(string label, IEnumerable<PickerValue> values, Action<BasePropertyEditor> clearValueAction, bool isSingleSelect = false)
        {
            return new ListPropertyEditor(label, values, isSingleSelect, clearValueAction);
        }

        public void Update(Dictionary<PickerValue, bool> values, bool hasChanges)
        {
            _values = new ObservableCollection<SelectionValue>(values.Select(x =>
            {
                var item = new SelectionValue(x.Key.DisplayValue, x.Key, OnSelectionChanged);
                item.UpdateSelectedState(x.Value);
                return item;
            }));
            OnPropertyChanged(nameof(Values));
            Update((BaseValue?)null, hasChanges);
        }

        public override void Update(BaseValue? data, bool hasChanges)
        {
            Data = data;
            HasChanges = hasChanges;
        }

        public override void Save()
        {
        }

        private void OnSelectionChanged(SelectionValue item)
        {
            HasChanges = true;
            if (IsSingleSelect)
            {
                foreach (var currentValue in Values)
                {
                    if (currentValue == item)
                    {
                        continue;
                    }

                    currentValue.UpdateSelectedState(false);
                }
            }
        }
    }
}
