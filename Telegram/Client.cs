using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BotApi.Controllers;
using BotApi.DB;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Image = BotApi.DB.Model.Image;

namespace BotApi.Telegram
{
    public class Client
    {
        public Client(string token)
        {
            Controller = new TelegramController();
            Controller.Start();
            Images = Controller.GetImages().ToList();

            Bot = new TelegramBotClient(token);
            Bot.OnMessage += OnMessageRecieved;
            Bot.OnInlineQuery += OnInlineQueryReceived;
            Bot.OnInlineResultChosen += OnInlineResultChosen;
        }

        public static TelegramController Controller { get; set; }
        public static List<Image> Images { get; set; }
        public TelegramBotClient Bot { get; }

        public async void OnMessageRecieved(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Type == MessageType.Photo)
            {
                var photo = message.Photo.Last();
                using(var stream = new MemoryStream())
                {
                    await GetInfoAndDownloadFileAsync(photo.FileId, stream);
                    var image = new Image
                    {
                        TelegramId = photo.FileId,
                        UsedCount = 0,
                        Value = stream.ToArray()
                    };
                    Controller.AddImage(image);
                    Images = Controller.GetImages().ToList();
                }

                await Bot.SendTextMessageAsync(message.Chat, "Image added");
            }

            Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.Message.From.Username + ": " + e.Message.Text);
        }

        public static string Route => "http://http://cottonbox.in.ua/Image/";

        public async void OnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            var query = e.InlineQuery;
            try
            {
                if (query.Query == "")
                {
                    await Bot.AnswerInlineQueryAsync(query.Id,
                        Images.Select(m => new InlineQueryResultCachedPhoto(m.Id.ToString(), m.TelegramId)).ToArray());
                }
                else
                {
                    var imgs = ImageController.Images;
                    imgs.RemoveAll(i => i.AccountId == query.From.Id);

                    var newImgs = Images.Select(im =>
                            (ImageHelper.DrawText(im, query.Query), $"{im.Id}_{query.From.Id}.jpg",
                                query.From.Id))
                        .ToList();

                    imgs.AddRange(newImgs);

                    var resultImages = newImgs.Select(m => new InlineQueryResultPhoto(Guid.NewGuid().ToString(), Route + m.Item2, Route + m.Item2))
                        .ToArray();
                    foreach (var img in resultImages)
                        System.Console.WriteLine(img.ThumbUrl);
                    await Bot.AnswerInlineQueryAsync(query.Id, resultImages);
                    // await Bot.AnswerInlineQueryAsync(query.Id, new []
                    // {
                    //     new InlineQueryResultPhoto("1", "https://imgflip.com/s/meme/Mocking-Spongebob.jpg", "https://imgflip.com/s/meme/Mocking-Spongebob.jpg")
                    // });
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            System.Console.WriteLine(e.InlineQuery.Query);
        }

        public void OnInlineResultChosen(object sender, ChosenInlineResultEventArgs e)
        {
            var result = e.ChosenInlineResult;
            try
            {
                var imgs = ImageController.Images;
                imgs.RemoveAll(i => i.AccountId == result.From.Id);

                var image = Images.FirstOrDefault(i => i.Id == int.Parse(result.ResultId));
                if (image == null) return;
                image.UsedCount++;
                Controller.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public async Task GetInfoAndDownloadFileAsync(string documentFileId, MemoryStream ms) =>
            await Bot.GetInfoAndDownloadFileAsync(documentFileId, ms);
    }
}