using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace WhoTheFuckBot.DB.Model
{
    public class Template : TelegramMedia
    {
        public int Id { get; set; }

        [ForeignKey("AuthorId")]
        public Account Account { get; set; }

        public override byte[] File { get; set; }
        public override string FileId { get; set; }
        public string RectangleString
        {
            get => $"{Rectangle.X};{Rectangle.Y};{Rectangle.Width};{Rectangle.Height}";
            set
            {
                var param = value.Split(';');
                Rectangle = new Rectangle(int.Parse(param[0]), int.Parse(param[1]), int.Parse(param[2]), int.Parse(param[3]));
            }
        }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public Rectangle Rectangle { get; set; }
    }
}