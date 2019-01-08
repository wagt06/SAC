namespace SAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quejas
    {
        [Key]
        public int CodigoQueja { get; set; }

        public int CodigoTipo { get; set; }

        public int CodigoDepartamento { get; set; }

        public int CodigoSucursal { get; set; }

        [StringLength(60)]
        public string Empleado { get; set; }

        public string Queja { get; set; }

        public DateTime Fecha { get; set; }

        [StringLength(200)]
        public string Ubicacion { get; set; }

        public string Imagen { get; set; }
    }
}
