// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SelectableEntry<TU> : ObservableObject, ISelectionInteractiveEntry
    {
        private bool _isSelected;

        public SelectableEntry(TU data, InteractionType interaction, Action<SelectableEntry<TU>> selectionChangedHandler)
        {
            Data = data;
            Interaction = interaction;
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

        public object Data { get; }

        public InteractionType Interaction { get; }

        private Action<SelectableEntry<TU>> SelectionChangedHandler { get; }

        public static SelectableEntry<TU> Import(TU data, InteractionType interaction, Action<SelectableEntry<TU>> selectionChangedHandler)
        {
            return new SelectableEntry<TU>(data, interaction, selectionChangedHandler);
        }

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
