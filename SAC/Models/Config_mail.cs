using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Models
{
    public class Config_mail
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Asunto { get; set; }
        public string Host { get; set; }
        public string Mail { get; set; }
        public string Pass { get; set; }
        public string Port { get; set; }
        public Boolean EnableSSl { get; set; }
        public string MailDestino { get; set; }
        public string MailDestinoSug { get; set; }
    }
}