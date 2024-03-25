// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;

namespace De.HDBW.Apollo.Client.Helper
{
    internal class WeakNotifyPropertyChangedProxy : WeakEventProxy<INotifyPropertyChanged, PropertyChangedEventHandler>
    {
        public WeakNotifyPropertyChangedProxy()
        {
        }

        public WeakNotifyPropertyChangedProxy(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            Subscribe(source, handler);
        }

        public override void Subscribe(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            if (TryGetSource(out var s))
            {
                s.PropertyChanged -= OnPropertyChanged;
            }

            source.PropertyChanged += OnPropertyChanged;

            base.Subscribe(source, handler);
        }

        public override void Unsubscribe()
        {
            if (TryGetSource(out var s))
            {
                s.PropertyChanged -= OnPropertyChanged;
            }

            base.Unsubscribe();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (TryGetHandler(out var handler))
            {
                handler(sender, e);
            }
            else
            {
                Unsubscribe();
            }
        }
    }
}
