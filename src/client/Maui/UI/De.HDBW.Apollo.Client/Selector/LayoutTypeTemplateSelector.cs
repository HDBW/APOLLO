// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class LayoutTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? UniformTemplate { get; set; }

        public DataTemplate? HorizontalTemplate { get; set; }

        public DataTemplate? CompareTemplate { get; set; }

        public DataTemplate? OverlayTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var type = item as LayoutType?;
            DataTemplate? result;
            switch (type)
            {
                case LayoutType.UniformGrid:
                    result = UniformTemplate;
                    break;
                case LayoutType.HorizontalList:
                    result = HorizontalTemplate;
                    break;
                case LayoutType.Compare:
                    result = CompareTemplate;
                    break;
                case LayoutType.Overlay:
                    result = OverlayTemplate;
                    break;
                default:
                    result = DefaultTemplate;
                    break;
            }

            return result ?? DefaultTemplate;
        }
    }
}
