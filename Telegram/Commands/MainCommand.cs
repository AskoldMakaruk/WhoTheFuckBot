using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using WhoTheFuckBot.DB.Model;
using WhoTheFuckBot.Telegram.Bot;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class MainCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            var res = 0;
            if (account.Status == AccountStatus.Free) res++;
            return res;
        }

        public override Response Execute(Message message, Client client, Account account)
        {
            // if (message.Document != null || message.Photo != null)
            // {
            //     using(var ms = new MemoryStream())
            //     {
            //         if (message.Document != null)
            //             client.GetInfoAndDownloadFileAsync(message.Document.FileId, ms).RunSynchronously();
            //         else
            //             client.GetInfoAndDownloadFileAsync(message.Photo[0].FileId, ms).RunSynchronously();
            //         var bm = new Bitmap(ms);

            //         var ratio = 512 / (double) (bm.Height > bm.Width ? bm.Height : bm.Width);
            //         bm = new Bitmap(bm, (int) (bm.Height * ratio), (int) (bm.Width * ratio));
            //         ms.Seek(0, SeekOrigin.Begin);
            //         bm.Save(ms, ImageFormat.Png);
            //         ms.Seek(0, SeekOrigin.Begin);
            //         var res = Response.SendDocument(account, new InputOnlineFile(ms, "photo.png"));
            //         bm.Dispose();
            //         return res;
            //     }
            // }
            if (message.Text != null)
            {
                try
                {
                    Bitmap bmp = new Bitmap(@"C:\Users\a.makaruk\Pictures\whothefuck.png");

                    var rect = new Rectangle(160, 85, 130, 80);

                    Graphics g = Graphics.FromImage(bmp);

                    g.FillRectangle(Brushes.White, new Rectangle(160, 85, 130, 70));
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    Font stringFont = new Font("Arial", 16);
                    stringFont = GetAdjustedFont(g, message.Text, stringFont, rect.Width, 50, 12, true);

                    g.DrawString(message.Text, stringFont, Brushes.Black, rect);

                    g.Flush();
                    var str = new MemoryStream();

                    bmp.Save(str, ImageFormat.Png);
                    str.Seek(0, SeekOrigin.Begin);

                    var res = Response.SendPhoto(account, new InputOnlineFile(str, "photo.png"));

                    return res;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }

            }

            return Response.TextMessage(account, "send photo");
        }
        public Font GetAdjustedFont(Graphics g, string graphicString, Font originalFont, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                SizeF adjustedSizeNew = g.MeasureString(graphicString, testFont);

                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    // Good font, return it
                    return testFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return minimumSize or original?
            if (smallestOnFail)
            {
                return testFont;
            }
            else
            {
                return originalFont;
            }
        }
    }
}