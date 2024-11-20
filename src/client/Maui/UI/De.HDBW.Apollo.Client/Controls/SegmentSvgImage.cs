// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Assessment;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace De.HDBW.Apollo.Client.Controls
{
    public class SegmentSvgImage : SKCanvasView
    {
        public static BindableProperty SegmentsProperty = BindableProperty.Create(nameof(Segments), typeof(IList<ModuleScoreEntry>), typeof(SegmentSvgImage), null, propertyChanged: OnSegmentsChanged);

        public static BindableProperty IsShowingModulResultProperty = BindableProperty.Create(nameof(IsShowingModulResult), typeof(bool), typeof(SegmentSvgImage), false, propertyChanged: OnIsShowingModulChanged);

        public SegmentSvgImage()
        {
            Background = Colors.White;
        }

        public IList<ModuleScoreEntry>? Segments
        {
            get => (IList<ModuleScoreEntry>?)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }

        public bool IsShowingModulResult
        {
            get => (bool)GetValue(IsShowingModulResultProperty);
            set => SetValue(IsShowingModulResultProperty, value);
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            base.OnPaintSurface(args);

            try
            {
                CreateSVG(args);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured when reading the Svg Resource: " + ex.Message);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            InvalidateSurface();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            InvalidateSurface();
        }

        private static void OnSegmentsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var skCanvasView = bindable as SKCanvasView;
            skCanvasView?.InvalidateSurface();
        }

        private static void OnIsShowingModulChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var skCanvasView = bindable as SKCanvasView;
            skCanvasView?.InvalidateSurface();
        }

        private void CreateSVG(SKPaintSurfaceEventArgs args)
        {
            try
            {
                var canvas = args.Surface.Canvas;
                canvas.Clear();

                var segmentCount = Segments?.Count ?? 0;

                if (segmentCount == 0)
                {
                    return;
                }

                var svg = new Svg.Skia.SKSvg();
                var info = args.Info;

                // claculate center point
                var cp = new SKPoint(info.Width / 2f, info.Height / 2f);

                // the angle of each segment
                double angle = segmentCount > 0 ? (double)(2d * Math.PI) / (segmentCount * 2d) : 2d * Math.PI / 2;
                var offset = angle * (double)(segmentCount - 1);

                // the radius
                double rad = Math.Min(info.Width / 2d, info.Height / 2d);
                var radSection = rad / 5d;

                var backgroundLayer = new List<SKPoint>();
                var lineLayers = new Dictionary<int, List<SKPoint>>();
                var axesDrawing = new List<(SKPoint p0, SKPoint p1)>();

                var resultsDrawing = new List<List<SKPoint>>();

                using (var paint = new SKPaint() { IsAntialias = false, StrokeWidth = 10 })
                {
                    ModuleScoreEntry? currentSegment = null;
                    for (var i = 0; i < segmentCount * 2; i++)
                    {
                        var r = (angle * i) + offset;
                        if (i % 2 == 0)
                        {
                            float pRx = 0f;
                            float pRy = 0f;
                            if (resultsDrawing.Any())
                            {
                                pRx = (float)((currentSegment!.Result ?? 0d) * rad * Math.Sin(r)) + cp.X;
                                pRy = (float)((currentSegment!.Result ?? 0d) * rad * Math.Cos(r)) + cp.Y;
                                resultsDrawing.Last().Add(new SKPoint(pRx, pRy));
                                resultsDrawing.Last().Add(cp);
                            }

                            currentSegment = Segments![i / 2];
                            resultsDrawing.Add(new List<SKPoint>());
                            float px = (float)(rad * Math.Sin(r)) + cp.X;
                            float py = (float)(rad * Math.Cos(r)) + cp.Y;
                            pRx = (float)((currentSegment!.Result ?? 0d) * rad * Math.Sin(r)) + cp.X;
                            pRy = (float)((currentSegment!.Result ?? 0d) * rad * Math.Cos(r)) + cp.Y;
                            resultsDrawing.Last().Add(cp);
                            resultsDrawing.Last().Add(new SKPoint(pRx, pRy));
                            var p0 = new SKPoint(px, py);
                            axesDrawing.Add((p0, cp));
                        }
                        else
                        {
                            float pRx = (float)((currentSegment!.Result ?? 0d) * rad * Math.Sin(r)) + cp.X;
                            float pRy = (float)((currentSegment!.Result ?? 0d) * rad * Math.Cos(r)) + cp.Y;
                            resultsDrawing.Last().Add(new SKPoint(pRx, pRy));

                            float px = (float)(rad * Math.Sin(r)) + cp.X;
                            float py = (float)(rad * Math.Cos(r)) + cp.Y;

                            if (i == (segmentCount * 2) - 1)
                            {
                                pRx = (float)((currentSegment!.Result ?? 0d) * rad * Math.Sin(offset)) + cp.X;
                                pRy = (float)((currentSegment!.Result ?? 0d) * rad * Math.Cos(offset)) + cp.Y;
                                resultsDrawing.Last().Add(new SKPoint(pRx, pRy));
                                resultsDrawing.Last().Add(cp);
                            }
                        }

                        for (var z = 1; z < 6; z++)
                        {
                            float pAx = (float)((z * radSection) * Math.Sin(angle * i)) + cp.X;
                            float pAy = (float)((z * radSection) * Math.Cos(angle * i)) + cp.Y;
                            if (!lineLayers.ContainsKey(z))
                            {
                                lineLayers.Add(z, new List<SKPoint>());
                            }

                            lineLayers[z].Add(new SKPoint(pAx, pAy));
                        }
                    }

                    foreach (var lineLayer in lineLayers.Values)
                    {
                        lineLayer.Add(lineLayer[0]);
                    }

                    backgroundLayer = lineLayers.Values.Last().Select(x => x).ToList();

                    // White
                    var defaultColor = new SKColor(255, 255, 255);
                    var highlightColor = new SKColor(66, 10, 152);
                    var mediumHighlightColor = new SKColor(148, 107, 210);
                    var backgroundColor = new SKColor(233, 210, 177);

                    DrawBackground(paint, canvas, backgroundColor, backgroundLayer);
                    foreach (var lineLayer in lineLayers)
                    {
                        bool isSolid = lineLayer.Key != 3;
                        DrawLines(paint, canvas, isSolid ? defaultColor : highlightColor, isSolid, lineLayer.Value);
                    }

                    foreach (var result in resultsDrawing)
                    {
                        var index = resultsDrawing.IndexOf(result);
                        var color = highlightColor;

                        if (IsShowingModulResult && index != 0)
                        {
                            color = mediumHighlightColor;
                        }

                        DrawResult(paint, canvas, color, result);
                    }

                    canvas.Save();
                    var path = new SKPath();
                    foreach (var result in resultsDrawing)
                    {
                        path.MoveTo(result[0].X, result[0].Y);
                        foreach (var point in result)
                        {
                            path.LineTo(point.X, point.Y);
                        }
                    }

                    path.Close();
                    canvas.ClipPath(path, SKClipOperation.Intersect, true);
                    DrawLines(paint, canvas, defaultColor, false, lineLayers[3]);
                    canvas.Restore();

                    DrawAxes(paint, canvas, defaultColor, axesDrawing);

                    canvas.Flush();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured when reading the Svg Resource: " + ex.Message);
            }
        }

        private void DrawResult(SKPaint paint, SKCanvas canvas, SKColor color, List<SKPoint> points)
        {
            var path = new SKPath() { FillType = SKPathFillType.EvenOdd };
            path.MoveTo(points[0].X, points[0].Y);
            for (var i = 1; i < points.Count - 1; i++)
            {
                path.LineTo(points[i]);
            }

            path.Close();
            paint.Color = color;
            paint.Style = SKPaintStyle.StrokeAndFill;
            canvas.DrawPath(path, paint);
        }

        private void DrawAxes(SKPaint paint, SKCanvas canvas, SKColor color, List<(SKPoint p0, SKPoint p1)> axes)
        {
            paint.PathEffect = null;
            paint.Color = color;
            foreach (var axis in axes)
            {
                canvas.DrawLine(axis.p0, axis.p1, paint);
            }
        }

        private void DrawBackground(SKPaint paint, SKCanvas canvas, SKColor color, List<SKPoint> backgroundLayer)
        {
            var path = new SKPath() { FillType = SKPathFillType.EvenOdd };
            path.MoveTo(backgroundLayer[0].X, backgroundLayer[0].Y);
            foreach (var point in backgroundLayer)
            {
                path.LineTo(point.X, point.Y);
            }

            path.Close();
            paint.Color = color;
            paint.Style = SKPaintStyle.StrokeAndFill;
            canvas.DrawPath(path, paint);
        }

        private void DrawLines(SKPaint paint, SKCanvas canvas, SKColor color, bool isSolid, List<SKPoint> values)
        {
            paint.StrokeWidth = 5;
            paint.Color = color;
            if (!isSolid)
            {
                paint.PathEffect = SKPathEffect.CreateDash(new float[2] { 50f, 10f }, -25f);
            }
            else
            {
                paint.PathEffect = null;
            }

            canvas.DrawPoints(SKPointMode.Polygon, values.ToArray(), paint);
        }
    }
}
