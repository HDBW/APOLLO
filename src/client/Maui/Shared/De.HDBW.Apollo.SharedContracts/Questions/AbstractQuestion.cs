// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.SharedContracts.Helper;

namespace De.HDBW.Apollo.SharedContracts.Questions
{
    public abstract class AbstractQuestion
    {
        protected AbstractQuestion(RawData data, string itemId, string compancyId, string bookletId, CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(itemId);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(compancyId);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(bookletId);
            Culture = culture;
            IsRTL = Culture.TextInfo.IsRightToLeft;
            ItemId = itemId.Trim();
            CompancyId = compancyId.Trim();
            BookletId = bookletId.Trim();
            Data = data;
            var parts = data.stemm.ToTextAndQuestion(culture);
            Text = parts.Text;
            Question = parts.Question;
        }

        public bool IsRTL { get; }

        public CultureInfo Culture { get; }

        public string ItemId { get; }

        public string CompancyId { get; }

        public string BookletId { get; }

        public string? Instruction
        {
            get { return Data.instruction; }
        }

        public string? Text { get; }

        public string? Question { get; }

        protected RawData Data { get; }

        public RawData Export()
        {
            return Data;
        }
    }
}
