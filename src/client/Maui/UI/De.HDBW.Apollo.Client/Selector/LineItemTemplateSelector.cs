// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Training;

namespace De.HDBW.Apollo.Client.Selector
{
    public class LineItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? InteractiveLineItemTemplate { get; set; }

        public DataTemplate? LineWithoutIconItemTemplate { get; set; }

        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case InteractiveLineItem _:
                    return InteractiveLineItemTemplate;
                case LineWithoutIconItem _:
                    return LineWithoutIconItemTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
