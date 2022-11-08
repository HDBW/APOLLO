// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper.Assessment
{
    public abstract class AbstractSelectInteraction
    {
        protected AbstractSelectInteraction(QuestionEntry question, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(question);
            ArgumentNullException.ThrowIfNull(logger);
            Question = question;
            Logger = logger;
        }

        protected QuestionEntry Question { get; }

        protected ILogger Logger { get; }

        public virtual void Execute(IInteractiveEntry entry)
        {
            Logger.LogDebug($"Called {nameof(Execute)} in {GetType().Name} with {entry}.");
            var selectableItems = Question.Answers.OfType<SelectableEntry<AnswerEntry>>();
            foreach (var selectable in selectableItems)
            {
                var answer = selectable.GetData();
                if (answer == null)
                {
                    Logger.LogWarning($"Unable to get answer from selectable {selectable.Data}");
                    continue;
                }

                var resultValue = GetValueForSelectionState(selectable.IsSelected, answer.AnswerType);
                answer.CurrentValue = resultValue!;
                Logger.LogDebug($"Set result of answer with id {answer.Id} to {answer.CurrentValue ?? "null"}.");
                Logger.LogDebug($"Correct answer for id {answer.Id} would be {answer.Value}.");
                Logger.LogDebug($"The answer for id {answer.Id} is {(answer.IsCorrect ? "Correct" : "Incorrect")}.");
            }

            Logger.LogDebug($"The question with id {Question.Id} is {(Question.IsCorrect ? "Correct" : "Incorrect")}.");
        }

        private string? GetValueForSelectionState(bool isSelected, AnswerType answerType)
        {
            switch (answerType)
            {
                case AnswerType.Boolean:
                    return isSelected.ToString(CultureInfo.InvariantCulture);
                default:
                    return null;
            }
        }
    }
}
