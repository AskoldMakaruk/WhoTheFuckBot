using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BotFramework.Bot;
using BotFramework.Responses;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class MainCommand : StaticCommand
    {
        private static Image<Rgba32> _template;
        public static Image<Rgba32> Template
        {
            get
            {
                if (_template == null)
                {
                    var bytes = typeof(MainCommand).Assembly.GetManifestResourceStream(Resources.First(c => c.Contains(".png")));
                    _template = Image.Load(bytes) as Image<Rgba32>;
                }
                return _template;
            }
        }

        private static readonly string[] Resources = typeof(MainCommand).Assembly.GetManifestResourceNames();
        private static readonly FontCollection Fonts = new FontCollection();
        private static readonly FontFamily Arial = Fonts.Install(typeof(MainCommand).Assembly.GetManifestResourceStream(Resources.First(c => c.Contains(".ttf"))));

        public override Response Execute(Account account, Message message, Client client)
        {
            try
            {
                var text = "Та хто цей ваш " + message.Text + " нахуй?";
                if (!string.IsNullOrEmpty(account.TemplateText))
                {
                    if (account.TemplateText.Contains('$'))
                    {
                        text = account.TemplateText.Replace("$", message.Text);
                    }
                    else text = account.TemplateText + " " + message.Text;
                }

                using var image = Template.Clone();
                var words = text.Split(' ');

                var font = Arial.CreateFont(12, FontStyle.Regular);

                var canvasPoint = new PointF(15, 15);
                var canvasSize = new SizeF(512 - 15, 120);

                font = GetLargestFont(canvasSize, font, text);

                var render = new RendererOptions(font);
                foreach (var line in SplitTextForMaxWidth(canvasSize.Width, font, text))
                {
                    var textSize = TextMeasurer.Measure(line, render);
                    var point = new PointF(canvasPoint.X + (canvasSize.Width - textSize.Width) / 2, canvasPoint.Y);
                    image.Mutate(cl => cl.DrawText(line, font, Color.Black, point));
                    canvasPoint.Y += textSize.Height;
                }
                var str = new MemoryStream();
                image.SaveAsJpeg(str);
                str.Seek(0, SeekOrigin.Begin);
                account.Controller.AddLog(new Log()
                {
                    AccountId = account.Id,
                        Template = account.TemplateText?? "Та хто цей ваш $ нахуй?",
                        UserText = message.Text,
                        Time = DateTime.Now
                });

                return new Response().AddMessage(new SendPhoto(message.From.Id, new InputOnlineFile(str, "photo.png")));

            }
            catch
            {
                return new Response().AddMessage(new TextMessage(message.From.Id, "504 internal server error."));
            }

        }

        private Font GetLargestFont(SizeF canvas, Font font, string inputText)
        {
            var canvasArea = canvas.Height * canvas.Width;
            var text = inputText.Replace("\n", " ");
            for (var i = 0; i < 1000; i++)
            {
                var t = GetTextArea(i);
                var textArea = t.Height * t.Width;
                if (canvasArea <= textArea || t.Height > canvas.Height)
                {
                    return font.Family.CreateFont(i - 1, FontStyle.Regular);
                }
            }
            return font;
            SizeF GetTextArea(int size)
            {
                var font = Arial.CreateFont(size, FontStyle.Regular);
                return TextMeasurer.Measure(text, new RendererOptions(font));
            }
        }

        private IEnumerable<string> SplitTextForMaxWidth(float width, Font font, string input)
        {
            var render = new RendererOptions(font);
            var words = input.Replace("\n", " ").Split(' ');
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

        public override bool Suitable(Message message) => message.Text != null && !message.Text.StartsWith("/");
    }
}