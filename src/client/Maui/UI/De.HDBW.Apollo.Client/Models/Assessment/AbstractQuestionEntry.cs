// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AbstractQuestionEntry : ObservableObject
    {
        public AbstractQuestionEntry(AbstractQuestion data)
        {
            ArgumentNullException.ThrowIfNull(data);
            Data = data;
        }

        public string? Instruction
        {
            get { return Data.Instruction; }
        }

        public string? Text
        {
            get { return Data.Text; }
        }

        public string? Question
        {
            get { return Data.Question; }
        }

        public bool? IsRTL
        {
            get { return Data.IsRTL; }
        }

        protected AbstractQuestion Data { get; }

        public AbstractQuestion Export()
        {
            return Data;
        }
    }
}
