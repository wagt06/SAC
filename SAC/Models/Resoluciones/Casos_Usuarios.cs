using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models.Resoluciones
{
    public class Casos_Usuarios
    {
        [Key]
        public int Id_Caso_Usuario { get; set; }
        public int Id_Caso { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Rol { get; set; }
    }
    
    public class Casos_Usuarios_vista
    {
        [Key]
        public int Id_Caso_Usuario { get; set; }
        public int Id_Caso { get; set; }
        public int Id_Usuario { get; set; }
        public string Usuario { get; set; }
        public int Id_Rol { get; set; }
    }
}