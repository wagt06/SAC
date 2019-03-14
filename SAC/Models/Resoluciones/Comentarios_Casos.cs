using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Comentarios_Casos
    {
        [Key]
        public int Id_Comentario { get; set; }
        public int Id_Caso { get; set; }
        public int Codigo_Observador { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha_Comentario { get; set; }
    }

    public class Comentarios_vista
    {
        [Key]
        public int Id_Comentario { get; set; }
        public int Id_Caso { get; set; }
        public int Codigo_Observador { get; set; }
        public string Observador { get; set; }
        public string Comentario { get; set; }
        public string Img { get; set; }
        public DateTime Fecha_Comentario { get; set; }
    }
}