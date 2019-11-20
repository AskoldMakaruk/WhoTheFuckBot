using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using BotFramework;
using BotFramework.Bot;
using BotFramework.Commands;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
namespace WhoTheFuckBot.Telegram.Commands
{
    public class MainCommand : IStaticCommand
    {
        public Response Execute(Message message, Client client)
        {
            using var controller = new Controllers.Controller();
            controller.Start();

            controller.FromMessage(message);

            try
            {
                Bitmap bmp = new Bitmap(SetBitmapPath.BitmapPath);

                var rect = new Rectangle(160, 85, 130, 80);

                Graphics g = Graphics.FromImage(bmp);

                g.FillRectangle(Brushes.White, new Rectangle(160, 85, 130, 70));
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var fontFamily = new FontFamily(Path.Combine("Arial", "arial.ttf"));
                var font = new Font(fontFamily, 16);

                font = GetAdjustedFont(g, message.Text, font, rect.Width, 50, 12, true);

                g.DrawString(message.Text, font, Brushes.Black, rect);

                g.Flush();
                var str = new MemoryStream();

                bmp.Save(str, ImageFormat.Png);
                str.Seek(0, SeekOrigin.Begin);

                return new Response(this, new SetBitmapPath()).SendPhoto(message.From.Id, new InputOnlineFile(str, "photo.png"));
            }
            catch (Exception e)
            {
                client.Write(e.ToString());
                return new Response(this, new SetBitmapPath()).SendTextMessage(message.From.Id, "504 internal server error.");
            }
        }

        public Font GetAdjustedFont(Graphics g, string graphicString, Font originalFont, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            Font testFont = null;
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);
                SizeF adjustedSizeNew = g.MeasureString(graphicString, testFont);
                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    return testFont;
                }
            }
            if (smallestOnFail)
            {
                return testFont;
            }
            else
            {
                return originalFont;
            }
        }

        public bool Suitable(Message message) => message.Text != null && !message.Text.StartsWith("/set");
    }
}