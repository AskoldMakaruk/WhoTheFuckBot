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

        public void HandleInlineQueryChoosen(ChosenInlineResult result)
        {
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

        public async Task HandleInlineQuery(InlineQuery query)
        {
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
                            (ImageHelper.DrawText(im, query.Query),
                                $"{Guid.NewGuid().ToString().Replace("-","")}.jpg",
                                query.From.Id, Id : im.Id))
                        .ToList();

                    imgs.AddRange(newImgs.Select(c => (c.Item1, c.Item2, c.Item3)));

                    var resultImages = newImgs.Select(m => new InlineQueryResultPhoto(m.Id.ToString(), Route + m.Item2, Route + m.Item2))
                        .ToArray();
                    foreach (var img in resultImages)
                        System.Console.WriteLine(img.ThumbUrl);
                    await Bot.AnswerInlineQueryAsync(query.Id, resultImages);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            System.Console.WriteLine(query.Query);
        }

        public async Task HandleMessage(Message message)
        {
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

            Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + message.From.Username + ": " + message.Text);
        }

        public async void OnMessageRecieved(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            await HandleMessage(message);
        }

        public static string Route => System.IO.File.Exists("Route.txt") ? System.IO.File.ReadAllText("Route.txt") : "http://134.249.124.62.xip.io/Image?imageName=";

        public async void OnInlineQueryReceived(object sender, InlineQueryEventArgs e)
        {
            var query = e.InlineQuery;
            await HandleInlineQuery(query);

        }

        public void OnInlineResultChosen(object sender, ChosenInlineResultEventArgs e)
        {
            var result = e.ChosenInlineResult;
            HandleInlineQueryChoosen(result);
        }

        public async Task GetInfoAndDownloadFileAsync(string documentFileId, MemoryStream ms) =>
            await Bot.GetInfoAndDownloadFileAsync(documentFileId, ms);
    }
}