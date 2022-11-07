// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Helper.Assessment;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class QuestionEntry : ObservableObject
    {
        private readonly QuestionItem _questionItem;
        private readonly IEnumerable<MetaDataItem> _questionMetaDataItems;
        private readonly ILogger _logger;

        [ObservableProperty]
        private string? _imagePath;
        [ObservableProperty]
        private ObservableCollection<IInteractiveEntry> _answers = new ObservableCollection<IInteractiveEntry>();

        private LayoutType? _questionLayout;
        private LayoutType? _answerLayout;
        private InteractionType? _interaction;

        private QuestionEntry(
            QuestionItem questionItem,
            IEnumerable<MetaDataItem> questionMetaDataItems,
            IEnumerable<AnswerItem> answerItems,
            IEnumerable<AnswerItemResult> answerResultItems,
            Dictionary<AnswerItem, IEnumerable<MetaDataItem>> answerMetaDataItems,
            ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(questionItem);
            ArgumentNullException.ThrowIfNull(questionMetaDataItems);
            ArgumentNullException.ThrowIfNull(answerItems);
            ArgumentNullException.ThrowIfNull(answerResultItems);
            ArgumentNullException.ThrowIfNull(answerMetaDataItems);
            ArgumentNullException.ThrowIfNull(logger);
            _questionItem = questionItem;
            _questionMetaDataItems = questionMetaDataItems;
            _logger = logger;

            ImagePath = _questionMetaDataItems?.FirstOrDefault(m => m.Type == MetaDataType.Image)?.Value?.ToUniformedName();
            OnPropertyChanged(nameof(HasImage));
            switch (Interaction)
            {
                default:
                    Answers = new ObservableCollection<IInteractiveEntry>(answerItems.Select(a => SelectableEntry<AnswerEntry>.Import(
                        AnswerEntry.Import(
                            a,
                            answerResultItems.FirstOrDefault(r => r.AnswerItemId == a.Id) ?? new AnswerItemResult() { QuestionItemId = questionItem.Id, AnswerItemId = a.Id, AssessmentItemId = questionItem.AssessmentId },
                            answerMetaDataItems: answerMetaDataItems[a]),
                        Interaction,
                        HandleInteraction)));
                    break;
            }
        }

        public LayoutType QuestionLayout
        {
            get { return _questionLayout ?? _questionItem.QuestionLayout; }
            set { SetProperty(ref _questionLayout, value); }
        }

        public LayoutType AnswerLayout
        {
            get { return _answerLayout ?? _questionItem.AnswerLayout; }
            set { SetProperty(ref _answerLayout, value); }
        }

        public InteractionType Interaction
        {
            get
            {
                return _interaction ?? _questionItem.Interaction;
            }

            set
            {
                if (SetProperty(ref _interaction, value))
                {
                    foreach (var answer in Answers ?? new ObservableCollection<IInteractiveEntry>())
                    {
                        answer.Interaction = value;
                    }
                }
            }
        }

        public string Question
        {
            get
            {
                return _questionMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Text)?.Value ?? string.Empty;
            }
        }

        public bool HasQuestion
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Question);
            }
        }

        public string Instruction
        {
            get
            {
                return _questionMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Hint)?.Value ?? string.Empty;
            }
        }

        public bool HasInstruction
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Instruction);
            }
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public long Id
        {
            get
            {
                return _questionItem.Id;
            }
        }

        public bool IsCorrect
        {
            get
            {
                return Answers.Select(a => a.Data).OfType<AnswerEntry>().All(a => a.IsCorrect);
            }
        }

        public static QuestionEntry Import(
            QuestionItem questionItem,
            IEnumerable<MetaDataItem> questionMetaDataItems,
            IEnumerable<AnswerItem> answerItems,
            IEnumerable<AnswerItemResult> answerResultItems,
            Dictionary<AnswerItem, IEnumerable<MetaDataItem>> answerMetaDataItems,
            ILogger logger)
        {
            return new QuestionEntry(questionItem, questionMetaDataItems, answerItems, answerResultItems, answerMetaDataItems, logger);
        }

        private void HandleInteraction(IInteractiveEntry entry)
        {
            var interaction = AnswerItemInteractionFactory.Create(entry.Interaction, this, _logger);
            interaction?.Execute(entry);
        }
    }
}
