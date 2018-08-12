using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

namespace MyTripCountdown.Controls
{
    public class CircleCountdown : SKCanvasView
    {
        public static readonly BindableProperty StrokeWidthProperty =
            BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(CircleCountdown), 10f, propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty ProgressProperty =
            BindableProperty.Create(nameof(Progress), typeof(float), typeof(CircleCountdown), 0f, propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty ProgressStartColorProperty =
            BindableProperty.Create(nameof(ProgressStartColor), typeof(Color), typeof(CircleCountdown), Color.Blue, propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty ProgressEndColorProperty =
            BindableProperty.Create(nameof(ProgressEndColor), typeof(Color), typeof(CircleCountdown), Color.Red, propertyChanged: OnPropertyChanged);

        private const float StartAngle = -90;
        private const float SweepAngle = 360;

        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        public float Progress
        {
            get { return (float)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        public Color ProgressStartColor
        {
            get { return (Color)GetValue(ProgressStartColorProperty); }
            set { SetValue(ProgressStartColorProperty, value); }
        }

        public Color ProgressEndColor
        {
            get { return (Color)GetValue(ProgressEndColorProperty); }
            set { SetValue(ProgressEndColorProperty, value); }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            int size = Math.Min(info.Width, info.Height);
            int max = Math.Max(info.Width, info.Height);

            canvas.Translate((max - size) / 2, 0);

            canvas.Clear();
            canvas.Save();
            canvas.RotateDegrees(0, size / 2, size / 2);
            DrawProgressCircle(info, canvas);

            canvas.Restore();
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var circleProgress = bindable as CircleCountdown;
            circleProgress?.InvalidateSurface();
        }

        private void DrawProgressCircle(SKImageInfo info, SKCanvas canvas)
        {
            float progressAngle = SweepAngle  * Progress;
            int size = Math.Min(info.Width, info.Height);

            var shader = SKShader.CreateSweepGradient(
                new SKPoint(size / 2, size / 2),
                new[]
                {
                    ProgressStartColor.ToSKColor(),
                    ProgressEndColor.ToSKColor(),
                    ProgressStartColor.ToSKColor()
                },
                new[]
                {
                    StartAngle / 360,
                    (StartAngle + progressAngle + 1) / 360,
                    (StartAngle + progressAngle + 2) / 360
                });

            var paint = new SKPaint
            {
                Shader = shader,
                StrokeWidth = StrokeWidth,
                IsStroke = true,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            };

            DrawCircle(info, canvas, paint, progressAngle);
        }

        private void DrawCircle(SKImageInfo info, SKCanvas canvas, SKPaint paint, float angle)
        {
            int size = Math.Min(info.Width, info.Height);
            float halfWidth = size / 2;
            float halfHeight = size / 2;

            using (SKPath path = new SKPath())
            {
                SKRect rect = new SKRect(
                    StrokeWidth,
                    StrokeWidth,
                    size - StrokeWidth,
                    size - StrokeWidth);

                path.AddArc(rect, StartAngle, angle);

                canvas.DrawPath(path, paint);
            }
        }
    }
}