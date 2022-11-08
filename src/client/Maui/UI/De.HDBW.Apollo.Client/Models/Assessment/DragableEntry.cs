// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class DragableEntry<TU> : ObservableObject, IAssociateInteractiveEntry
        where TU : class
    {
        private InteractionType _interaction;

        public DragableEntry(TU data, InteractionType interaction, Action<DragableEntry<TU>>? dragStartingHandler, Action<DragableEntry<TU>>? dropCompletedHandler)
        {
            ArgumentNullException.ThrowIfNull(data);
            Data = data;
            Interaction = interaction;
            DragStartingHandler = dragStartingHandler;
            DropCompletedHandler = dropCompletedHandler;
        }

        public object Data { get; }

        public InteractionType Interaction
        {
            get { return _interaction; }
            set { SetProperty(ref _interaction, value); }
        }

        private Action<DragableEntry<TU>>? DragStartingHandler { get; }

        private Action<DragableEntry<TU>>? DropCompletedHandler { get; }

        public static DragableEntry<TU> Import(TU data, InteractionType interaction, Action<DragableEntry<TU>>? dragStartingHandler, Action<DragableEntry<TU>>? dropCompletedHandler)
        {
            return new DragableEntry<TU>(data, interaction, dragStartingHandler, dropCompletedHandler);
        }

        public TU? GetData()
        {
            return Data as TU;
        }

        [RelayCommand]
        private void DragSarting()
        {
        }

        [RelayCommand]
        private void DropCompleted()
        {
        }
    }
}
