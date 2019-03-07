using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Estados_Casos
    {
        [Key]
        public int Codigo_Estado { get; set; }
        public string Nombre_Estado { get; set; }
        public Boolean  Activo { get; set; }
    }
}