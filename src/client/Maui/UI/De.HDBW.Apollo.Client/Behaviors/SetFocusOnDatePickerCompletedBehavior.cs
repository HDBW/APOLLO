// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Handlers;

namespace De.HDBW.Apollo.Client.Behaviors
{
    public static class SetFocusOnDatePickerCompletedBehavior
    {
        public static readonly BindableProperty NextElementProperty = BindableProperty.CreateAttached("NextElement", typeof(VisualElement), typeof(SetFocusOnEntryCompletedBehavior), null, BindingMode.OneWay, null, OnNextElementChanged);

        public static VisualElement? GetNextElement(BindableObject view)
        {
            return (VisualElement?)view.GetValue(NextElementProperty);
        }

        public static void SetNextElement(BindableObject view, VisualElement value)
        {
            view.SetValue(NextElementProperty, value);
        }

        private static void OnNextElementChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var entry = bindable as Entry;
            if (entry == null || newValue == null)
            {
                return;
            }

            SubscibeEvents(entry);
        }

        private static void SubscibeEvents(Entry entry)
        {
            WeakReference<Entry> weakEntry = new WeakReference<Entry>(entry);
            entry.Completed += CompletedHandler;
            void CompletedHandler(object? sender, EventArgs e)
            {
                if (weakEntry.TryGetTarget(out Entry? target) && target != null)
                {
                    var nextElement = GetNextElement(target);

                    switch (nextElement)
                    {
                        case DatePicker datePicker:
                            if (datePicker == null)
                            {
                                return;
                            }

                            var handler = datePicker.Handler as IDatePickerHandler;
                            if (handler == null)
                            {
                                return;
                            }
#if ANDROID
                            handler.PlatformView.PerformClick();
#elif IOS
                            handler.PlatformView.BecomeFirstResponder();
#endif
                            datePicker.Focus();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
