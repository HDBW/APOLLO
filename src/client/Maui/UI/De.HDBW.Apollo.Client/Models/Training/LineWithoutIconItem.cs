// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class LineWithoutIconItem : LineItem
    {
        protected LineWithoutIconItem(string? icon, string text)
            : base(icon, text)
        {
        }

        public static new LineWithoutIconItem Import(string? icon, string text)
        {
            return new LineWithoutIconItem(icon, text);
        }
    }
}
