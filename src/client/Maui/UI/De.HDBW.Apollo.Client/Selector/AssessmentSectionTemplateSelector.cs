// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Selector
{
    public class AssessmentSectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? ContinueSessionTemplate { get; set; }

        public DataTemplate? RepeatSessionTemplate { get; set; }

        public DataTemplate? DecoTemplate { get; set; }

        public DataTemplate? IconTextTemplate { get; set; }

        public DataTemplate? HeadlineTextTemplate { get; set; }

        public DataTemplate? SublineTextTemplate { get; set; }

        public DataTemplate? TextTemplate { get; set; }

        public DataTemplate? SkillAssessmentTemplate { get; set; }

        public DataTemplate? IsMemberOnlyTemplate { get; set; }

        public DataTemplate? TileTemplate { get; set; }

        public DataTemplate? InteractionTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; } = new DataTemplate();

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case TestSessionEntry session:
                    return session.CanContinue ? ContinueSessionTemplate : RepeatSessionTemplate;
                case InteractionEntry _:
                    return InteractionTemplate;
                case DecoEntry _:
                    return DecoTemplate;
                case IconTextEntry _:
                    return IconTextTemplate;
                case HeadlineTextEntry _:
                    return HeadlineTextTemplate;
                case SublineTextEntry _:
                    return SublineTextTemplate;
                case TextEntry _:
                    return TextTemplate;
                case AssessmentTileEntry tile:
                    if (tile.NeedsUser && !tile.HasUser)
                    {
                        return IsMemberOnlyTemplate;
                    }

                    if (tile.Type == AssessmentType.So)
                    {
                        return SkillAssessmentTemplate;
                    }

                    return TileTemplate;
                case ModuleTileEntry tile:
                    if (tile.NeedsUser && !tile.HasUser)
                    {
                        return IsMemberOnlyTemplate;
                    }

                    return TileTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
