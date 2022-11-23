// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Helper;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class StartViewInteractionEntry : InteractionEntry
    {
        private readonly Func<StartViewInteractionEntry, Task> _handleToggleIsFavorite;

        private readonly Func<StartViewInteractionEntry, bool> _canHandleToggleIsFavorite;

        [ObservableProperty]
        private Status _status;

        [ObservableProperty]
        private string? _subline;

        [ObservableProperty]
        private string? _info;

        [ObservableProperty]
        private string? _imagePath;

        [ObservableProperty]
        private string? _decoratorText;

        [ObservableProperty]
        private bool _isFiltered;

        private bool _isFavorite;

        private StartViewInteractionEntry(string? text, string? subline, string? decoratorText, string? info, string imagePath, Status status, Type entityType, object? data, Func<StartViewInteractionEntry, Task> handleToggleIsFavorite, Func<StartViewInteractionEntry, bool> canHandleToggleIsFavorite, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
            : base(text, data, navigateHandler, canNavigateHandle)
        {
            _handleToggleIsFavorite = handleToggleIsFavorite;
            _canHandleToggleIsFavorite = canHandleToggleIsFavorite;
            Subline = subline;
            Info = info;
            Status = status;
            EntityType = entityType;
            ImagePath = imagePath?.ToUniformedName();
            DecoratorText = decoratorText;
        }

        public Type EntityType { get; }

        public bool HasDecorator
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DecoratorText);
            }
        }

        public bool IsFavorite
        {
            get
            {
                return _isFavorite;
            }

            set
            {
                if (SetProperty(ref _isFavorite, value))
                {
                    ToggleIsFavoriteCommand?.NotifyCanExecuteChanged();
                }
            }
        }

        public static InteractionEntry Import<TU>(string text, string subline, string decoratorText, string info, string imagePath, Status status, object? data, Func<StartViewInteractionEntry, Task> handleToggleIsFavorite, Func<StartViewInteractionEntry, bool> canHandleToggleIsFavorite, Func<InteractionEntry, Task> handleInteract, Func<InteractionEntry, bool> canHandleInteract)
        {
            return new StartViewInteractionEntry(text, subline, decoratorText, info, imagePath, status, typeof(TU), data, handleToggleIsFavorite, canHandleToggleIsFavorite, handleInteract, canHandleInteract);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleIsFavorite))]
        private Task ToggleIsFavorite()
        {
            return _handleToggleIsFavorite?.Invoke(this) ?? Task.CompletedTask;
        }

        private bool CanToggleIsFavorite()
        {
            return _canHandleToggleIsFavorite?.Invoke(this) ?? false;
        }
    }
}
