namespace SAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quejas
    {
        public Quejas()
        {
            Fecha = DateTime.Now;
        }
        [Key]
        public int CodigoQueja { get; set; }

        public int CodigoTipo { get; set; }

        public int CodigoDepartamento { get; set; }

        public int CodigoSucursal { get; set; }

        [StringLength(60)]
        public string Empleado { get; set; }

        public string Queja { get; set; }

        public DateTime Fecha { get; set; }

        public int CodigoEstado { get; set; }

        [StringLength(200)]
        public string Ubicacion { get; set; }

        public string Imagen { get; set; }
    }


    public  class QuejasVista
    {
  
        public int CodigoQueja { get; set; }

        public int CodigoTipo { get; set; }
        public String TipoNombre { get; set; }

        public int CodigoDepartamento { get; set; }
        public String Departamento { get; set; }

        public int CodigoSucursal { get; set; }

        [StringLength(60)]
        public string Empleado { get; set; }

        public string Queja { get; set; }

        public DateTime Fecha { get; set; }

        public int CodigoEstado { get; set; }
        public String EstadoNombre { get; set; }
        public Boolean IsCierre { get; set; }

        [StringLength(200)]
        public string Ubicacion { get; set; }

        public string Imagen { get; set; }

        public List<Models.Resoluciones.CasoVista> CasosVista = new List<Resoluciones.CasoVista>();
    }

}
