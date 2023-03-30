// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class FilterInteractionEntry : InteractionEntry
    {
        [ObservableProperty]
        private bool _isSelected;

        private FilterInteractionEntry(string? text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
            : base(text, data, navigateHandler, canNavigateHandle)
        {
        }

        public static InteractionEntry Import<TU>(string text, TU? data, Func<InteractionEntry, Task> handleInteract, Func<InteractionEntry, bool> canHandleInteract)
        {
            return new FilterInteractionEntry(text, data, handleInteract, canHandleInteract);
        }
    }
}
