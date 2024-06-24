// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ModuleTileEntry : ObservableObject
    {
        private readonly Func<ModuleTileEntry, CancellationToken, Task> _interactHandler;
        private readonly Func<ModuleTileEntry, bool> _canInteractHandler;
        private readonly Func<ModuleTileEntry, CancellationToken, Task> _toggleFavoriteHandler;
        private readonly Func<ModuleTileEntry, bool> _canToggleFavoriteHandler;
        private ModuleTile _data;

        [ObservableProperty]
        private bool _isFavorite;

        private ModuleTileEntry(
            ModuleTile data,
            string? route,
            NavigationParameters? parameters,
            bool isFavorite,
            Func<ModuleTileEntry, CancellationToken, Task> interactHandler,
            Func<ModuleTileEntry, bool> canInteractHandler,
            Func<ModuleTileEntry, CancellationToken, Task> toggleFavoriteHandler,
            Func<ModuleTileEntry, bool> canToggleFavoriteHandler)
        {
            ArgumentNullException.ThrowIfNull(data);
            _data = data;
            Route = route;
            Parameters = parameters;
            IsFavorite = isFavorite;
            _interactHandler = interactHandler;
            _canInteractHandler = canInteractHandler;
            _toggleFavoriteHandler = toggleFavoriteHandler;
            _canToggleFavoriteHandler = canToggleFavoriteHandler;
        }

        public string? Route { get; }

        public NavigationParameters? Parameters { get; }

        public string? DecoratorImagePath
        {
            get
            {
                switch (Type)
                {
                    case AssessmentType.Sk:
                        return KnownIcons.SkillAssessment;
                    case AssessmentType.Ea:
                        return KnownIcons.ExperienceAssessment;
                    case AssessmentType.So:
                        return KnownIcons.SoftSkillAssessment;
                    case AssessmentType.Gl:
                        return KnownIcons.LanguageAssessment;
                    case AssessmentType.Be:
                        return KnownIcons.DiscoverAssessment;
                    default:
                        return null;
                }
            }
        }

        public bool CanBeMadeFavorite
        {
            get
            {
                return true;
            }
        }

        public string AssessmentId
        {
            get
            {
                return _data.AssessmentId;
            }
        }

        public string ModuleId
        {
            get
            {
                return _data.ModuleId;
            }
        }

        public string Title
        {
            get
            {
                return _data.Title;
            }
        }

        public AssessmentType Type
        {
            get
            {
                return _data.Type;
            }
        }

        public string Text
        {
            get
            {
                return _data.Title;
            }
        }

        public static ModuleTileEntry Import(
            ModuleTile data,
            string? route,
            NavigationParameters? parameters,
            bool isFavorite,
            Func<ModuleTileEntry, CancellationToken, Task> interactHandler,
            Func<ModuleTileEntry, bool> canInteractHandler,
            Func<ModuleTileEntry, CancellationToken, Task> toggleFavoriteHandler,
            Func<ModuleTileEntry, bool> canToggleFavoriteHandler)
        {
            return new ModuleTileEntry(
                data,
                route,
                parameters,
                isFavorite,
                interactHandler,
                canInteractHandler,
                toggleFavoriteHandler,
                canToggleFavoriteHandler);
        }

        public void RefreshCommands()
        {
            InteractCommand.NotifyCanExecuteChanged();
            ToggleIsFavoriteCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleIsFavorite))]
        private Task ToggleIsFavorite(CancellationToken token)
        {
            return _toggleFavoriteHandler?.Invoke(this, token) ?? Task.CompletedTask;
        }

        private bool CanToggleIsFavorite()
        {
            return _canToggleFavoriteHandler?.Invoke(this) ?? false;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanInteract))]
        private Task Interact(CancellationToken token)
        {
            return _interactHandler?.Invoke(this, token) ?? Task.CompletedTask;
        }

        private bool CanInteract()
        {
            return _canInteractHandler?.Invoke(this) ?? false;
        }
    }
}
