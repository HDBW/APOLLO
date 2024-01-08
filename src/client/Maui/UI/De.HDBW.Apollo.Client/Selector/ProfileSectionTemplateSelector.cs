// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Generic;
using De.HDBW.Apollo.Client.Models.Interactions;

namespace De.HDBW.Apollo.Client.Selector
{
    internal class ProfileSectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? InteractionEntryTemplate { get; set; }

        public DataTemplate? StringValueTemplate { get; set; }

        public DataTemplate? SeperatorValueTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case StringValue _:
                    return StringValueTemplate;
                case SeperatorValue _:
                    return SeperatorValueTemplate;
                case InteractionEntry _:
                    return InteractionEntryTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
