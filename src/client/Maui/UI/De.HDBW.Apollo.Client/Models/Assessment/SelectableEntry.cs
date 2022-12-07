// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SelectableEntry<TU> : ObservableObject, ISelectionInteractiveEntry
        where TU : class
    {
        private static readonly Random s_random = new Random((int)DateTime.Now.Ticks);
        private bool _isSelected;
        private InteractionType _interaction;

        private SelectableEntry(TU data, InteractionType interaction, AnswerType? answerType, Action<SelectableEntry<TU>>? selectionChangedHandler)
        {
            ArgumentNullException.ThrowIfNull(data);
            Data = data;
            AnswerType = answerType;
            Interaction = interaction;
            SelectionChangedHandler = selectionChangedHandler;
            SortIndex = s_random.Next(1000);
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    SelectionChangedHandler?.Invoke(this);
                }
            }
        }

        public object Data { get; }

        public InteractionType Interaction
        {
            get { return _interaction; }
            set { SetProperty(ref _interaction, value); }
        }

        public AnswerType? AnswerType { get; }

        public int SortIndex { get; }

        private Action<SelectableEntry<TU>>? SelectionChangedHandler { get; }

        public static SelectableEntry<TU> Import(TU data, InteractionType interaction, AnswerType? answerType, Action<SelectableEntry<TU>>? selectionChangedHandler)
        {
            return new SelectableEntry<TU>(data, interaction, answerType, selectionChangedHandler);
        }

        public void UpdateSelectedState(bool isSelected)
        {
            _isSelected = isSelected;
            OnPropertyChanged(nameof(IsSelected));
        }

        public TU? GetData()
        {
            return Data as TU;
        }

        [RelayCommand]
        private void ToggleSelection()
        {
            IsSelected = !IsSelected;
        }
    }
}
