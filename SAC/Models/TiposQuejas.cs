using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models
{
    public class TiposQuejas
    {
        [Key]
        public int CodigoTipoQueja{ get; set; }
        public string Descripcion { get; set; }
        public Boolean Activo { get; set; }
    }


}