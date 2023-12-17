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
        private readonly ObservableCollection<SelectionValue> _values = new ObservableCollection<SelectionValue>();

        [ObservableProperty]
        private SelectionValue? _value;

        [ObservableProperty]
        private bool _isSingleSelect;

        protected ListPropertyEditor(string label, IEnumerable<PickerValue> values, bool isSingleSelect)
           : base(label)
        {
            _values = new ObservableCollection<SelectionValue>(values.Select(x => new SelectionValue(x.DisplayValue, x, OnSelectionChanged)));
        }

        public ObservableCollection<SelectionValue> Values
        {
            get
            {
                return _values;
            }
        }

        public static IPropertyEditor Import(string label, IEnumerable<PickerValue> values, bool isSingleSelect = false)
        {
            return new ListPropertyEditor(label, values, isSingleSelect);
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
