// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class InteractionEntry : ObservableObject
    {
        private readonly object? _data;

        [ObservableProperty]
        private string? _text;

        private Func<InteractionEntry, bool> _canNavigateHandle;

        private Func<InteractionEntry, Task> _navigateHandler;

        protected InteractionEntry(string? text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            Text = text;
            _data = data;
            _canNavigateHandle = canNavigateHandle;
            _navigateHandler = navigateHandler;
        }

        public object? Data
        {
            get
            {
                return _data;
            }
        }

        public static InteractionEntry Import(string text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            return new InteractionEntry(text, data, navigateHandler, canNavigateHandle);
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
