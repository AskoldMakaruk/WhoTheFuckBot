using System;

namespace MemeBot.DB.Model
{
    public class Meme
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int Likes { get; set; }
        public string FileId { get; set; }
        public DateTime Time { get; set; }
    }
    public class Like
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int MemeId { get; set; }
    }
}