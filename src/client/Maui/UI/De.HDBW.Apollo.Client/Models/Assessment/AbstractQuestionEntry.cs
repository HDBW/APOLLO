﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.SharedContracts.Questions;
using Microsoft.Maui.Platform;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public abstract partial class AbstractQuestionEntry<TU> : ObservableObject, IAbstractQuestionEntry
        where TU : AbstractQuestion
    {
        public AbstractQuestionEntry(TU data)
        {
            ArgumentNullException.ThrowIfNull(data);
            Data = data;
        }

        public string? Instruction
        {
            get { return Data.Instruction; }
        }

        public string? InstructionHtml
        {
            get
            {
                return Instruction.EnsureHtmlFlowDirection(Data.Culture);
            }
        }

        public string? Text
        {
            get { return Data.Text; }
        }

        public string? Question
        {
            get { return Data.Question; }
        }

        public string? QuestionHtml
        {
            get
            {
                return Question.EnsureHtmlFlowDirection(Data.Culture);
            }
        }

        public bool? IsRTL
        {
            get { return Data.IsRTL; }
        }

        public abstract bool DidInteract { get; protected set; }

        protected TU Data { get; }

        public AbstractQuestion Export()
        {
            return Data;
        }

        public abstract double? GetScore();
    }
}
