using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public SearchHistory History { get; private set; }

        public static SearchSuggestionEntry Import(SearchHistory history)
        {
            return new HistoricalSuggestionEntry(history);
        }
    }
}
