// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class QuestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? OverlayTemplate { get; set; }

        public DataTemplate? CompareTemplate { get; set; }

        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var question = item as QuestionEntry;
            switch (question?.QuestionLayout)
            {
                case LayoutType.Overlay:
                    return OverlayTemplate;
                case LayoutType.Compare:
                    return CompareTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
