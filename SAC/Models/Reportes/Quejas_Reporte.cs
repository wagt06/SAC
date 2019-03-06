using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Models.Reportes
{
    public class Quejas_Reporte
    {
        public int CodigoQueja { get; set; }
        public int CodigoTipo { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoDepartamento { get; set; }
        public string NombreArea { get; set; }
        public string Empleado { get; set; }
        public string Queja { get; set; }
        public DateTime Fecha { get; set; }
    }
}