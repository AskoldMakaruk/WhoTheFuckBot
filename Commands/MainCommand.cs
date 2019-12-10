using System;
using System.IO;
using System.Linq;
using System.Text;
using BotFramework;
using BotFramework.Bot;
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
    public class MainCommand : Command
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

        public override Response Run(Account account, Message message, Client client)
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
                //todo this split
                int minWordsOnLine = 3;
                int maxWordsOnLine = 6;
                int wordsOnCurrentLine = 0;
                var builder = new StringBuilder();
                if (words.Length > minWordsOnLine)
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (wordsOnCurrentLine < maxWordsOnLine)
                        {
                            builder.Append(words[i] + " ");
                            wordsOnCurrentLine++;
                        }
                        else
                        {
                            builder.Append(words[i] + "\n");
                            wordsOnCurrentLine = 0;
                        }

                    }
                    text = builder.ToString();
                }
                var font = Arial.CreateFont(14, FontStyle.Regular);
                var size = TextMeasurer.Measure(text, new RendererOptions(font));

                var canvasPoint = new PointF(15, 15);
                var canvasSize = new Size(512 - 15, 120);

                float maxHeight = canvasSize.Height / size.Height;
                float maxWidth = canvasSize.Width / size.Width;

                var scalingFactor = maxHeight > maxWidth?maxWidth : maxHeight;
                var scaledFont = new Font(font, scalingFactor * font.Size);
                size = TextMeasurer.Measure(text, new RendererOptions(scaledFont));
                image.Mutate(cl => cl.DrawText(text, scaledFont, Color.Black, canvasPoint));
                var str = new MemoryStream();
                image.SaveAsJpeg(str);
                str.Seek(0, SeekOrigin.Begin);
                account.Controller.AddLog(new DB.Model.Log()
                {
                    AccountId = account.Id,
                        Template = account.TemplateText?? "Та хто цей ваш $ нахуй?",
                        UserText = message.Text,
                        Time = DateTime.Now
                });

                return new Response().SendPhoto(message.From.Id, new InputOnlineFile(str, "photo.png"));
            }
            catch (Exception e)
            {
                client.Write(e.ToString());
                return new Response().SendTextMessage(message.From.Id, "504 internal server error.");
            }
        }

        public override bool Suitable(Message message) => message.Text != null && !message.Text.StartsWith("/");
    }
}