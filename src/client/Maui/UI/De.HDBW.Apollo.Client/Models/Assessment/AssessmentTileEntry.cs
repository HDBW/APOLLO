// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AssessmentTileEntry : ObservableObject
    {
        private readonly Func<AssessmentTileEntry, CancellationToken, Task> _interactHandler;
        private readonly Func<AssessmentTileEntry, bool> _canInteractHandler;
        private readonly Func<AssessmentTileEntry, CancellationToken, Task> _toggleFavoriteHandler;
        private readonly Func<AssessmentTileEntry, bool> _canToggleFavoriteHandler;
        private AssessmentTile _data;

        [ObservableProperty]
        private bool _isFavorite;

        [ObservableProperty]
        private List<ModuleScoreEntry> _segments = new List<ModuleScoreEntry>();

        private AssessmentTileEntry(
            AssessmentTile data,
            string? route,
            NavigationParameters? parameters,
            bool isFavorite,
            Func<AssessmentTileEntry, CancellationToken, Task> interactHandler,
            Func<AssessmentTileEntry, bool> canInteractHandler,
            Func<AssessmentTileEntry, CancellationToken, Task> toggleFavoriteHandler,
            Func<AssessmentTileEntry, bool> canToggleFavoriteHandler)
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
            Segments = new List<ModuleScoreEntry>(data.ModuleScores.Select(x => ModuleScoreEntry.Import(x, string.Empty, data.Type)));
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

        public bool HasDecoratorImage
        {
            get
            {
                return !string.IsNullOrEmpty(DecoratorImagePath);
            }
        }

        public bool NeedsUser
        {
            get
            {
                return _data.MemberOnly;
            }
        }

        public bool CanStart
        {
            get
            {
                return (_data.Repeatable ?? 0) == 0;
            }
        }

        public bool IsMemberOnly
        {
            get
            {
                return _data.MemberOnly;
            }
        }

        public bool CanBeMadeFavorite
        {
            get
            {
                return (_data.ModuleIds?.Count ?? 0) == 1;
            }
        }

        public string AssessmentId
        {
            get
            {
                return _data.AssessmentId;
            }
        }

        public IEnumerable<string> ModuleIds
        {
            get
            {
                return _data.ModuleIds?.ToList() ?? new List<string>();
            }
        }

        public string Title
        {
            get
            {
                switch (Type)
                {
                    case AssessmentType.Sk:
                        return Resources.Strings.Resources.AssessmentTypeSk;
                    case AssessmentType.Ea:
                        return Resources.Strings.Resources.AssessmentTypeEa;
                    case AssessmentType.So:
                        return Resources.Strings.Resources.AssessmentTypeSo;
                    case AssessmentType.Gl:
                        return Resources.Strings.Resources.AssessmentTypeGl;
                    case AssessmentType.Be:
                        return Resources.Strings.Resources.AssessmentTypeBe;
                    default:
                        return string.Empty;
                }
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

        public static AssessmentTileEntry Import(
            AssessmentTile data,
            string? route,
            NavigationParameters? parameters,
            bool isFavorite,
            Func<AssessmentTileEntry, CancellationToken, Task> interactHandler,
            Func<AssessmentTileEntry, bool> canInteractHandler,
            Func<AssessmentTileEntry, CancellationToken, Task> toggleFavoriteHandler,
            Func<AssessmentTileEntry, bool> canToggleFavoriteHandler)
        {
            return new AssessmentTileEntry(
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
            IsFavorite = !IsFavorite;
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
