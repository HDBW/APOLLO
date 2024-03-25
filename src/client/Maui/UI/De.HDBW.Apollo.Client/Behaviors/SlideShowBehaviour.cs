// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Behaviors
{
    public class SlideShowBehaviour : Behavior<CarouselView>
    {
        private CancellationTokenSource? _cts;

        protected override void OnAttachedTo(CarouselView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Loaded += HandleLoaded;
            bindable.Unloaded += HandleUnloade;
        }

        protected override void OnDetachingFrom(CarouselView bindable)
        {
            bindable.Loaded -= HandleLoaded;
            bindable.Unloaded -= HandleUnloade;
            base.OnDetachingFrom(bindable);
            _cts?.Dispose();
            _cts = null;
        }

        private void HandleUnloade(object? sender, EventArgs e)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void HandleLoaded(object? sender, EventArgs e)
        {
            var bindable = sender as CarouselView;
            if (bindable == null)
            {
                return;
            }

            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            StartSlideShow(bindable, _cts.Token);
        }

        private async void StartSlideShow(CarouselView view, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && view != null)
                {
                    await Task.Delay(10000);
                    if (view?.IsDragging ?? true)
                    {
                        continue;
                    }

                    if (view.ItemsSource == null || view.Position < 0)
                    {
                        continue;
                    }

                    var next = view.Position + 1;
                    if (next == view.ItemsSource?.OfType<object>().Count())
                    {
                        next = 0;
                    }

                    view.Position = next;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
