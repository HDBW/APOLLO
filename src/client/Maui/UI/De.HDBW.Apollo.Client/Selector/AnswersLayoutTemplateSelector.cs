// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class AnswersLayoutTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? UniformTemplate { get; set; }

        public DataTemplate? HorizontalTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var type = item as LayoutType?;
            switch (type)
            {
                case LayoutType.UniformGrid:
                    return UniformTemplate;
                case LayoutType.HorizontalList:
                    return HorizontalTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
