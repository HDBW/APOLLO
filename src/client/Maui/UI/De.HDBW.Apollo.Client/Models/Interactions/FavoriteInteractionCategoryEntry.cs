// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class FavoriteInteractionCategoryEntry : InteractionCategoryEntry
    {
        private FavoriteInteractionCategoryEntry(string? headLine, string? sublineLine, List<InteractionEntry> interactions, List<InteractionEntry> filters, object? data, Func<InteractionCategoryEntry, Task> navigateHandler, Func<InteractionCategoryEntry, bool> canNavigateHandle)
            : base(headLine, sublineLine, interactions, filters, data, navigateHandler, canNavigateHandle)
        {
        }

        public static new InteractionCategoryEntry Import(string? headLine, string? sublineLine, List<InteractionEntry> interactions, List<InteractionEntry> filters, object? data, Func<InteractionCategoryEntry, Task> navigateHandler, Func<InteractionCategoryEntry, bool> canNavigateHandle)
        {
            return new FavoriteInteractionCategoryEntry(headLine, sublineLine, interactions, filters, data, navigateHandler, canNavigateHandle);
        }

        public void AddFilter(InteractionEntry filter)
        {
            Filters.Add(filter);
            OnPropertyChanged(nameof(HasFilters));
        }

        public void RemoveFilter(InteractionEntry filter)
        {
            Filters.Remove(filter);
            OnPropertyChanged(nameof(HasFilters));
        }

        public void AddInteraction(InteractionEntry interaction)
        {
            Interactions.Add(interaction);
            OnPropertyChanged(nameof(HasInteractions));
            OnPropertyChanged(nameof(HasNoInteractions));
        }

        public void RemoveInteraction(InteractionEntry interaction)
        {
            Interactions.Remove(interaction);
            OnPropertyChanged(nameof(HasInteractions));
            OnPropertyChanged(nameof(HasNoInteractions));
        }
    }
}
