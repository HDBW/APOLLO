// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Svg.Skia;

namespace De.HDBW.Apollo.Client.Controls;

public class SvgImage : SKCanvasView
{
    public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(string), typeof(SvgImage), default(string), propertyChanged: OnPropertyChanged);

    private SKSvg? _svg;

    public SvgImage()
    {
        BackgroundColor = Colors.Transparent;
    }

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
    {
        base.OnPaintSurface(args);

        try
        {
            if (!string.IsNullOrEmpty(Source))
            {
                if (_svg == null)
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Source)))
                    {
                        if (stream == null)
                        {
                            throw new Exception("SVG Stream is NULL for Content");
                        }

                        stream.Position = 0;
                        CreateSVG(stream, args);
                    }
                }
                else
                {
                    CreateSVG(null, args);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occured when reading the Svg Resource: " + ex.Message);
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        InvalidateSurface();
    }

    private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var skCanvasView = bindable as SKCanvasView;
        var imageView = bindable as SvgImage;
        imageView?.InvalidateSvg();
        skCanvasView?.InvalidateSurface();
    }

    private void InvalidateSvg()
    {
        _svg?.Dispose();
        _svg = null;
    }

    private void CreateSVG(Stream? stream, SKPaintSurfaceEventArgs args)
    {
        try
        {
            var canvas = args.Surface.Canvas;
            canvas.Clear();

            if (stream != null)
            {
                InvalidateSvg();
                _svg = new SKSvg();
                _svg.Load(stream);
            }

            if (_svg == null || _svg.Model == null)
            {
                InvalidateSvg();
                return;
            }

            var info = args.Info;

            var bounds = _svg.Model.CullRect;
            if (!IsVisible || (info.Width == 0 && info.Height == 0) || bounds.IsEmpty)
            {
                return;
            }

            float xRatio = info.Width / bounds.Width;
            float yRatio = info.Height / bounds.Height;

            var ratio = Math.Min(xRatio, yRatio);

            canvas.Translate(info.Width / 2f, info.Height / 2f);
            canvas.Scale(ratio);
            canvas.Translate(-(bounds.Width / 2), -(bounds.Height / 2));
            canvas.DrawPicture(_svg.Picture);

            canvas.Flush();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occured when reading the Svg Resource: " + ex.Message);
        }
    }
}
