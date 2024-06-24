// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AssessmentTileEntry : ObservableObject
    {
        private AssessmentTile _data;

        private AssessmentTileEntry(AssessmentTile data)
        {
            ArgumentNullException.ThrowIfNull(data);
            _data = data;
        }

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
                return _data.ModuleIds.Count == 1;
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleIsFavorite))]
        private Task ToggleIsFavorite()
        {
            return Task.CompletedTask;
        }

        private bool CanToggleIsFavorite()
        {
            return false;
        }

        public static AssessmentTileEntry Import(AssessmentTile data)
        {
            return new AssessmentTileEntry(data);
        }
    }
}
