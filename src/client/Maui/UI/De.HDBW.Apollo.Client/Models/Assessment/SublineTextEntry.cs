// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SublineTextEntry : TextEntry
    {
        protected SublineTextEntry(string? text)
            : base(text)
        {
        }

        public static new SublineTextEntry Import(string text)
        {
            return new SublineTextEntry(text);
        }
    }
}
