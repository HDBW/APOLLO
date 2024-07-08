// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SortEntry : AbstractQuestionEntry<Sort>
    {
        [ObservableProperty]
        private ObservableCollection<DraggableEntry> _sortTexts;
        private int _draggingItemIndex;

        private SortEntry(Sort data)
            : base(data)
        {
            SortTexts = new ObservableCollection<DraggableEntry>(data.SortTexts.Select(x => DraggableEntry.Import(
                x,
                data.SortTexts.IndexOf(x),
                HandleDragStartingInteraction,
                HandleDragCompletedInteraction,
                HandleDropInteraction)));
        }

        public static SortEntry Import(Sort data)
        {
            return new SortEntry(data);
        }

        public override double? GetScore()
        {
            return Data.CalculateScore(string.Join(";", SortTexts.Select(x => x.OriginalIndex + 1)));
        }

        private void HandleDragStartingInteraction(DraggableEntry entry)
        {
            _draggingItemIndex = SortTexts.IndexOf(entry);
        }

        private void HandleDragCompletedInteraction(DraggableEntry entry)
        {
            _draggingItemIndex = 0;
        }

        private void HandleDropInteraction(DraggableEntry entry)
        {
            var targetIndex = SortTexts.IndexOf(entry);
            SortTexts.Move(_draggingItemIndex, targetIndex);
            foreach (var item in SortTexts)
            {
                item.Index = SortTexts.IndexOf(item);
            }
        }
    }
}
