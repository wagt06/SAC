using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Casos
    {
        [Key]
        public int Id_Caso { get; set; }

        public int Codigo_Queja { get; set; }
        public int Codigo_Responsable { get; set; }

        [NotMapped]
        public virtual  string NombreResponsable { get; set; }

        public int? CodigoTipo { get; set; }
        public int? CodigoDepartamento { get; set; }
        public string Comentario { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public int? Codigo_Estado { get; set; }

        [NotMapped]
        public virtual string NombreEstado{ get; set; }

        public DateTime? Fecha_Inicio { get; set; }
        public DateTime? Fecha_Finalizacion { get; set; }


    }

    public class CasoVista {
        public Casos _Caso { get; set; }
        public int CodigoTipo { get; set; }
        public int CodigoDepartamento { get; set; }
        public string _Responsable { get; set; }
        public List<Area> _Areas { get; set; }
        public Quejas _Queja { get; set; }
        public List<Casos_Usuarios_vista> _Observadores = new List<Casos_Usuarios_vista>(); 
        public List<Comentarios_vista> _Comentarios = new List<Comentarios_vista>();
    }

   

}