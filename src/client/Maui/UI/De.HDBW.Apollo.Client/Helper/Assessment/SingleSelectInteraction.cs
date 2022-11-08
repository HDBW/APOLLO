// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper.Assessment
{
    public class SingleSelectInteraction : AbstractSelectInteraction, IInteraction
    {
        public SingleSelectInteraction(QuestionEntry question, ILogger logger)
            : base(question, logger)
        {
        }

        public override void Execute(IInteractiveEntry entry)
        {
            var answers = Question.Answers;

            // Deselect all other answers.
            var itemsToDeselect = answers.Where(a => a != entry).OfType<ISelectableEntry>();
            foreach (var item in itemsToDeselect)
            {
                item.UpdateSelectedState(false);
            }

            base.Execute(entry);
        }
    }
}
