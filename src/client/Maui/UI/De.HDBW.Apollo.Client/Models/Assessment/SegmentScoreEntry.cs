// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SegmentScoreEntry : ObservableObject
    {
        private readonly Func<SegmentScoreEntry, CancellationToken, Task>? _interactHandler;
        private readonly Func<SegmentScoreEntry, bool>? _canInteractHandler;
        private SegmentScore _score;

        private SegmentScoreEntry(SegmentScore score, string displayQuantity, AssessmentType type, Func<SegmentScoreEntry, CancellationToken, Task>? interactHandler, Func<SegmentScoreEntry, bool>? canInteractHandler)
        {
            ArgumentNullException.ThrowIfNull(score);
            if (type == AssessmentType.Unknown)
            {
                throw new NotSupportedException("AssessmentType.Unknown not supported.");
            }

            _score = score;
            DisplayQuantity = displayQuantity;
            Type = type;
            _interactHandler = interactHandler;
            _canInteractHandler = canInteractHandler;
        }

        public AssessmentType Type { get; }

        public string? Description
        {
            get
            {
                return _score.ResultDescription;
            }
        }

        public AssessmentScoreQuantity Quantity
        {
            get
            {
                return _score.Quantity;
            }
        }

        public string DisplayQuantity { get; }

        public double? Result
        {
            get
            {
                return _score.Result;
            }
        }

        public string? Segment
        {
            get
            {
                return _score.Segment;
            }
        }

        public static SegmentScoreEntry Import(SegmentScore score, string displayQuantity, AssessmentType type, Func<SegmentScoreEntry, CancellationToken, Task>? interactHandler = null, Func<SegmentScoreEntry, bool>? canInteractHandler = null)
        {
            return new SegmentScoreEntry(score, displayQuantity, type, interactHandler, canInteractHandler);
        }

        public void RefreshCommands()
        {
            InteractCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanInteract))]
        private Task Interact(CancellationToken token)
        {
            return _interactHandler?.Invoke(this, token) ?? Task.CompletedTask;
        }

        private bool CanInteract()
        {
            return _canInteractHandler?.Invoke(this) ?? false;
        }
    }
}
