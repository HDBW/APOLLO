// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Helper;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class InteractionEntry : ObservableObject
    {
        private readonly object? _data;

        [ObservableProperty]
        private string? _text;

        [ObservableProperty]
        private string? _imagePath;

        protected InteractionEntry(string? text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle, string? imagePath = null)
        {
            Text = text;
            ImagePath = imagePath?.ToUniformedName();
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

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        protected Func<InteractionEntry, bool> CanNavigateHandle { get; }

        protected Func<InteractionEntry, Task> NavigateHandler { get; }

        public static InteractionEntry Import(string text, object? data, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle, string? imagePath = null)
        {
            return new InteractionEntry(text, data, navigateHandler, canNavigateHandle, imagePath);
        }

        protected virtual bool CanNavigate()
        {
            return CanNavigateHandle?.Invoke(this) ?? false;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigate))]
        private Task Navigate()
        {
            return NavigateHandler?.Invoke(this) ?? Task.CompletedTask;
        }
    }
}
