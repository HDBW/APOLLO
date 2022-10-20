// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class QuestionEntry : ObservableObject
    {
        private readonly QuestionItem _questionItem;
        private readonly IEnumerable<MetaDataItem> _questionMetaDataItems;
        [ObservableProperty]
        private string? _imagePath;
        [ObservableProperty]
        private ObservableCollection<AnswerEntry> _answers = new ObservableCollection<AnswerEntry>();

        private QuestionEntry(
            QuestionItem questionItem,
            IEnumerable<MetaDataItem> questionMetaDataItems,
            IEnumerable<AnswerItem> answerItems,
            Dictionary<AnswerItem, IEnumerable<MetaDataItem>> answerMetaDataItems)
        {
            ArgumentNullException.ThrowIfNull(questionItem);
            ArgumentNullException.ThrowIfNull(questionMetaDataItems);
            ArgumentNullException.ThrowIfNull(answerItems);
            ArgumentNullException.ThrowIfNull(answerMetaDataItems);

            _questionItem = questionItem;
            _questionMetaDataItems = questionMetaDataItems;

            ImagePath = _questionMetaDataItems?.FirstOrDefault(m => m.Type == MetaDataType.Image)?.Value?.ToLower();
            OnPropertyChanged(nameof(HasImage));
            Answers = new ObservableCollection<AnswerEntry>(answerItems.Select(a => AnswerEntry.Import(a, answerMetaDataItems[a])));
        }

        public LayoutType QuestionLayout
        {
            get { return _questionItem.QuestionLayout; }
        }

        public LayoutType AnswerLayout
        {
            get { return _questionItem.AnswerLayout; }
        }

        public InteractionType Interaction
        {
            get { return _questionItem.Interaction; }
        }

        public string Question
        {
            get
            {
                return _questionMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Text)?.Value ?? string.Empty;
            }
        }

        public string Instruction
        {
            get
            {
                return _questionMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Hint)?.Value ?? string.Empty;
            }
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public static QuestionEntry Import(
            QuestionItem questionItem,
            IEnumerable<MetaDataItem> questionMetaDataItems,
            IEnumerable<AnswerItem> answerItems,
            Dictionary<AnswerItem, IEnumerable<MetaDataItem>> answerMetaDataItems)
        {
            return new QuestionEntry(questionItem, questionMetaDataItems, answerItems, answerMetaDataItems);
        }
    }
}
