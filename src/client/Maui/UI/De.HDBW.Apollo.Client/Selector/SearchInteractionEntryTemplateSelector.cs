// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Interactions;

namespace De.HDBW.Apollo.Client.Selector
{
    public class SearchInteractionEntryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? ImageTemplate { get; set; }

        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as SearchInteractionEntry;
            switch (entry?.HasSublineImage)
            {
                case true:
                    return ImageTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}