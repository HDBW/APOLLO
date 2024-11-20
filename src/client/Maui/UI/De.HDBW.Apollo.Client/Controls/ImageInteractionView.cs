// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.Controls
{
    public partial class ImageInteractionView : GraphicsView, IDisposable
    {
        public static readonly BindableProperty InteractionsProperty = BindableProperty.Create("Interactions", typeof(IEnumerable<InteractionShape>), typeof(ImageInteractionView), new List<InteractionShape>(), BindingMode.OneWay, null, HandleInteractionsChanged);

        public static readonly BindableProperty InteractionSizeProperty = BindableProperty.Create("InteractionSize", typeof(double), typeof(ImageInteractionView), 30d, BindingMode.OneWay);

        public static readonly BindableProperty InteractionBorderBrushProperty = BindableProperty.Create("InteractionBorderBrush", typeof(SolidColorBrush), typeof(ImageInteractionView), Brush.Red, BindingMode.OneWay, null, HandleBrushChanged);

        public static readonly BindableProperty InteractionBackgroundBrushProperty = BindableProperty.Create("InteractionBackgroundBrush", typeof(SolidColorBrush), typeof(ImageInteractionView), Brush.White, BindingMode.OneWay, null, HandleBrushChanged);

        public static readonly BindableProperty InteractionSelectedBorderBrushProperty = BindableProperty.Create("InteractionSelectedBorderBrush", typeof(SolidColorBrush), typeof(ImageInteractionView), Brush.Blue, BindingMode.OneWay, null, HandleBrushChanged);

        public static readonly BindableProperty InteractionSelectedBackgroundBrushProperty = BindableProperty.Create("InteractionSelectedBackgroundBrush", typeof(SolidColorBrush), typeof(ImageInteractionView), Brush.Blue, BindingMode.OneWay, null, HandleBrushChanged);

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(string), typeof(ImageInteractionView), string.Empty, BindingMode.OneWay, null, HandleImageSourceChanged);

        private ImageDrawable? _drawable;
        private bool _disposed;
        private PointF _touch;

        public ImageInteractionView()
        {
            SubscribeEvents();
        }

        ~ImageInteractionView()
        {
            Dispose(false);
        }

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public SolidColorBrush InteractionBorderBrush
        {
            get => (SolidColorBrush)GetValue(InteractionBorderBrushProperty);
            set => SetValue(InteractionBorderBrushProperty, value);
        }

        public SolidColorBrush InteractionBackgroundBrush
        {
            get => (SolidColorBrush)GetValue(InteractionBackgroundBrushProperty);
            set => SetValue(InteractionBackgroundBrushProperty, value);
        }

        public SolidColorBrush InteractionSelectedBorderBrush
        {
            get => (SolidColorBrush)GetValue(InteractionSelectedBorderBrushProperty);
            set => SetValue(InteractionSelectedBorderBrushProperty, value);
        }

        public SolidColorBrush InteractionSelectedBackgroundBrush
        {
            get => (SolidColorBrush)GetValue(InteractionSelectedBackgroundBrushProperty);
            set => SetValue(InteractionSelectedBackgroundBrushProperty, value);
        }

        public double InteractionSize
        {
            get => (double)GetValue(InteractionSizeProperty);
            set => SetValue(InteractionSizeProperty, value);
        }

        public IEnumerable<InteractionShape> Interactions
        {
            get => (IEnumerable<InteractionShape>)GetValue(InteractionsProperty);
            set => SetValue(InteractionsProperty, value);
        }

        public RectF ImageRect
        {
            get { return _drawable?.ImageRect ?? RectF.Zero; }
        }

        public float ImageScale
        {
            get { return _drawable?.ImageScale ?? 1f; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            UnsubscribeEvents();
            if (disposing)
            {
                Drawable = null;
            }

            _drawable = null;
            _disposed = true;
        }

        private static void HandleInteractionsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ImageInteractionView;
            if (control?.OnInteractionsChanged() ?? false)
            {
                control!.Invalidate();
            }
        }

        private static void HandleBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ImageInteractionView;
            if (control?.OnBrushChanged() ?? false)
            {
                control!.Invalidate();
            }
        }

        private static void HandleImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ImageInteractionView;
            if (control?.OnImageSourceChanged() ?? false)
            {
                var controlRef = new WeakReference<ImageInteractionView>(control);
                var currentValue = control.ImageSource;
                if (string.IsNullOrWhiteSpace(currentValue))
                {
                    control.Invalidate();
                    return;
                }

                if (!Uri.IsWellFormedUriString(currentValue, UriKind.Absolute))
                {
                    control.Invalidate();
                    return;
                }

                var tasks = new List<Task>()
                {
                    ImageHelper.EnsureOriginalSizesAreLoaded(),
                    DownloadFileAsync(currentValue),
                };

                Task.Run(() => Task.WhenAll(tasks)).ContinueWith((x) =>
                {
                    if (controlRef.TryGetTarget(out ImageInteractionView? control))
                    {
                        control.Dispatcher.Dispatch(() => { control.Invalidate(); });
                    }
                });
            }
        }

        private static async Task DownloadFileAsync(string file)
        {
            string cacheDir = FileSystem.Current.CacheDirectory;
            var path = Path.Combine(cacheDir, Path.GetFileName(file));

            var info = new FileInfo(path);
            if (info.Exists && info.Length > 0)
            {
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    using (var tempFile = new TempFile())
                    {
                        using (var response = await client.GetAsync(file))
                        {
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                await tempFile.SaveAsync(stream);
                                tempFile.Move(path, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool OnImageSourceChanged()
        {
            if (string.IsNullOrWhiteSpace(ImageSource))
            {
                _drawable = null;
                Drawable = null;
                return true;
            }

            if (_drawable == null)
            {
                _drawable = new ImageDrawable();
                Drawable = _drawable;
                OnInteractionsChanged();
                OnBrushChanged();
            }

            _drawable.SetImageSource(ImageSource);
            return true;
        }

        private bool OnBrushChanged()
        {
            if (_drawable == null)
            {
                return false;
            }

            _drawable.InteractionBackgroundColor = InteractionBackgroundBrush?.Color ?? Colors.Transparent;
            _drawable.InteractionBorderColor = InteractionBorderBrush?.Color ?? Colors.Transparent;
            _drawable.InteractionSelectedBackgroundColor = InteractionSelectedBackgroundBrush?.Color ?? Colors.Transparent;
            _drawable.InteractionSelectedBorderColor = InteractionSelectedBorderBrush?.Color ?? Colors.Transparent;
            return true;
        }

        private bool OnInteractionsChanged()
        {
            if (_drawable == null)
            {
                return false;
            }

            var shapes = new List<InteractionShape>();
            if (Interactions != null)
            {
                shapes.AddRange(Interactions);
            }

            _drawable.SetShapes(shapes);
            return true;
        }

        private void SubscribeEvents()
        {
            UnsubscribeEvents();
            StartInteraction += OnTouchDown;
            EndInteraction += OnTouchUp;
        }

        private void UnsubscribeEvents()
        {
            StartInteraction -= OnTouchDown;
            EndInteraction -= OnTouchUp;
        }

        private void OnTouchDown(object? sender, TouchEventArgs e)
        {
            Invalidate();
            if (e.Touches.Count() != 1 || _drawable == null)
            {
                _touch = PointF.Zero;
                return;
            }

            _touch = e.Touches[0];
        }

        private void OnTouchUp(object? sender, TouchEventArgs e)
        {
            if (_touch == PointF.Zero || e.Touches.Count() != 1 || !e.IsInsideBounds || _drawable == null)
            {
                _touch = PointF.Zero;
                return;
            }

            if (Math.Abs(_touch.Distance(e.Touches[0])) > 10)
            {
                _touch = PointF.Zero;
                return;
            }

            _touch = PointF.Zero;
            HandleClicked(e.Touches[0]);
        }

        private void HandleClicked(PointF touchPoint)
        {
            var touchPointInImage = TranslatePointToImage(touchPoint);
            if (!touchPointInImage.HasValue)
            {
                return;
            }

            touchPoint = touchPointInImage.Value;
            var touchedInteractions = new List<InteractionShape>();
            foreach (var interaction in Interactions)
            {
                switch (interaction)
                {
                    case InteractionCircle circle:
                        if (Intersect(touchPoint, circle))
                        {
                            touchedInteractions.Add(circle);
                        }

                        break;
                    case InteractionRectangle rectangle:
                        if (Intersect(touchPoint, rectangle))
                        {
                            touchedInteractions.Add(rectangle);
                        }

                        break;
                }
            }

            var closestInteraction = touchedInteractions.OrderBy(x => Distance(x, touchPoint)).FirstOrDefault();
            if (closestInteraction == null)
            {
                return;
            }

            closestInteraction.IsSelected = !closestInteraction.IsSelected;
            OnInteractionsChanged();
            Invalidate();
        }

        private float Distance(InteractionShape interaction, PointF touchPoint)
        {
            PointF centerPoint = PointF.Zero;
            switch (interaction)
            {
                case InteractionCircle circle:
                    centerPoint = new PointF(Convert.ToSingle(circle.Location.X) * ImageScale, Convert.ToSingle(circle.Location.Y) * ImageScale);
                    return Math.Abs(centerPoint.Distance(touchPoint));
                case InteractionRectangle rectangle:
                    centerPoint = new PointF(Convert.ToSingle(rectangle.Rectangle.Center.X) * ImageScale, Convert.ToSingle(rectangle.Rectangle.Center.Y) * ImageScale);
                    return Math.Abs(centerPoint.Distance(touchPoint));
                default:
                    return float.MaxValue;
            }
        }

        private bool Intersect(PointF touchPoint, InteractionCircle circle)
        {
            // If distance between centers of the circles is grerater than sum of its radiuses, the circles do not touch.
            var interactionPoint = new PointF(Convert.ToSingle(circle.Location.X) * ImageScale, Convert.ToSingle(circle.Location.Y) * ImageScale);
            var distance = Math.Abs(interactionPoint.Distance(touchPoint));

            return distance <= ((InteractionSize / 2) + (circle.Radius * ImageScale));
        }

        private bool Intersect(PointF touchPoint, InteractionRectangle rectangle)
        {
            var interactionPoint = new RectF(
                Convert.ToSingle(rectangle.Rectangle.X) * ImageScale,
                Convert.ToSingle(rectangle.Rectangle.Y) * ImageScale,
                Convert.ToSingle(rectangle.Rectangle.Width) * ImageScale,
                Convert.ToSingle(rectangle.Rectangle.Height) * ImageScale);

            // Check if the center of the circle is inside the rectangle
            if (interactionPoint.Contains(touchPoint))
            {
                return true;
            }

            // Check if any corner of the rectangle is inside the circle
            if (PointIntersectCircle(touchPoint, InteractionSize / 2, new PointF(interactionPoint.Left, interactionPoint.Top)) ||
                PointIntersectCircle(touchPoint, InteractionSize / 2, new PointF(interactionPoint.Right, interactionPoint.Top)) ||
                PointIntersectCircle(touchPoint, InteractionSize / 2, new PointF(interactionPoint.Right, interactionPoint.Bottom)) ||
                PointIntersectCircle(touchPoint, InteractionSize / 2, new PointF(interactionPoint.Left, interactionPoint.Bottom)))
            {
                return true;
            }

            return false;
        }

        private bool PointIntersectCircle(PointF centerPoint, double radius, PointF point)
        {
            var distance = Math.Abs(point.Distance(centerPoint));
            return distance <= radius;
        }

        private PointF? TranslatePointToImage(PointF point)
        {
            if (ImageRect == RectF.Zero || !ImageRect.Contains(point))
            {
                return null;
            }

            return new PointF((point.X - ImageRect.X) / _drawable?.ShapeScale ?? 1, (point.Y - ImageRect.Y) / _drawable?.ShapeScale ?? 1);
        }
    }
}
