using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Casos
    {
        public int Id_Caso { get; set; }
        public int Codigo_Queja { get; set; }
        public int Codigo_Responsable { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public DateTime Fecha_Modificacion { get; set; }

    }
}