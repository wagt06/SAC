namespace SAC.Models.Seguridad
{
    using global::Models.Commons;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Permiso")]
    public partial class Permiso
    {
        public Permiso()
        {
            Rol = new List<Rol>();
        }

        public RolesPermisos PermisoID { get; set; }

        [Required]
        [StringLength(20)]
        public string Modulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }

        public virtual ICollection<Rol> Rol { get; set; }
    }
}
