using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using WhoTheFuckBot.DB.Model;
using WhoTheFuckBot.Images;
using WhoTheFuckBot.Telegram.Bot;
namespace WhoTheFuckBot.Telegram.Commands
{
    public class LoadTemplateCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.AddTemplate) res = 2;
            return res;
        }
        //rgb(184, 0, 230)
        public static readonly Color BasicColor = (Color) ColorTranslator.FromHtml("#b800e6");
        public override Response Execute(Message message, Client client, Account account)
        {
            using(var stream = new MemoryStream())
            {
                if (message.Document != null)
                {
                    client.GetInfoAndDownloadFileAsync(message.Document.FileId, stream).Wait();
                }
                else if (message.Photo != null)
                {
                    client.GetInfoAndDownloadFileAsync(message.Photo.First().FileId, stream).Wait();
                }
                else
                {
                    account.Status = AccountStatus.Free;
                    return Response.TextMessage(account, "no image detected");
                }

                var image = new Bitmap(stream);

                var pixels = new List < (int x, int y) > ();
                for (int i = 0; i < image.Height - 1; i++)
                {
                    for (int j = 0; j < image.Width - 1; j++)
                    {
                        var pixel = image.GetPixel(j, i);
                        if (pixel == BasicColor)
                            pixels.Add((j, i));
                    }

                }
                if (pixels.Count == 2)
                {
                    var template = new Template() { Account = account };

                    var first = pixels[0];
                    var second = pixels[1];
                    var x = first.x > second.x?second.x : first.x;
                    var y = first.y > second.y?second.y : first.y;
                    var w = Math.Abs(first.x - second.x);
                    var h = Math.Abs(first.y - second.y);

                    template.Rectangle = new Rectangle(x, y, w, h);
                    image = ImageHelper.DrawText(image, template.Rectangle, "Your text here", new Font("Arial", 10));

                    var imgStream = new MemoryStream();
                    {
                        image.Save(imgStream, ImageFormat.Png);
                        imgStream.Seek(0, SeekOrigin.Begin);
                        template.File = imgStream.ToArray();
                        imgStream.Seek(0, SeekOrigin.Begin);
                        account.Controller.AddTemplate(template);
                        return Response.SendPhoto(account, template);
                    }

                }

            }
            return Response.TextMessage(account, "internal error");
        }
    }
}