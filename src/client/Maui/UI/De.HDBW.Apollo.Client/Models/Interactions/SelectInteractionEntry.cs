// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class SelectInteractionEntry : InteractionEntry
    {
        [ObservableProperty]
        private bool _isSelected;

        private Action _onIsSelectedChanged;

        private SelectInteractionEntry(string? text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle, Action onIsSelectedChanged, bool isSelected)
            : base(text, data, navigateHandler, canNavigateHandle)
        {
            _onIsSelectedChanged = onIsSelectedChanged;
            _isSelected = isSelected;
        }

        public static SelectInteractionEntry Import<TU>(string text, TU? data, Func<InteractionEntry, Task> handleInteract, Func<InteractionEntry, bool> canHandleInteract, Action onIsSelectedChanged, bool isSelected)
        {
            return new SelectInteractionEntry(text, data, handleInteract, canHandleInteract, onIsSelectedChanged, isSelected);
        }

        partial void OnIsSelectedChanged(bool value)
        {
            _onIsSelectedChanged.Invoke();
        }
    }
}
