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

        protected InteractionEntry(string? text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            Text = text;
            _data = data;
            CanNavigateHandle = canNavigateHandle;
            NavigateHandler = navigateHandler;
        }

        public object? Data
        {
            get
            {
                return _data;
            }
        }

        protected Func<InteractionEntry, bool> CanNavigateHandle { get; }

        protected Func<InteractionEntry, Task> NavigateHandler { get; }

        public static InteractionEntry Import(string text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
        {
            return new InteractionEntry(text, data, navigateHandler, canNavigateHandle);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigate))]
        private Task Navigate()
        {
            return NavigateHandler?.Invoke(this) ?? Task.CompletedTask;
        }

        protected virtual bool CanNavigate()
        {
            return CanNavigateHandle?.Invoke(this) ?? false;
        }
    }
}
