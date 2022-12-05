// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
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
        private static readonly Random s_random = new Random((int)DateTime.Now.Ticks);
        private readonly QuestionItem _questionItem;
        private readonly IEnumerable<MetaDataItem> _questionMetaDataItems = new List<MetaDataItem>();
        private readonly ILogger _logger;

        [ObservableProperty]
        private ObservableCollection<IInteractiveEntry> _answers = new ObservableCollection<IInteractiveEntry>();

        [ObservableProperty]
        private ObservableCollection<IInteractiveEntry> _details = new ObservableCollection<IInteractiveEntry>();

        [ObservableProperty]
        private string? _imagePath;

        private LayoutType? _questionLayout;
        private LayoutType? _answerLayout;
        private InteractionType? _interaction;
        private IInteraction? _currentInteraction;
        private int? _selectedDetailIndex;

        private QuestionEntry(
            bool sort,
            QuestionItem questionItem,
            IEnumerable<MetaDataItem> questionMetaDataItems,
            Dictionary<MetaDataItem, IEnumerable<MetaDataItem>> questionDetailMetaData,
            IEnumerable<AnswerItem> answerItems,
            IEnumerable<AnswerItemResult> answerResultItems,
            Dictionary<AnswerItem, IEnumerable<MetaDataItem>> answerMetaDataItems,
            ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(questionItem);
            ArgumentNullException.ThrowIfNull(questionMetaDataItems);
            ArgumentNullException.ThrowIfNull(questionDetailMetaData);
            ArgumentNullException.ThrowIfNull(answerItems);
            ArgumentNullException.ThrowIfNull(answerResultItems);
            ArgumentNullException.ThrowIfNull(answerMetaDataItems);
            ArgumentNullException.ThrowIfNull(logger);
            _questionItem = questionItem;
            _questionMetaDataItems = questionMetaDataItems;
            _logger = logger;

            SortIndex = s_random.Next(1000);
            var questionMetaDataItemsList = _questionMetaDataItems?.ToList() ?? new List<MetaDataItem>();
            var firstTextMetaData = questionMetaDataItemsList.FirstOrDefault(m => m.Type == MetaDataType.Text);
            var index = firstTextMetaData != null ? questionMetaDataItemsList.IndexOf(firstTextMetaData) + 1 : 0;

            var detailMetaDatas = _questionMetaDataItems?.Skip(index).Where(m => m.Type == MetaDataType.Image || m.Type == MetaDataType.Text) ?? new List<MetaDataItem>();

            foreach (var detailMetaData in detailMetaDatas)
            {
                var metaData = new List<MetaDataItem>() { detailMetaData };
                if (questionDetailMetaData.ContainsKey(detailMetaData))
                {
                    metaData.AddRange(questionDetailMetaData[detailMetaData]);
                }

                IInteractiveEntry? detail = null;
                switch (Interaction)
                {
                    case InteractionType.Associate:
                        detail = DropTagetEntry<QuestionDetailEntry>.Import(QuestionDetailEntry.Import(metaData), null, questionItem.Interaction, AnswerType.Unknown, HandleAssociateTargetInteraction, HandleClearAssociateTargetInteraction, _logger);
                        break;
                    default:
                        detail = SelectableEntry<QuestionDetailEntry>.Import(QuestionDetailEntry.Import(metaData), InteractionType.SingleSelect, AnswerType.Unknown, null);
                        break;
                }

                Details.Add(detail);
            }

            if (sort)
            {
                Details = new ObservableCollection<IInteractiveEntry>(Details.OrderBy(d => d.SortIndex));
            }

            OnPropertyChanged(nameof(HasDetails));

            _selectedDetailIndex = Details.Any() ? 0 : null;
            if (_selectedDetailIndex.HasValue)
            {
                SelectDetail(_selectedDetailIndex.Value);
            }

            var anserList = answerItems.ToList();
            switch (Interaction)
            {
                case InteractionType.Associate:
                    Answers = new ObservableCollection<IInteractiveEntry>(anserList.Select(a => DragSourceEntry<AnswerEntry>.Import(
                        AnswerEntry.Import(
                            a,
                            answerResultItems.FirstOrDefault(r => r.AnswerItemId == a.Id) ?? new AnswerItemResult() { Id = -1, QuestionItemId = questionItem.Id, AnswerItemId = a.Id, AssessmentItemId = questionItem.AssessmentId },
                            answerMetaDataItems: answerMetaDataItems[a]),
                        anserList.IndexOf(a) + 1,
                        false,
                        Interaction,
                        a.AnswerType,
                        HandleAssociateStartingInteraction,
                        HandleAssociateCompletedInteraction,
                        _logger)));
                    break;
                case InteractionType.Input:
                    Answers = new ObservableCollection<IInteractiveEntry>(anserList.Select(a => InputEntry<AnswerEntry>.Import(
                       AnswerEntry.Import(
                           a,
                           answerResultItems.FirstOrDefault(r => r.AnswerItemId == a.Id) ?? new AnswerItemResult() { Id = -1, QuestionItemId = questionItem.Id, AnswerItemId = a.Id, AssessmentItemId = questionItem.AssessmentId },
                           answerMetaDataItems: answerMetaDataItems[a]),
                       Interaction,
                       a.AnswerType,
                       HandleInteraction)));
                    break;
                default:
                    Answers = new ObservableCollection<IInteractiveEntry>(answerItems.Select(a => SelectableEntry<AnswerEntry>.Import(
                        AnswerEntry.Import(
                            a,
                            answerResultItems.FirstOrDefault(r => r.AnswerItemId == a.Id) ?? new AnswerItemResult() { Id = -1, QuestionItemId = questionItem.Id, AnswerItemId = a.Id, AssessmentItemId = questionItem.AssessmentId },
                            answerMetaDataItems: answerMetaDataItems[a]),
                        Interaction,
                        a.AnswerType,
                        HandleInteraction)));
                    break;
            }

            if (sort)
            {
                Answers = new ObservableCollection<IInteractiveEntry>(Answers.OrderBy(a => a.SortIndex));
            }

            RefreshCommands();
        }

        public int SortIndex { get; }

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

        public int? SelectedDetailIndex
        {
            get
            {
                return _selectedDetailIndex;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                if (SetProperty(ref _selectedDetailIndex, value))
                {
                    SelectDetailCommand.Execute(_selectedDetailIndex);
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

        public bool HasDetails
        {
            get
            {
                return Details.Count > 1;
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
            bool sort,
            QuestionItem questionItem,
            IEnumerable<MetaDataItem> questionMetaDataItems,
            Dictionary<MetaDataItem, IEnumerable<MetaDataItem>> questionDetailMetaData,
            IEnumerable<AnswerItem> answerItems,
            IEnumerable<AnswerItemResult> answerResultItems,
            Dictionary<AnswerItem, IEnumerable<MetaDataItem>> answerMetaDataItems,
            ILogger logger)
        {
            return new QuestionEntry(sort, questionItem, questionMetaDataItems, questionDetailMetaData, answerItems, answerResultItems, answerMetaDataItems, logger);
        }

        public IEnumerable<AnswerItemResult> ExportResultes()
        {
            return Answers.Select(a => a.Data).OfType<AnswerEntry>().Select(m => m.ExportResult()).ToList();
        }

        private void HandleInteraction(IInteractiveEntry entry)
        {
            try
            {
                _currentInteraction = ItemInteractionFactory.CreateInteraction(entry.Interaction, this, entry, _logger);
                _currentInteraction?.Execute(entry);
                _currentInteraction = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error while {nameof(HandleInteraction)} in {GetType().Name}.");
            }
        }

        private void HandleAssociateCompletedInteraction(IAssociateSourceInteractiveEntry source)
        {
            _currentInteraction = null;
        }

        private void HandleAssociateStartingInteraction(IAssociateSourceInteractiveEntry source)
        {
            try
            {
                _currentInteraction = ItemInteractionFactory.CreateInteraction(source.Interaction, this, source, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error while {nameof(HandleAssociateStartingInteraction)} in {GetType().Name}.");
            }
        }

        private void HandleAssociateTargetInteraction(IAssociateTargetInteractiveEntry target)
        {
            try
            {
                _currentInteraction?.Execute(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown error while {nameof(HandleAssociateTargetInteraction)} in {GetType().Name}.");
            }
        }

        private void HandleClearAssociateTargetInteraction(DropTagetEntry<QuestionDetailEntry> target)
        {
            if (!target.AssociatedIndex.HasValue)
            {
                return;
            }

            try
            {
                var source = Answers[target.AssociatedIndex.Value - 1] as DragSourceEntry<AnswerEntry>;
                if (source == null)
                {
                    throw new NotSupportedException("Clearing association on not exiting source.");
                }

                source.IsAssociated = false;
                target.AssociatedIndex = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown Error while {nameof(HandleAssociateTargetInteraction)} in {GetType().Name}.");
            }
        }

        private void RefreshCommands()
        {
            SelectDetailCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanSelectDetail))]
        private void SelectDetail(int index)
        {
            var detailEntry = Details[index].Data as QuestionDetailEntry;
            if (QuestionLayout == LayoutType.Compare)
            {
                ImagePath = detailEntry?.ImagePath;
            }
            else
            {
                ImagePath = !HasDetails ? detailEntry?.ImagePath : null;
            }

            OnPropertyChanged(nameof(HasImage));
        }

        private bool CanSelectDetail(int index)
        {
            return index > -1 && index < Details.Count;
        }
    }
}
