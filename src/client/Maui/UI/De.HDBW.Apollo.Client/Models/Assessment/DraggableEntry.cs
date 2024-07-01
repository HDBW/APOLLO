// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class DraggableEntry : ObservableObject
    {
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayIndex))]
        private int _index;

        public DraggableEntry(string text, int index, Action<DraggableEntry>? dragStartingHandler, Action<DraggableEntry>? dropCompletedHandler, Action<DraggableEntry>? dropHandler)
        {
            Text = text;
            Index = index;
            DragStartingHandler = dragStartingHandler;
            DropCompletedHandler = dropCompletedHandler;
            DropHandler = dropHandler;
        }

        public int DisplayIndex
        {
            get { return Index + 1; }
        }

        private Action<DraggableEntry>? DragStartingHandler { get; }

        private Action<DraggableEntry>? DropCompletedHandler { get; }

        private Action<DraggableEntry>? DropHandler { get; }

        public static DraggableEntry Import(string text, int index, Action<DraggableEntry>? dragStartingHandler, Action<DraggableEntry>? dropCompletedHandler, Action<DraggableEntry>? dropHandler)
        {
            return new DraggableEntry(text, index, dragStartingHandler, dropCompletedHandler, dropHandler);
        }

        [RelayCommand]
        private void DragStarting()
        {
            try
            {
                DragStartingHandler?.Invoke(this);
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        private void DropCompleted()
        {
            DropCompletedHandler?.Invoke(this);
        }

        [RelayCommand]
        private void Drop()
        {
            DropHandler?.Invoke(this);
        }
    }
}
