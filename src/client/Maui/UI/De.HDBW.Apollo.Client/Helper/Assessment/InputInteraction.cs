// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper.Assessment
{
    public class InputInteraction : IInteraction
    {
        public InputInteraction(QuestionEntry question, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(question);
            ArgumentNullException.ThrowIfNull(logger);
            Question = question;
            Logger = logger;
        }

        private QuestionEntry Question { get; }

        private ILogger Logger { get; }

        public void Execute(IInteractiveEntry entry)
        {
            Logger.LogDebug($"Called {nameof(Execute)} in {GetType().Name} with {entry}.");
            var input = entry as InputEntry<AnswerEntry>;
            if (input == null)
            {
                Logger.LogError($"Entry is not instance of type {nameof(InputEntry<AnswerEntry>)}.");
                return;
            }

            var answer = input.GetData();
            if (answer == null)
            {
                Logger.LogError($"Answer is null.");
                return;
            }

            answer.CurrentValue = input.Value !;
            Logger.LogDebug($"Set result of answer with id {answer.Id} to {answer.CurrentValue ?? "null"}.");
            Logger.LogDebug($"Correct answer for id {answer.Id} would be {answer.Value}.");
            Logger.LogDebug($"The answer for id {answer.Id} is {(answer.IsCorrect ? "Correct" : "Incorrect")}.");
            Logger.LogDebug($"The question with id {Question.Id} is {(Question.IsCorrect ? "Correct" : "Incorrect")}.");
        }

        private string? GetValueForSelectedAnswer(QuestionDetailEntry entry, AnswerType answerType)
        {
            switch (answerType)
            {
                case AnswerType.Long:
                    return entry?.Id.ToString();
                default:
                    return null;
            }
        }
    }
}
