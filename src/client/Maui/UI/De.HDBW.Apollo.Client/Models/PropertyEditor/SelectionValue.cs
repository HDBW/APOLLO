// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.PropertyEditor
{
    public partial class SelectionValue : BaseValue, ISelectableEntry
    {
        [ObservableProperty]
        private string _displayValue;

        private bool _isSelected;

        public SelectionValue(string displayValue, BaseValue data, Action<SelectionValue> selectionChangedHandler)
          : base(data)
        {
            DisplayValue = displayValue;
            SelectionChangedHandler = selectionChangedHandler;
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    SelectionChangedHandler?.Invoke(this);
                }
            }
        }

        private Action<SelectionValue> SelectionChangedHandler { get; }

        public void UpdateSelectedState(bool isSelected)
        {
            _isSelected = isSelected;
            OnPropertyChanged(nameof(IsSelected));
        }

        [RelayCommand]
        private void ToggleSelection()
        {
            IsSelected = !IsSelected;
        }
    }
}
