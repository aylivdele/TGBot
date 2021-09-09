using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGBot.Models
{
    public class AnonMessage
    {
        public string Text { get; set; } = "";

        public string Password { get; set; } = "";

        public string Date { get; set; } = "";

        public AnonMessage()
        {
            this.Date = DateTime.Now.ToString("G");
        }
    }
}