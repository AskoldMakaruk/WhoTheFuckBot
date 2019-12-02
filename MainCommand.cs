using System;
using System.IO;
using System.Linq;
using BotFramework;
using BotFramework.Bot;
using BotFramework.Commands;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
namespace WhoTheFuckBot.Telegram.Commands
{
    public class MainCommand : IStaticCommand
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

        public Response Execute(Message message, Client client)
        {
            // using var controller = new Controllers.Controller();
            // controller.Start();

            // controller.FromMessage(message);

            try
            {
                using var image = Template.Clone();
                var rectangle = new []
                {
                    new PointF(160, 85),
                    new PointF(160 + 130, 85),
                    new PointF(160 + 130, 85 + 80),
                    new PointF(160, 85 + 80),
                };
                image.Mutate(cl => cl.FillPolygon(GraphicsOptions.Default, Brushes.Solid(Color.White), rectangle));

                var font = Arial.CreateFont(16, FontStyle.Regular);
                var size = TextMeasurer.Measure(message.Text, new RendererOptions(font));

                float scalingFactor = 80 / size.Height;
                var scaledFont = new Font(font, scalingFactor * font.Size);

                image.Mutate(cl => cl.DrawText(message.Text, scaledFont, Color.Black, new PointF(170, 90)));
                var str = new MemoryStream();
                image.SaveAsJpeg(str);
                str.Seek(0, SeekOrigin.Begin);
                return new Response(this, new SetBitmapPath()).SendPhoto(message.From.Id, new InputOnlineFile(str, "photo.png"));
            }
            catch (Exception e)
            {
                client.Write(e.ToString());
                return new Response(this, new SetBitmapPath()).SendTextMessage(message.From.Id, "504 internal server error.");
            }
        }

        public bool Suitable(Message message) => message.Text != null && !message.Text.StartsWith("/set");
    }
}