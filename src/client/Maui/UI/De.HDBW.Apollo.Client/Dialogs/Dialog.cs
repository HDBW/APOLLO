// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class Dialog : ContentPage
    {
        public Dialog()
            : base()
        {
            Loaded += OnPageLoaded;
        }

        public async Task CloseAsync()
        {
            // Wait for the animation to complete
            await PoppingOutAsync();

            // Navigate away without the default animation
            await Navigation.PopModalAsync(animated: false);
        }

        private void PoppingIn()
        {
            // Measure the actual content size
            var contentSize = Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
            var contentHeight = contentSize.Request.Height;

            // Start by translating the content below / off screen
            var distance = ((Window?.Height ?? 0) / 2d) + (contentHeight / 2d);
            Content.TranslationY = distance;

            // Animate the translucent background, fading into view
            this.Animate(
                nameof(BackgroundColor),
                callback: v => BackgroundColor = Colors.Black.WithAlpha((float)v),
                start: 0d,
                end: 0.7d,
                rate: 32,
                length: 350,
                easing: Easing.CubicOut,
                finished: (v, k) =>
                    BackgroundColor = Colors.Black.WithAlpha(0.7f));

            // Also animate the content sliding up from below the screen
            this.Animate(
                nameof(Content),
                callback: v => Content.TranslationY = (int)(distance - v),
                start: 0,
                end: distance,
                length: 500,
                easing: Easing.CubicInOut,
                finished: (v, k) => Content.TranslationY = 0);
        }

        private Task PoppingOutAsync()
        {
            var done = new TaskCompletionSource();

            // Measure the content size so we know how much to translate
            var contentSize = Content.DesiredSize;
            var distance = ((Window?.Height ?? 0) / 2d) + (contentSize.Height / 2d);

            // Start fading out the background
            this.Animate(
                nameof(BackgroundColor),
                callback: v => BackgroundColor = Colors.Black.WithAlpha((float)v),
                start: 0.7d,
                end: 0d,
                rate: 32,
                length: 350,
                easing: Easing.CubicIn,
                finished: (v, k) => BackgroundColor = Colors.Black.WithAlpha(0.0f));

            // Start sliding the content down below the bottom of the screen
            this.Animate(
                nameof(Content),
                callback: v => Content.TranslationY = (int)(distance - v),
                start: distance,
                end: 0,
                length: 500,
                easing: Easing.CubicInOut,
                finished: (v, k) =>
                {
                    Content.TranslationY = distance;

                    // Important: Set our completion source to done!
                    done.TrySetResult();
                });

            // We return the task so we can wait for the animation to finish
            return done.Task;
        }

        private void OnPageLoaded(object? sender, EventArgs e)
        {
            // We only need this to fire once, so clean things up!
            Loaded -= OnPageLoaded;

            // Call the animation
            PoppingIn();
        }
    }
}
