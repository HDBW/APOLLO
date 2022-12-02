// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class DragSourceEntry<TU> : ObservableObject, IAssociateSourceInteractiveEntry
        where TU : class
    {
        private static readonly Random s_random = new Random((int)DateTime.Now.Ticks);
        private InteractionType _interaction;
        private int _indexToAssociate;
        private bool _isAssociated;

        private DragSourceEntry(TU data, int indexToAssociate, bool isAssociated, InteractionType interaction, AnswerType? answerType,  Action<DragSourceEntry<TU>>? dragStartingHandler, Action<DragSourceEntry<TU>>? dropCompletedHandler, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(logger);
            Data = data;
            IndexToAssociate = indexToAssociate;
            Logger = logger;
            Interaction = interaction;
            AnswerType = answerType;
            IsAssociated = isAssociated;
            DragStartingHandler = dragStartingHandler;
            DropCompletedHandler = dropCompletedHandler;
            SortIndex = s_random.Next(1000);
        }

        public object Data { get; }

        public InteractionType Interaction
        {
            get { return _interaction; }
            set { SetProperty(ref _interaction, value); }
        }

        public int IndexToAssociate
        {
            get { return _indexToAssociate; }
            private set { SetProperty(ref _indexToAssociate, value); }
        }

        public bool IsAssociated
        {
            get
            {
                return _isAssociated;
            }

            set
            {
                if (SetProperty(ref _isAssociated, value))
                {
                    OnPropertyChanged(nameof(IsNotAssociated));
                }
            }
        }

        public bool IsNotAssociated
        {
            get
            {
                return !IsAssociated;
            }
        }

        public AnswerType? AnswerType { get; }

        public int SortIndex { get; }

        private Action<DragSourceEntry<TU>>? DragStartingHandler { get; }

        private Action<DragSourceEntry<TU>>? DropCompletedHandler { get; }

        private ILogger Logger { get; }

        public static DragSourceEntry<TU> Import(TU data, int indexToAssociate, bool isAssociated, InteractionType interaction, AnswerType? answerType, Action<DragSourceEntry<TU>>? dragStartingHandler, Action<DragSourceEntry<TU>>? dropCompletedHandler, ILogger logger)
        {
            return new DragSourceEntry<TU>(data, indexToAssociate, isAssociated, interaction, answerType, dragStartingHandler, dropCompletedHandler, logger);
        }

        public TU? GetData()
        {
            return Data as TU;
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
                Logger?.LogError(ex, $"Unknown error while DragStarting in {GetType().Name}.");
            }
        }

        [RelayCommand]
        private void DropCompleted()
        {
            DropCompletedHandler?.Invoke(this);
        }
    }
}
