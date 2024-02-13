﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Helper;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class BasicViewInteractionEntry : InteractionEntry, ICloneable
    {
        private readonly Func<BasicViewInteractionEntry, Task> _handleToggleIsFavorite;

        private readonly Func<BasicViewInteractionEntry, bool> _canHandleToggleIsFavorite;

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

        private Status _status;

        private BasicViewInteractionEntry(string? text, string? subline, string? decoratorText, string? info, string imagePath, Status status, string entityId, Type entityType, object? data, Func<BasicViewInteractionEntry, Task> handleToggleIsFavorite, Func<BasicViewInteractionEntry, bool> canHandleToggleIsFavorite, Func<InteractionEntry, Task> navigateHandler, Func<InteractionEntry, bool> canNavigateHandle)
            : base(text, data, navigateHandler, canNavigateHandle)
        {
            _handleToggleIsFavorite = handleToggleIsFavorite;
            _canHandleToggleIsFavorite = canHandleToggleIsFavorite;
            Subline = subline;
            Info = info;
            Status = Status.Processed;
            Status = status;
            EntityId = entityId;
            EntityType = entityType;
            ImagePath = imagePath?.ToUniformedName();
            DecoratorText = decoratorText;
        }

        public string EntityId { get; }

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

        public Status Status
        {
            get
            {
                return _status;
            }

            set
            {
                if (SetProperty(ref _status, value))
                {
                    OnPropertyChanged(nameof(IsProcessed));
                }
            }
        }

        public bool IsProcessed
        {
            get
            {
                return Status == Status.Processed;
            }
        }

        public static BasicViewInteractionEntry Import<TU>(string text, string subline, string decoratorText, string info, string imagePath, Status status, string entityId, object? data, Func<BasicViewInteractionEntry, Task> handleToggleIsFavorite, Func<BasicViewInteractionEntry, bool> canHandleToggleIsFavorite, Func<InteractionEntry, Task> handleInteract, Func<InteractionEntry, bool> canHandleInteract)
        {
            return new BasicViewInteractionEntry(text, subline, decoratorText, info, imagePath, status, entityId, typeof(TU), data, handleToggleIsFavorite, canHandleToggleIsFavorite, handleInteract, canHandleInteract);
        }

        public object Clone()
        {
            return new BasicViewInteractionEntry(Text, Subline, DecoratorText, Info, ImagePath!, Status, EntityId, EntityType, Data, _handleToggleIsFavorite, _canHandleToggleIsFavorite, NavigateHandler, CanNavigateHandle)
            {
                _isFavorite = IsFavorite,
            };
        }

        protected override bool CanNavigate()
        {
            if (Status == Status.Processed)
            {
                return false;
            }

            return base.CanNavigate();
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