// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models
{
    public partial class InteractionEntry : ObservableObject
    {
        [ObservableProperty]
        private string _text;

        private Func<InteractionEntry, bool> _canNavigateHandle;

        private Func<InteractionEntry, Task> _navigateHandler;

        private InteractionEntry(string text, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            _text = text;
            _canNavigateHandle = canNavigateHandle;
            _navigateHandler = navigateHandler;
        }

        public static InteractionEntry Import(string text, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            return new InteractionEntry(text, navigateHandler, canNavigateHandle);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigate))]
        private Task Navigate()
        {
            return _navigateHandler?.Invoke(this) ?? Task.CompletedTask;
        }

        private bool CanNavigate()
        {
            return _canNavigateHandle?.Invoke(this) ?? false;
        }
    }
}
