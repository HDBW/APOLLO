// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class InputEntry<TU> : ObservableObject, IInputInteractiveEntry
         where TU : AnswerEntry
    {
        private static readonly Random s_random = new Random((int)DateTime.Now.Ticks);
        private string? _value;
        private InteractionType _interaction;

        private InputEntry(TU data, InteractionType interaction, AnswerType? answerType, Action<InputEntry<TU>>? valueChangedHandler)
        {
            ArgumentNullException.ThrowIfNull(data);
            Data = data;
            Interaction = interaction;
            AnswerType = answerType;
            ValueChangedHandler = valueChangedHandler;
            SortIndex = s_random.Next(1000);
        }

        public string? Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (SetProperty(ref _value, value))
                {
                    ValueChangedHandler?.Invoke(this);
                }
            }
        }

        public object Data { get; }

        public string? Hint
        {
            get
            {
                return GetData()?.Hint;
            }
        }

        public AnswerType? AnswerType { get; }

        public InteractionType Interaction
        {
            get { return _interaction; }
            set { SetProperty(ref _interaction, value); }
        }

        public int SortIndex { get; }

        private Action<InputEntry<TU>>? ValueChangedHandler { get; }

        public static InputEntry<TU> Import(TU data, InteractionType interaction, AnswerType? answerType, Action<InputEntry<TU>>? valueChangedHandler)
        {
            return new InputEntry<TU>(data, interaction, answerType, valueChangedHandler);
        }

        public TU? GetData()
        {
            return Data as TU;
        }
    }
}
