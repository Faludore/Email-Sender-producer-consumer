﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EmailSenderRrod_Confumer.Models
{
    class Mail
    {
        public int Id { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Message { get; set; }
        public string DnT { get; set; }
        public bool Status { get; set; }
    }
}
