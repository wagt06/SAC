using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Casos_Usuarios
    {
        public int Id_Caso_Usuario { get; set; }
        public int Id_Caso { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Rol { get; set; }
    }
}