// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class SelectInteractionEntry : InteractionEntry
    {
        [ObservableProperty]
        private bool _isSelected;

        private SelectInteractionEntry(
            string? text,
            object? data,
            bool isSelected,
            Func<InteractionEntry, Task> navigateHandler,
            Func<InteractionEntry, bool> canNavigateHandle,
            Func<SelectInteractionEntry, Task> selectHandler,
            Func<SelectInteractionEntry, bool> canSelectHandler,
            Action<SelectInteractionEntry> selectedChangedHandler,
            string? imagePath = null)
            : base(text, data, navigateHandler, canNavigateHandle, imagePath)
        {
            SelectHandler = selectHandler;
            CanSelectHandler = canSelectHandler;
            SelectedChangedHandler = selectedChangedHandler;
            _isSelected = isSelected;
        }

        private Func<SelectInteractionEntry, Task> SelectHandler { get; }

        private Func<SelectInteractionEntry, bool> CanSelectHandler { get; }

        private Action<SelectInteractionEntry> SelectedChangedHandler { get; }

        public static SelectInteractionEntry Import(
            string? text,
            object? data,
            bool isSelected,
            Func<InteractionEntry, Task> navigateHandler,
            Func<InteractionEntry, bool> canNavigateHandle,
            Func<SelectInteractionEntry, Task> selectHandler,
            Func<SelectInteractionEntry, bool> canSelectHandler,
            Action<SelectInteractionEntry> selectedChangedHandler,
            string? imagePath = null)
        {
            return new SelectInteractionEntry(text, data, isSelected, navigateHandler, canNavigateHandle, selectHandler, canSelectHandler, selectedChangedHandler, imagePath);
        }

        partial void OnIsSelectedChanged(bool value)
        {
            SelectedChangedHandler?.Invoke(this);
        }

        private bool CanToggleSelectionState()
        {
            return CanSelectHandler?.Invoke(this) ?? false;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleSelectionState))]
        private Task ToggleSelectionState(CancellationToken token)
        {
            return SelectHandler?.Invoke(this) ?? Task.CompletedTask;
        }
    }
}
