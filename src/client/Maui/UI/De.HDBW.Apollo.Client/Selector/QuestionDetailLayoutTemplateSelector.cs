// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Selector
{
    public class QuestionDetailLayoutTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DefaultTemplate { get; set; }

        public DataTemplate? AssociateTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var type = item as IInteractiveEntry;
            switch (type?.Interaction)
            {
                case InteractionType.Associate:
                    return AssociateTemplate;
                default:
                    return DefaultTemplate;
            }
        }
    }
}
