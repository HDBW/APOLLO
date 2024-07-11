﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Selector
{
    public class ResultSectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? GLDecoTemplate { get; set; }

        public DataTemplate? SKDecoTemplate { get; set; }

        public DataTemplate? EADecoTemplate { get; set; }

        public DataTemplate? SODecoTemplate { get; set; }

        public DataTemplate? GLScoreTemplate { get; set; }

        public DataTemplate? EAScoreTemplate { get; set; }

        public DataTemplate? SKScoreTemplate { get; set; }

        public DataTemplate? SOScoreTemplate { get; set; }

        public DataTemplate? HeadlineTextTemplate { get; set; }

        public DataTemplate? SublineTextTemplate { get; set; }

        public DataTemplate? TextTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; } = new DataTemplate();

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case HeadlineTextEntry _:
                    return HeadlineTextTemplate;

                case SublineTextEntry _:
                    return SublineTextTemplate;

                case TextEntry _:
                    return TextTemplate;

                case DecoEntry deco:
                    switch (deco.Type)
                    {
                        case AssessmentType.Gl:
                            return GLDecoTemplate;
                        case AssessmentType.Sk:
                            return SKDecoTemplate;
                        case AssessmentType.Ea:
                            return EADecoTemplate;
                        case AssessmentType.So:
                            return SODecoTemplate;
                        default:
                            return DefaultTemplate;
                    }

                case ModuleScoreEntry score:
                    switch (score.Type)
                    {
                        case AssessmentType.Gl:
                            return GLScoreTemplate;
                        case AssessmentType.Sk:
                            return SKScoreTemplate;
                        case AssessmentType.Ea:
                            return EAScoreTemplate;
                        case AssessmentType.So:
                            return SOScoreTemplate;
                        default:
                            return DefaultTemplate;
                    }

                default:
                    return DefaultTemplate;
            }
        }
    }
}