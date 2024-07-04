// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models.Assessment;
using Microsoft.Maui.Graphics.Platform;

namespace De.HDBW.Apollo.Client.Controls
{
    public class ImageDrawable : IDrawable
    {
        private IEnumerable<InteractionShape>? _shapes;
        private string? _imageSource;

        public Color? InteractionBorderColor { get; set; }

        public Color? InteractionBackgroundColor { get; set; }

        public Color? InteractionSelectedBorderColor { get; set; }

        public Color? InteractionSelectedBackgroundColor { get; set; }

        public RectF ImageRect { get; set; } = RectF.Zero;

        public float Scale { get; set; } = 1f;

        public void SetShapes(List<InteractionShape> shapes)
        {
            _shapes = shapes;
        }

        public void SetImageSource(string imageSource)
        {
            _imageSource = imageSource;
        }

        public async void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (string.IsNullOrWhiteSpace(_imageSource))
            {
                return;
            }

            string cacheDir = FileSystem.Current.CacheDirectory;
            var path = Path.Combine(cacheDir, Path.GetFileName(_imageSource));
            if (!File.Exists(path))
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        using (var tempFile = new TempFile())
                        {
                            using (var stream = await client.GetStreamAsync(_imageSource))
                            {
                                await tempFile.SaveAsync(stream);
                                tempFile.Move(path, true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    var image = PlatformImage.FromStream(stream, ImageFormat.Jpeg);
                    var xScale = dirtyRect.Width / image.Width;
                    var yScale = dirtyRect.Height / image.Height;
                    Scale = Math.Min(xScale, yScale);
                    Scale = Math.Min(Scale, 1);
                    var size = new Size(image.Width * Scale, image.Height * Scale);
                    var point = new PointF((dirtyRect.Width - Convert.ToSingle(size.Width)) / 2f, (dirtyRect.Height - Convert.ToSingle(size.Height)) / 2f);
                    ImageRect = new RectF(point, size);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            if (_shapes != null &&
                InteractionBorderColor != null &&
                InteractionBackgroundColor != null &&
                InteractionSelectedBorderColor != null &&
                InteractionSelectedBackgroundColor != null)
            {
                foreach (var shape in _shapes)
                {
                    Color? borderColor = null;
                    switch (shape)
                    {
                        case InteractionCircle circle:
                            var pt = new PointF(ImageRect.X + (Convert.ToSingle(circle.Location.X) * Scale), ImageRect.Y + (Convert.ToSingle(circle.Location.Y) * Scale));
                            borderColor = circle.IsSelected ? InteractionSelectedBorderColor : InteractionBorderColor;
                            DrawSelectionIndicator(canvas, pt, Convert.ToSingle(circle.Radius), borderColor, InteractionBackgroundColor);
                            if (circle.IsSelected)
                            {
                                DrawSelectionIndicator(canvas, pt, Convert.ToSingle(circle.Radius) / 2, InteractionSelectedBorderColor, InteractionSelectedBackgroundColor);
                            }

                            break;

                        case InteractionRectangle rect:
                            var rectangle = new RectF(
                                ImageRect.X + (Convert.ToSingle(rect.Rectangle.X) * Scale),
                                ImageRect.Y + (Convert.ToSingle(rect.Rectangle.Y) * Scale),
                                Convert.ToSingle(rect.Rectangle.Width) * Scale,
                                Convert.ToSingle(rect.Rectangle.Height) * Scale);
                            borderColor = rect.IsSelected ? InteractionSelectedBorderColor : InteractionBorderColor;
                            DrawSelectionIndicator(canvas, rectangle, borderColor, InteractionBackgroundColor);
                            break;
                    }
                }
            }
        }

        private void DrawSelectionIndicator(ICanvas canvas, PointF pt, float radius, Color borderColor, Color fillColor)
        {
            canvas.StrokeColor = borderColor;
            canvas.FillColor = fillColor;
            canvas.StrokeSize = 2;
            canvas.FillCircle(pt.X, pt.Y, radius * Scale);
            canvas.DrawCircle(pt.X, pt.Y, radius * Scale);
        }

        private void DrawSelectionIndicator(ICanvas canvas, RectF rect, Color borderColor, Color fillColor)
        {
            canvas.StrokeColor = borderColor;
            canvas.FillColor = fillColor;
            canvas.StrokeSize = 2;
            canvas.FillRectangle(rect);
            canvas.DrawRectangle(rect);
        }
    }
}
