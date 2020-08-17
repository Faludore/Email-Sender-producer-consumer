using System;
using System.Collections.Generic;
using System.Text;

namespace EmailSenderRrod_Confumer.Models
{
    class MailToSend
    {
        public int Id { get; set; }
        public string EmailTo { get; set; }
        public string Message { get; set; }
    }
}
