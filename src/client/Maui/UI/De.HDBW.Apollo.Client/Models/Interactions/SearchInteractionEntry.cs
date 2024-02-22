// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class SearchInteractionEntry : InteractionEntry, ICloneable, IProvideImageData
    {
        private readonly Func<SearchInteractionEntry, Task> _handleToggleIsFavorite;

        private readonly Func<SearchInteractionEntry, bool> _canHandleToggleIsFavorite;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasSubline))]
        private string? _subline;

        [ObservableProperty]
        private string? _info;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasDecoratorImage))]
        private string? _decoratorImagePath;

        [ObservableProperty]
        private string? _sublineImagePath;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasDecorator))]
        private string? _decoratorText;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasSublineImage))]
        private string? _imageData;

        [ObservableProperty]
        private bool _isFiltered;

        private bool _isFavorite;

        private SearchInteractionEntry(
            string? text,
            string? subline,
            string? sublineImagePath,
            string? decoratorText,
            string? decoratorImagePath,
            string? info,
            object? data,
            Func<SearchInteractionEntry, Task> handleToggleIsFavorite,
            Func<SearchInteractionEntry, bool> canHandleToggleIsFavorite,
            Func<InteractionEntry, Task> navigateHandler,
            Func<InteractionEntry, bool> canNavigateHandle)
            : base(text, data, navigateHandler, canNavigateHandle)
        {
            _handleToggleIsFavorite = handleToggleIsFavorite;
            _canHandleToggleIsFavorite = canHandleToggleIsFavorite;
            Subline = subline;
            SublineImagePath = sublineImagePath;
            DecoratorText = decoratorText;
            DecoratorImagePath = decoratorImagePath?.ToUniformedName();
            Info = info;
        }

        public bool HasDecorator
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DecoratorText);
            }
        }

        public bool HasDecoratorImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DecoratorImagePath);
            }
        }

        public bool HasSubline
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Subline);
            }
        }

        public bool HasSublineImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImageData);
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

        public static SearchInteractionEntry Import(string? text, string? subline, string? sublineImagePath, string? decoratorText, string? decoratorImagePath, string? info, object? data, Func<SearchInteractionEntry, Task> handleToggleIsFavorite, Func<SearchInteractionEntry, bool> canHandleToggleIsFavorite, Func<InteractionEntry, Task> handleInteract, Func<InteractionEntry, bool> canHandleInteract)
        {
            return new SearchInteractionEntry(text, subline, sublineImagePath, decoratorText, decoratorImagePath, info, data, handleToggleIsFavorite, canHandleToggleIsFavorite, handleInteract, canHandleInteract);
        }

        public object Clone()
        {
            return new SearchInteractionEntry(Text, Subline, SublineImagePath, DecoratorText, DecoratorImagePath, Info, Data, _handleToggleIsFavorite, _canHandleToggleIsFavorite, NavigateHandler, CanNavigateHandle)
            {
                _isFavorite = IsFavorite,
            };
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
