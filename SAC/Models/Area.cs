using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models
{
    public class Area
    {
        [Key]
        public int CodigoArea { get; set; }
        public string NombreArea { get; set; }
        public Boolean Activo { get; set; }

    }
}