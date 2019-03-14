using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models
{
    public class Usuarios

    {
        [Key]
        public int CodigoUsuario { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Img { get; set; }
        public int  Roll { get; set; }
    }
}