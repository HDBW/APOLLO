// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class HeadlineTextEntry : TextEntry
    {
        protected HeadlineTextEntry(string? text)
            : base(text)
        {
        }

        public static new HeadlineTextEntry Import(string text)
        {
            return new HeadlineTextEntry(text);
        }
    }
}
