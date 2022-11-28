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

        public DataTemplate? IntegerInputInteractionTemplate { get; set; }

        public DataTemplate? StringInputInteractionTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var entry = item as IInteractiveEntry;
            DataTemplate? result = null;
            if (entry != null)
            {
                switch (entry.Interaction)
                {
                    case InteractionType.SingleSelect:
                    case InteractionType.MultiSelect:
                        result = SelectInteractionTemplate;
                        break;
                    case InteractionType.Associate:
                        result = AssociateInteractionTemplate;
                        break;
                    case InteractionType.Input:
                        switch (entry.AnswerType)
                        {
                            case AnswerType.Integer:
                                result = IntegerInputInteractionTemplate;
                                break;
                            case AnswerType.String:
                            case AnswerType.TextBox:
                                result = StringInputInteractionTemplate;
                                break;
                        }

                        break;
                }
            }

            return result ?? DefaultTemplate;
        }
    }
}
