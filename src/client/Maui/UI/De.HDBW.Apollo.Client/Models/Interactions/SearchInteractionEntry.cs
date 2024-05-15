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

        [ObservableProperty]
        private bool _isFavorite;

        [ObservableProperty]
        private bool _canBeMadeFavorite;

        private SearchInteractionEntry(
            string? text,
            string? subline,
            string? sublineImagePath,
            string? decoratorText,
            string? decoratorImagePath,
            string? info,
            object? data,
            bool canBeMadeFavorite,
            bool isFavorite,
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
            DecoratorImagePath = decoratorImagePath;
            Info = info;
            CanBeMadeFavorite = canBeMadeFavorite;
            IsFavorite = isFavorite;
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
                return !string.IsNullOrWhiteSpace(SublineImagePath);
            }
        }

        public static SearchInteractionEntry Import(string? text, string? subline, string? sublineImagePath, string? decoratorText, string? decoratorImagePath, string? info, object? data, bool canBeMadeFavorite, bool isFavorite, Func<SearchInteractionEntry, Task> handleToggleIsFavorite, Func<SearchInteractionEntry, bool> canHandleToggleIsFavorite, Func<InteractionEntry, Task> handleInteract, Func<InteractionEntry, bool> canHandleInteract)
        {
            return new SearchInteractionEntry(text, subline, sublineImagePath, decoratorText, decoratorImagePath, info, data, canBeMadeFavorite, isFavorite, handleToggleIsFavorite, canHandleToggleIsFavorite, handleInteract, canHandleInteract);
        }

        public object Clone()
        {
            return new SearchInteractionEntry(Text, Subline, SublineImagePath, DecoratorText, DecoratorImagePath, Info, Data, CanBeMadeFavorite, IsFavorite, _handleToggleIsFavorite, _canHandleToggleIsFavorite, NavigateHandler, CanNavigateHandle);
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
