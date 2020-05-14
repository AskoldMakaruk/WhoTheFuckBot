using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace MemeBot.Imaging
{
    public static class Renderer
    {
        private static readonly string[]       Resources = typeof(Program).Assembly.GetManifestResourceNames();
        private static readonly FontCollection Fonts     = new FontCollection();
        private static readonly FontFamily     Arial     = Fonts.Install(typeof(Program).Assembly.GetManifestResourceStream(Resources.First(c => c.Contains(".ttf"))));

        public static Stream Render(string text, Stream image)
        {
            image.Seek(0, SeekOrigin.Begin);
            using var imagi = Image.Load(image) as Image<Rgba32>;
            return Render(text, imagi);
        }

        public static Stream Render(string text, Image<Rgba32> image)
        {
            var words = text.Split(' ');

            var font = Arial.CreateFont(2, FontStyle.Regular);

            var canvasPoint = new PointF(image.Width * 0.125F, image.Height * 0.70F);
            var canvasSize  = new SizeF(image.Width  * 0.75F, image.Height  * 0.3F);

            font = GetLargestFont(canvasSize, font, text);

            var render = new RendererOptions(font);
            foreach (var line in SplitTextForMaxWidth(canvasSize.Width, font, text))
            {
                var textSize = TextMeasurer.Measure(line, render);
                var point    = new PointF(canvasPoint.X + (canvasSize.Width - textSize.Width) / 2, canvasPoint.Y);
                image.Mutate(cl => cl.DrawText(line, font, Brushes.Solid(Color.WhiteSmoke), Pens.Solid(Color.Black, 1), point));
                canvasPoint.Y += textSize.Height;
            }

            var str = new MemoryStream();
            image.SaveAsJpeg(str);
            str.Seek(0, SeekOrigin.Begin);
            return str;
        }

        private static Font GetLargestFont(SizeF canvas, Font font, string inputText)
        {
            var canvasArea = canvas.Height * canvas.Width;
            var text       = inputText.Replace("\n", " ");
            for (var i = 0; i < 1000; i++)
            {
                var currentFont = Arial.CreateFont(i, FontStyle.Regular);
                var t           = GetTextArea(currentFont);
                var textArea    = t.Height * t.Width;
                if (canvasArea <= textArea || t.Height > canvas.Height || canvas.Height < SplitTextForMaxWidth(t.Width, currentFont, text).Count() * t.Height)
                {
                    return font.Family.CreateFont(i - 1, FontStyle.Regular);
                }
            }

            return font;

            SizeF GetTextArea(Font font)
            {
                return TextMeasurer.Measure(text, new RendererOptions(font));
            }
        }

        private static IEnumerable<string> SplitTextForMaxWidth(float width, Font font, string input)
        {
            var    render      = new RendererOptions(font);
            var    words       = input.Replace("\n", " ").Split(' ');
            string currentLine = null;

            foreach (var word in words)
            {
                if (currentLine == null) currentLine = "";

                if (GetWidth(currentLine + word + " ") > width)
                {
                    yield return currentLine.Trim();
                    currentLine = word;
                }
                else currentLine += " " + word;
            }

            if (currentLine != null) yield return currentLine;

            float GetWidth(string line)
            {
                return TextMeasurer.Measure(line, render).Width;
            }
        }
    }
}