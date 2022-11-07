// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper.Assessment
{
    public static class AnswerItemInteractionFactory
    {
        public static IAnswerItemInteraction Create(InteractionType interaction, QuestionEntry entry, ILogger logger)
        {
            switch (interaction)
            {
                case InteractionType.SingleSelect:
                    return new SingleSelectInteraction(entry, logger);
                case InteractionType.MultiSelect:
                    return new MultiSelectSelectInteraction(entry, logger);
                default:
                    return null;
            }
        }
    }
}
