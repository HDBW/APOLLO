// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper.Assessment
{
    public static class ItemInteractionFactory
    {
        public static IInteraction CreateInteraction(InteractionType interaction, QuestionEntry question, IInteractiveEntry entry, ILogger logger)
        {
            switch (interaction)
            {
                case InteractionType.SingleSelect:
                    return new SingleSelectInteraction(question, logger);
                case InteractionType.MultiSelect:
                    return new MultiSelectSelectInteraction(question, logger);
                case InteractionType.Associate:
                    return new AssociateInteraction(question, entry, logger);
                default:
                    return null;
            }
        }
    }
}
