using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using static System.Net.Mime.MediaTypeNames;
using Font = SixLabors.Fonts.Font;
using Framework.AppPathHelper;

namespace Services.CaptchaProvider
{
    public  class CaptchaImageGenerator
    {
        private  readonly CaptchaOptions _options ;
        public CaptchaImageGenerator(CaptchaOptions options , IApplicationPathHelper applicationPathHelper)
        {
            _options = options;
            FontCollection fonts = new FontCollection();
            //var fontpath = System.IO.Path.Combine(options.FontPath);
            var fontpath = System.IO.Path.Combine(new[] { "assets", "fonts", "fonts", "ttf", "Vazirmatn-FD-NL-Bold.ttf" });
            var absolutefontpath = System.IO.Path.Combine(applicationPathHelper.WebRootPath, fontpath);
            FontFamily fontFamily = fonts.Add(absolutefontpath);
            _font = new Font(fontFamily, _options.FontSize, _options.FontStyle);

        }
        private   Font _font ;

        public IApplicationPathHelper ApplicationPathHelper { get; }

        /// <returns>
        /// image png bytes
        /// </returns>
        public  async Task<byte[]> GenerateAsync(string text, CancellationToken cancellationToken)
        {
            using var imgText = new Image<Rgba32>(_options.Width, _options.Height);

            float position = 0;
            var startWith = _options.Width / 2 + Random.Shared.Next(-50, -30);
            imgText.Mutate(ctx => ctx.BackgroundColor(Color.Transparent));

            foreach (var c in text)
            {
                var location = new PointF(startWith + position, _options.Height / 2 + Random.Shared.Next(-25, -15));
                imgText.Mutate(ctx => ctx.DrawText(c.ToString(), _font, _options.TextColor[Random.Shared.Next(0, _options.TextColor.Length)], location));
                position += TextMeasurer.MeasureSize(c.ToString(), new TextOptions(_font)).Width;
            }

            // add rotation
            var rotation = GetRotation();
            imgText.Mutate(ctx => ctx.Transform(rotation));

            // add lines and noise
            imgText.Mutate(ctx =>
            {
                for (int i = 0; i < _options.DrawLines; i++)
                {
                    var x0 = Random.Shared.Next(0, Random.Shared.Next(0, 30));
                    var y0 = Random.Shared.Next(10, imgText.Height);
                    var x1 = Random.Shared.Next(imgText.Width - Random.Shared.Next(0, (int)(imgText.Width * 0.25)), imgText.Width);
                    var y1 = Random.Shared.Next(0, imgText.Height);
                    ctx.DrawLine(_options.DrawLinesColor[Random.Shared.Next(0, _options.DrawLinesColor.Length)],
                        GetRandomThickness(_options.MinLineThickness, _options.MaxLineThickness),
                        new PointF[] { new(x0, y0), new(x1, y1) });
                }

                for (int i = 0; i < _options.NoiseRate; i++)
                {
                    var x0 = Random.Shared.Next(0, imgText.Width);
                    var y0 = Random.Shared.Next(0, imgText.Height);
                    ctx.DrawLine(_options.NoiseRateColor[Random.Shared.Next(0, _options.NoiseRateColor.Length)],
                        GetRandomThickness(0.5, 1.5), new PointF[] { new(x0, y0), new(x0, y0) });
                }
            });

            imgText.Mutate(x => x.Resize(_options.Width, _options.Height));

            using var ms = new MemoryStream();
            await imgText.SaveAsPngAsync(ms, cancellationToken);

            return ms.ToArray();
        }

        private  float GetRandomThickness(double min, double max)
        {
            double range = max - min;
            var sample = Random.Shared.NextDouble();
            var scaled = (sample * range) + min;
            return (float)scaled;
        }

        private  AffineTransformBuilder GetRotation()
        {
            var width = Random.Shared.Next(10, _options.Width);
            var height = Random.Shared.Next(10, _options.Height);
            var pointF = new PointF(width, height);
            var rotationDegrees = Random.Shared.Next(0, _options.MaxRotationDegrees);
            return new AffineTransformBuilder().PrependRotationDegrees(rotationDegrees, pointF);
        }

        public record CaptchaOptions
        {
            public CaptchaOptions(string fontFamily)
            {
                FontFamily = fontFamily;
            }
            public string FontFamily { get; set; } = "vazirmatn";
            public Color[] TextColor { get; set; } = new Color[] { Color.Blue, Color.Black, Color.Black, Color.Brown, Color.Gray, Color.Green };
            //F:\C\WorkSpace\Azad\_meshkatcore-main\meshkatcore\Server\Src\Presentations\WebApp\wwwroot\assets\fonts\fonts\ttf\Vazirmatn-FD-NL-Bold.ttf
            public string[] FontPath { get; set; } = new [] {"assets","fonts","fonts","ttf", "Vazirmatn-FD-NL-Bold.tff" };
            public Color[] DrawLinesColor { get; set; } = new Color[] { Color.Blue, Color.Black, Color.Black, Color.Brown, Color.Gray, Color.Green };
            public float MinLineThickness { get; set; } = 0.5f;
            public float MaxLineThickness { get; set; } = 1.0f;
            public ushort Width { get; set; } = 230;
            public ushort Height { get; set; } = 70;
            public ushort NoiseRate { get; set; } = 400;
            public Color[] NoiseRateColor { get; set; } = new Color[] { Color.Black, Color.Gray };
            public byte FontSize { get; set; } = 55;
            public FontStyle FontStyle { get; set; } = FontStyle.Bold;
            public byte DrawLines { get; set; } = 5;
            public byte MaxRotationDegrees { get; set; } = 5;
        }
    }
}
