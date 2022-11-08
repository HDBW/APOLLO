// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class IInteractiveEntryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? SelectInteractionTemplate { get; set; }

        public DataTemplate? AssociateInteractionTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as IInteractiveEntry;
            if (entry != null)
            {
                switch (entry.Interaction)
                {
                    case InteractionType.SingleSelect:
                    case InteractionType.MultiSelect:
                        return SelectInteractionTemplate;
                    case InteractionType.Associate:
                        return AssociateInteractionTemplate;
                }
            }

            return DefaultTemplate;
        }
    }
}
