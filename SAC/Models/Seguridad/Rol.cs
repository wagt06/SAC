namespace SAC.Models.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rol")]
    public partial class Rol
    {
        public Rol()
        {
            Usuario = new List<Usuarios>();
            Permiso = new List<Permiso>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public virtual ICollection<Usuarios> Usuario { get; set; }
        public virtual ICollection<Permiso> Permiso { get; set; }
    }
}
