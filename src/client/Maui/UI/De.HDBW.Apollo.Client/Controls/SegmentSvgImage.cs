// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace De.HDBW.Apollo.Client.Controls
{
    public class SegmentSvgImage : SKCanvasView
    {
        public static BindableProperty SegmentsProperty = BindableProperty.Create(nameof(Segments), typeof(IList<TestResultEntry>), typeof(SegmentSvgImage), null, propertyChanged: OnSegmentsChanged);

        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SegmentSvgImage), 62d, propertyChanged: OnSegmentsChanged);

        public SegmentSvgImage()
        {
            BackgroundColor = Colors.AliceBlue;
        }

        public IList<TestResultEntry>? Segments
        {
            get => (IList<TestResultEntry>?)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
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
                var cp = new SKPoint(info.Width / 2f, info.Height / 2f);
                double angle = segmentCount > 0 ? (double)(2d * Math.PI) / (segmentCount * 2d) : 2d * Math.PI / 2;
                double rad = Math.Min(info.Width / 2d, info.Height / 2d);
                double radAvarage = rad * 0.3;

                var layer0Drawing = new List<(SKPoint p0, SKPoint p1)>();
                var labelDrawing = new List<(SKPoint p0, string)>();
                var layer2Drawing = new List<SKPoint>();
                var layer3Drawing = new List<SKPoint>();
                var figures = new List<List<SKPoint>>();
                var testSizes = new Dictionary<TestResultEntry, float>();
                using (var paint = new SKPaint() { IsAntialias = false, StrokeWidth = 10 })
                {
                    paint.TextSize = (float)FontSize;
                    foreach (var segment in Segments!)
                    {
                        testSizes.Add(segment, paint.MeasureText(segment.Text));
                    }

                    TestResultEntry? currentSegment = null;
                    for (var i = 0; i < segmentCount * 2; i++)
                    {
                        if (i % 2 == 0)
                        {
                            float pRx = 0f;
                            float pRy = 0f;
                            if (figures.Any())
                            {
                                pRx = (float)(currentSegment!.Score * rad * Math.Sin(angle * i)) + cp.X;
                                pRy = (float)(currentSegment!.Score * rad * Math.Cos(angle * i)) + cp.Y;
                                figures.Last().Add(new SKPoint(pRx, pRy));
                                figures.Last().Add(cp);
                            }

                            currentSegment = Segments![i / 2];
                            figures.Add(new List<SKPoint>());
                            float px = (float)(rad * Math.Sin(angle * i)) + cp.X;
                            float py = (float)(rad * Math.Cos(angle * i)) + cp.Y;
                            pRx = (float)(currentSegment.Score * rad * Math.Sin(angle * i)) + cp.X;
                            pRy = (float)(currentSegment.Score * rad * Math.Cos(angle * i)) + cp.Y;
                            figures.Last().Add(cp);
                            figures.Last().Add(new SKPoint(pRx, pRy));
                            var p0 = new SKPoint(px, py);
                            layer0Drawing.Add((p0, cp));
                        }
                        else
                        {
                            float pRx = (float)(currentSegment!.Score * rad * Math.Sin(angle * i)) + cp.X;
                            float pRy = (float)(currentSegment!.Score * rad * Math.Cos(angle * i)) + cp.Y;
                            figures.Last().Add(new SKPoint(pRx, pRy));

                            float px = (float)(rad * Math.Sin(angle * i)) + cp.X;
                            float py = (float)(rad * Math.Cos(angle * i)) + cp.Y;

                            var x = px - testSizes[currentSegment];
                            if (x < 0)
                            {
                                x = px;
                            }

                            var p0 = new SKPoint(x, py);

                            labelDrawing.Add((p0, currentSegment.Text));
                            if (i == (segmentCount * 2) - 1)
                            {
                                pRx = (float)(currentSegment.Score * rad * Math.Sin(0)) + cp.X;
                                pRy = (float)(currentSegment.Score * rad * Math.Cos(0)) + cp.Y;
                                figures.Last().Add(new SKPoint(pRx, pRy));
                                figures.Last().Add(cp);
                            }
                        }

                        float pMx = (float)(rad * Math.Sin(angle * i)) + cp.X;
                        float pMy = (float)(rad * Math.Cos(angle * i)) + cp.Y;
                        layer2Drawing.Add(new SKPoint(pMx, pMy));

                        float pAx = (float)(radAvarage * Math.Sin(angle * i)) + cp.X;
                        float pAy = (float)(radAvarage * Math.Cos(angle * i)) + cp.Y;
                        layer3Drawing.Add(new SKPoint(pAx, pAy));
                    }

                    layer2Drawing.Add(layer2Drawing[0]);
                    layer3Drawing.Add(layer3Drawing[0]);

                    paint.Color = new SKColor(233, 210, 177);
                    foreach (var line in layer0Drawing)
                    {
                        canvas.DrawLine(line.p0, line.p1, paint);
                    }

                    // paint.Color = new SKColor(255, 0, 255);
                    // foreach (var line in layer1Drawing)
                    // {
                    //    canvas.DrawLine(line.p0, line.p1, paint);
                    // }
                    paint.StrokeWidth = 5;
                    paint.Color = new SKColor(233, 210, 177);
                    paint.PathEffect = SKPathEffect.CreateDash(new float[2] { 50f, 10f }, -25f);
                    canvas.DrawPoints(SKPointMode.Polygon, layer2Drawing.ToArray(), paint);

                    foreach (var figure in figures)
                    {
                        var path = new SKPath() { FillType = SKPathFillType.EvenOdd };
                        path.MoveTo(figure[0].X, figure[0].Y);
                        foreach (var point in figure)
                        {
                            path.LineTo(point.X, point.Y);
                        }

                        path.Close();
                        paint.Color = new SKColor(66, 10, 152, 50);
                        paint.Style = SKPaintStyle.StrokeAndFill;
                        canvas.DrawPath(path, paint);
                    }

                    foreach (var label in labelDrawing)
                    {
                        paint.Color = new SKColor(0, 0, 0);
                        canvas.DrawText(label.Item2, label.p0, paint);
                    }

                    paint.StrokeWidth = 5;
                    paint.Color = new SKColor(66, 10, 152);
                    paint.PathEffect = SKPathEffect.CreateDash(new float[2] { 50f, 10f }, -25f);
                    canvas.DrawPoints(SKPointMode.Polygon, layer3Drawing.ToArray(), paint);

                    canvas.Flush();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured when reading the Svg Resource: " + ex.Message);
            }
        }
    }
}
