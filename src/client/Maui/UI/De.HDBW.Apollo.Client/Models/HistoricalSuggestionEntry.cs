using De.HDBW.Apollo.SharedContracts.Models;

namespace De.HDBW.Apollo.Client.Models
{
    public class HistoricalSuggestionEntry : SearchSuggestionEntry
    {
        private HistoricalSuggestionEntry(SearchHistory history)
            : base(history.Query!, true)
        {
            History = history;
        }

        public long Ticks
        {
            get
            {
                return History?.Ticks ?? 0;
            }
        }

        private SearchHistory History { get; set; }

        public static HistoricalSuggestionEntry Import(SearchHistory history)
        {
            return new HistoricalSuggestionEntry(history);
        }

        public SearchHistory Export()
        {
            return History;
        }

        public void Update(SearchHistory history)
        {
            History = history;
            OnPropertyChanged();
        }
    }
}
