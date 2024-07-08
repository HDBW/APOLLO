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

        public float ImageScale { get; set; } = 1f;

        public float ShapeScale { get; set; } = 1f;

        public void SetShapes(List<InteractionShape> shapes)
        {
            _shapes = shapes;
        }

        public void SetImageSource(string imageSource)
        {
            _imageSource = imageSource;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (string.IsNullOrWhiteSpace(_imageSource))
            {
                return;
            }

            string cacheDir = FileSystem.Current.CacheDirectory;
            var path = Path.Combine(cacheDir, Path.GetFileName(_imageSource));
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    var imageFormat = ImageFormatHelper.RetrieveImageFormatFromFileExtension(path);
                    var image = PlatformImage.FromStream(stream, imageFormat);
                    var originalWidth = 1300; // todo: read from EXIF
                    var originalHeight = 1300; // todo: read from EXIF
                    var xScale = dirtyRect.Width / image.Width;
                    var yScale = dirtyRect.Height / image.Height;
                    ImageScale = Math.Min(xScale, yScale);
                    ImageScale = Math.Min(ImageScale, 1);
                    ShapeScale = Math.Min(dirtyRect.Width / originalWidth, dirtyRect.Height / originalHeight);
                    var size = new Size(image.Width * ImageScale, image.Height * ImageScale);
                    var point = new PointF((dirtyRect.Width - Convert.ToSingle(size.Width)) / 2f, (dirtyRect.Height - Convert.ToSingle(size.Height)) / 2f);
                    ImageRect = new RectF(point, size);
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
                                var pt = new PointF(ImageRect.X + (Convert.ToSingle(circle.Location.X) * ShapeScale), ImageRect.Y + (Convert.ToSingle(circle.Location.Y) * ShapeScale));
                                borderColor = circle.IsSelected ? InteractionSelectedBorderColor : InteractionBorderColor;
                                DrawSelectionIndicator(canvas, pt, Convert.ToSingle(circle.Radius * ShapeScale), borderColor, InteractionBackgroundColor);
                                if (circle.IsSelected)
                                {
                                    DrawSelectionIndicator(canvas, pt, Convert.ToSingle(circle.Radius * ShapeScale) / 2, InteractionSelectedBorderColor, InteractionSelectedBackgroundColor);
                                }

                                break;

                            case InteractionRectangle rect:
                                var rectangle = new RectF(
                                    ImageRect.X + (Convert.ToSingle(rect.Rectangle.X) * ShapeScale),
                                    ImageRect.Y + (Convert.ToSingle(rect.Rectangle.Y) * ShapeScale),
                                    Convert.ToSingle(rect.Rectangle.Width) * ShapeScale,
                                    Convert.ToSingle(rect.Rectangle.Height) * ShapeScale);
                                borderColor = rect.IsSelected ? InteractionSelectedBorderColor : InteractionBorderColor;
                                DrawSelectionIndicator(canvas, rectangle, borderColor, InteractionBackgroundColor);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DrawSelectionIndicator(ICanvas canvas, PointF pt, float radius, Color borderColor, Color fillColor)
        {
            canvas.StrokeColor = borderColor;
            canvas.FillColor = fillColor;
            canvas.StrokeSize = 2;
            canvas.FillCircle(pt.X, pt.Y, radius);
            canvas.DrawCircle(pt.X, pt.Y, radius);
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
