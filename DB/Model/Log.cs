using System;

namespace WhoTheFuckBot.DB.Model
{
    public class Log
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string UserText { get; set; }
        public string Template { get; set; }
        public DateTime Time { get; set; }
    }
}