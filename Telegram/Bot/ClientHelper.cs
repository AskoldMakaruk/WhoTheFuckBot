using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Bot
{
    public partial class Client
    {
        private async Task<Message> SendTextMessageAsync(Response m)
        {
            try
            {
                Message result;
                switch (m.Type)
                {
                    case ResponseType.AnswerQuery:
                        break;
                    case ResponseType.EditTextMesage:
                        return await Bot.EditMessageTextAsync(m.Account, m.EditMessageId, m.Text,
                            replyMarkup : m.ReplyMarkup as InlineKeyboardMarkup);

                    case ResponseType.SendDocument:

                        return await Bot.SendDocumentAsync(m.Account, m.Document, m.Text);

                    case ResponseType.SendPhoto:
                        result = await Bot.SendPhotoAsync(m.Account, m.Document, m.Text);

                        return result;

                    case ResponseType.TextMessage:
                        return await Bot.SendTextMessageAsync(m.Account, m.Text, replyToMessageId : m.ReplyToMessageId,
                            replyMarkup : m.ReplyMarkup);
                    case ResponseType.Album:
                        result = await Bot.SendMediaGroupAsync(m.Account, m.Document, m.Text);

                        break;

                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return default;
        }

        private async Task<Message> SendTextMessageAsync(Account account, string text,
            ParseMode parseMode = ParseMode.Default,
            bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null,
            CancellationToken cancellationToken = default)
        {
            var message = await Bot.SendTextMessageAsync(account, text, parseMode, disableWebPagePreview,
                disableNotification, replyToMessageId, replyMarkup, cancellationToken);
            return message;
        }

        public async Task GetInfoAndDownloadFileAsync(string documentFileId, MemoryStream ms) =>
            await Bot.GetInfoAndDownloadFileAsync(documentFileId, ms);
    }
}