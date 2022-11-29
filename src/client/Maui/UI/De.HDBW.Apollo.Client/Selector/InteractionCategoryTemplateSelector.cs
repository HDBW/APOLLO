// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Interactions;

namespace De.HDBW.Apollo.Client.Selector
{
    public class InteractionCategoryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? FavoriteTemplate { get; set; }

        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as InteractionCategoryEntry;
            DataTemplate? result = null;
            switch (entry)
            {
                case FavoriteInteractionCategoryEntry:
                    result = FavoriteTemplate;
                    break;
            }

            return result ?? DefaultTemplate;
        }
    }
}
