using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Resoluciones_Historico
    {
        [Key]
        public int Id_Historico { get; set; }
        public int Id_Caso { get; set; }
        public int Codigo_Responsable { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public int Codigo_Estado { get; set; }
    }
}