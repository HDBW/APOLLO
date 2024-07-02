// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Helper;
using Invite.Apollo.App.Graph.Common.Models.Assessments;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public abstract class AbstractQuestion
    {
        protected AbstractQuestion(RawData data, string rawDataId, string modulId, string assessmentId, CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(culture);
            ArgumentNullException.ThrowIfNull(rawDataId);
            ArgumentNullException.ThrowIfNull(modulId);
            ArgumentNullException.ThrowIfNull(assessmentId);
            Culture = culture;
            IsRTL = Culture.TextInfo.IsRightToLeft;
            Data = data;
            RawDataId = rawDataId;
            ModulId = modulId;
            AssessmentId = assessmentId;
            var parts = data.stemm.ToTextAndQuestion(culture);
            Text = parts.Text;
            Question = parts.Question;
        }

        public bool IsRTL { get; }

        public CultureInfo Culture { get; }

        public string? Instruction
        {
            get { return Data.instruction; }
        }

        public string? Text { get; }

        public string? Question { get; }

        public string RawDataId { get; }

        public string ModulId { get; }

        public string AssessmentId { get; }

        protected RawData Data { get; }

        public RawData Export()
        {
            return Data;
        }
    }
}
