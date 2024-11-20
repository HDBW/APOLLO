// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace De.HDBW.Apollo.Client.Selector
{
    public class OccupationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? UnknownTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as InteractionEntry;
            switch (entry?.Data)
            {
                case Occupation occupation:
                    if (occupation.TaxonomyInfo == Taxonomy.Unknown)
                    {
                        return UnknownTemplate;
                    }

                    return DefaultTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
