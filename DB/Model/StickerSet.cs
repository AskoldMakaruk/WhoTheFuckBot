using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoTheFuckBot.DB.Model
{
    public class StickerSet
    {
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string Id { get; set; }
        public string PackId { get; set; }
        public string Name { get; set; }
    }
}