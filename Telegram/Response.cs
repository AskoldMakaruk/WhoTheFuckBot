using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram
{
    public class Response
    {
        public static Response TextMessage(Account account, string text, IReplyMarkup replyMarkup = null,
            int replyToMessageId = 0) => new Response()
        {
            Account = account,
            Text = text,
            ReplyToMessageId = replyToMessageId,
            ReplyMarkup = replyMarkup,
            Type = ResponseType.TextMessage
        };

        public static Response EditTextMessage(Account account, int editMessageId, string text,
            IReplyMarkup replyMarkup = null) => new Response()
        {
            Account = account,
            Text = text,
            ReplyMarkup = replyMarkup,
            EditMessageId = editMessageId,
            Type = ResponseType.EditTextMesage
        };

        public static Response AnswerQueryMessage(string answerToMessageId, string text) => new Response()
        {
            AnswerToMessageId = answerToMessageId,
            Text = text,
            AnswerQuery = true,
            Type = ResponseType.AnswerQuery,
        };

        public static Response SendDocument(Account account,
            InputOnlineFile document,
            string caption = null,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null) => new Response()
        {
            Account = account,
            Text = caption,
            ReplyToMessageId = replyToMessageId,
            ReplyMarkup = replyMarkup,
            Document = document,
            Type = ResponseType.SendDocument
        };
        public static Response SendPhoto(Account account,
            InputOnlineFile document,
            string caption = null,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null) => new Response()
        {
            Account = account,
            Text = caption,
            ReplyToMessageId = replyToMessageId,
            ReplyMarkup = replyMarkup,
            Document = document,
            Type = ResponseType.SendPhoto
        };

        private Response() { }

        public Account Account { get; set; }
        public string Text { get; set; }
        public int ReplyToMessageId { get; set; } = 0;
        public IReplyMarkup ReplyMarkup { get; set; }
        public int EditMessageId { get; set; } = 0;
        public bool AnswerQuery { get; set; } = false;
        public string AnswerToMessageId { get; set; }
        public InputOnlineFile Document { get; set; }
        public ResponseType Type { get; private set; }
    }

    public enum ResponseType
    {
        TextMessage,
        EditTextMesage,
        AnswerQuery,
        SendDocument,
        SendPhoto
    }
}