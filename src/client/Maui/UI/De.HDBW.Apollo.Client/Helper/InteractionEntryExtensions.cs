// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Interactions;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class InteractionEntryExtensions
    {
        public static List<InteractionEntry> AsSortedList(this IEnumerable<InteractionEntry>? items)
        {
            return items?.OrderBy(x => x.Text)?.ToList() ?? new List<InteractionEntry>();
        }

        public static List<SelectInteractionEntry> AsSortedList(this IEnumerable<SelectInteractionEntry>? items)
        {
            return items?.OrderBy(x => x.Text)?.ToList() ?? new List<SelectInteractionEntry>();
        }
    }
}
