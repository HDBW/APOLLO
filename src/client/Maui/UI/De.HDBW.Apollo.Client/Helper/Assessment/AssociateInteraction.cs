// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Helper.Assessment
{
    public class AssociateInteraction : IInteraction
    {
        public AssociateInteraction(QuestionEntry question, IInteractiveEntry source, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(question);
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(logger);
            Question = question;
            Logger = logger;
            Source = source;
        }

        private QuestionEntry Question { get; }

        private ILogger Logger { get; }

        private IInteractiveEntry Source { get; }

        public void Execute(IInteractiveEntry entry)
        {
            Logger.LogDebug($"Called {nameof(Execute)} in {GetType().Name} with {entry}.");
            Logger.LogDebug($"Source is {Source}.");
            var source = Source as DragSourceEntry<AnswerEntry>;
            var target = entry as DropTagetEntry<QuestionDetailEntry>;
            if (source == null)
            {
                Logger.LogError($"Source is not instance of type {nameof(DragSourceEntry<AnswerEntry>)}.");
                return;
            }

            if (target == null)
            {
                Logger.LogError($"Target is not instance of type {nameof(DropTagetEntry<QuestionDetailEntry>)}.");
                return;
            }

            var selectedAnswer = target.GetData();
            var answer = source.GetData();
            if (selectedAnswer == null)
            {
                Logger.LogError($"SelectedAnswer is null.");
                return;
            }

            if (answer == null)
            {
                Logger.LogError($"Answer is null.");
                return;
            }

            if (target.ClearAssociationCommand.CanExecute(null))
            {
                target.ClearAssociationCommand.Execute(null);
            }

            source.IsAssociated = true;
            target.AssociatedIndex = source.IndexToAssociate;
            answer.CurrentValue = GetValueForSelectedAnswer(selectedAnswer, answer.AnswerType)!;
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
