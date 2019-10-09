using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoTheFuckBot.DB.Model
{
    public class AccountToTemplate
    {
        public int Id { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [ForeignKey("TemplateId")]
        public Template Template { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}