using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Comentarios_Casos
    {
        public int Id_Comentario { get; set; }
        public int Id_Caso { get; set; }
        public int Codigo_Observador { get; set; }
        public int Comentario { get; set; }
        public int Fecha_Comentario { get; set; }
    }
}