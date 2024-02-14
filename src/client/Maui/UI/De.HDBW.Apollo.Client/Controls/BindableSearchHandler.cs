// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Platforms;

namespace De.HDBW.Apollo.Client.Controls
{
    public class BindableSearchHandler : SearchHandler
    {
        public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create("SearchCommand", typeof(ICommand), typeof(BindableSearchHandler), null, BindingMode.OneWay);
        public static readonly BindableProperty SuggestionsProperty = BindableProperty.Create("Suggestions", typeof(IEnumerable<object>), typeof(BindableSearchHandler), null, BindingMode.OneWay, null, HandleSuggestionsChanged);
        public static readonly BindableProperty RecentProperty = BindableProperty.Create("Recent", typeof(IEnumerable<object>), typeof(BindableSearchHandler), null, BindingMode.OneWay);

        public IEnumerable<object> Suggestions
        {
            get => (IEnumerable<object>)GetValue(SuggestionsProperty);
            set => SetValue(SuggestionsProperty, value);
        }

        public IEnumerable<object> Recent
        {
            get => (IEnumerable<object>)GetValue(RecentProperty);
            set => SetValue(RecentProperty, value);
        }

        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
                return;
            }

            ItemsSource = new ObservableCollection<object>(Recent ?? Array.Empty<object>());
            SearchBoxVisibility = SearchBoxVisibility.Expanded;
            var viewModel = BindingContext as ILoadSuggestionsProvider;
            viewModel?.StartLoadSuggestions(newValue);
        }

        protected override void OnQueryConfirmed()
        {
            base.OnQueryConfirmed();
            Unfocus();
            if (OperatingSystem.IsAndroid())
            {
                while (IsFocused)
                {
                    Unfocus();
                }
            }

            KeyboardHelper.HideKeyboard(Shell.Current?.Handler?.PlatformView);

            SearchCommand?.Execute(Query);
            // SearchBoxVisibility = SearchBoxVisibility.Hidden;
            // await Task.Delay(200);
            // SearchBoxVisibility = SearchBoxVisibility.Expanded;
        }

        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            Unfocus();
            if (OperatingSystem.IsAndroid())
            {
                while (IsFocused)
                {
                    Unfocus();
                }
            }

            KeyboardHelper.HideKeyboard(Shell.Current?.Handler?.PlatformView);
            var entry = item as SearchSuggestionEntry;
            SearchCommand?.Execute(entry);

            // SearchBoxVisibility = SearchBoxVisibility.Hidden;
            // await Task.Delay(200);
            // SearchBoxVisibility = SearchBoxVisibility.Expanded;
        }

        private static void HandleSuggestionsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as BindableSearchHandler;
            if (control == null || newValue == null)
            {
                return;
            }

            var result = newValue as IEnumerable<object> ?? Array.Empty<object>();
            result = result.Union(control.Recent ?? Array.Empty<object>());
            control.ItemsSource = new ObservableCollection<object>(result);
            control.SearchBoxVisibility = SearchBoxVisibility.Expanded;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
#if IOS
            // see https://github.com/dotnet/maui/issues/14442
            if (nameof(QueryIcon) == propertyName && QueryIcon != null)
            {
                if (QueryIcon.Parent == null)
                {
                    QueryIcon.Parent = Application.Current?.MainPage;
                }
            }
#endif
            base.OnPropertyChanged(propertyName);
        }
    }
}
