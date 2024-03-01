// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using MaybeNullWhenAttribute = System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute;

namespace De.HDBW.Apollo.Client.Helper
{
    internal abstract class WeakEventProxy<TSource, TEventHandler>
    where TSource : class
    where TEventHandler : Delegate
    {
        private WeakReference<TSource>? _source;
        private WeakReference<TEventHandler>? _handler;

        public bool TryGetSource([MaybeNullWhen(false)] out TSource source)
        {
            if (_source is not null && _source.TryGetTarget(out source))
            {
                return source is not null;
            }

            source = default;
            return false;
        }

        public bool TryGetHandler([MaybeNullWhen(false)] out TEventHandler handler)
        {
            if (_handler is not null && _handler.TryGetTarget(out handler))
            {
                return handler is not null;
            }

            handler = default;
            return false;
        }

        public virtual void Subscribe(TSource source, TEventHandler handler)
        {
            _source = new WeakReference<TSource>(source);
            _handler = new WeakReference<TEventHandler>(handler);
        }

        public virtual void Unsubscribe()
        {
            _source = null;
            _handler = null;
        }
    }
}
