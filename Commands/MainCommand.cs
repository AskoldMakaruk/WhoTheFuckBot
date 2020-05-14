using System;
using System.IO;
using BotFramework.Bot;
using BotFramework.Responses;
using MemeBot.DB.Model;
using MemeBot.Imaging;
using SixLabors.ImageSharp;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace MemeBot.Telegram.Commands
{
    public class HandleStickerCommand : StaticCommand
    {
        public override bool Suitable(Message message) => message.Sticker != null;

        public override Response Execute(Account account, Message message, Client client)
        {
            using (var stream = new MemoryStream())
            {
                client.GetInfoAndDownloadFileAsync(message.Sticker.FileId, stream).Wait();
                var image = Image.Load(stream);
                image.SaveAsPng(File.OpenWrite("sticker.png"));
            }

            return new Response(new TextMessage(message.Chat, "some response"));
        }
    }

    public class MainCommand : StaticCommand
    {
        public override Response Execute(Account account, Message message, Client client)
        {
            try
            {
                using var memstr = new MemoryStream();
                client.GetInfoAndDownloadFileAsync(message.Photo[0].FileId, memstr).Wait();

                var str = Renderer.Render(message.Caption, memstr);

                var sendphoto = new SendPhoto(message.From.Id, new InputOnlineFile(str, "photo.png"));
                sendphoto.OnSend += m =>
                {
                    str.Dispose();
                    //here i need to add log
                    // account.Controller.AddLog(new Log()
                    // {
                    //     AccountId = account.Id,
                    //         Template = account.TemplateText?? "Та хто цей ваш $ нахуй?",
                    //         UserText = message.Text,
                    //         Time = DateTime.Now
                    // });
                };
                return new Response(sendphoto);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return new Response(new TextMessage(message.From.Id, "504 internal server error."));
            }
        }

        public override bool Suitable(Message message) => message.Photo != null && message.Caption != null;
    }
}