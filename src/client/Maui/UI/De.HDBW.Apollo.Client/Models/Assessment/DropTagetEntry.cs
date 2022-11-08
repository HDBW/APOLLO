// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class DropTagetEntry<TU> : ObservableObject, IAssociateTargetInteractiveEntry
        where TU : class
    {
        private InteractionType _interaction;
        private int? _associatedIndex;

        public DropTagetEntry(
            TU data,
            int? associatedIndex,
            InteractionType interaction,
            Action<DropTagetEntry<TU>>? dropHandler,
            Action<DropTagetEntry<TU>>? clearHandler,
            ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(logger);
            Data = data;
            Logger = logger;
            AssociatedIndex = associatedIndex;
            Interaction = interaction;
            DropHandler = dropHandler;
            ClearHandler = clearHandler;
        }

        public object Data { get; }

        public InteractionType Interaction
        {
            get { return _interaction; }
            set { SetProperty(ref _interaction, value); }
        }

        public Action<DropTagetEntry<TU>>? DropHandler { get; }

        public Action<DropTagetEntry<TU>>? ClearHandler { get; }

        public int? AssociatedIndex
        {
            get
            {
                return _associatedIndex;
            }

            set
            {
                if (SetProperty(ref _associatedIndex, value))
                {
                    OnPropertyChanged(nameof(HasAssociation));
                    ClearAssociationCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public bool HasAssociation
        {
            get { return AssociatedIndex.HasValue; }
        }

        private ILogger Logger { get; }

        public static DropTagetEntry<TU> Import(
            TU data,
            int? associatedIndex,
            InteractionType interaction,
            Action<DropTagetEntry<TU>>? dropHandler,
            Action<DropTagetEntry<TU>>? clearHandler,
            ILogger logger)
        {
            return new DropTagetEntry<TU>(data, associatedIndex, interaction, dropHandler, clearHandler, logger);
        }

        public TU? GetData()
        {
            return Data as TU;
        }

        [RelayCommand]
        private void Drop()
        {
            DropHandler?.Invoke(this);
        }

        [RelayCommand(CanExecute = nameof(CanClearAssociation))]
        private void ClearAssociation()
        {
            ClearHandler?.Invoke(this);
        }

        private bool CanClearAssociation()
        {
            return HasAssociation;
        }
    }
}
