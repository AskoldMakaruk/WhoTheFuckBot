namespace BotApi.DB.Model
{

    public class Image
    {
        public int Id { get; set; }
        public string TelegramId { get; set; }
        public byte[] Value { get; set; }
        public int UsedCount { get; set; }
    }
}