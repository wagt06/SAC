namespace SAC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using SAC.Models.Resoluciones;

    public partial class dbModel : DbContext
    {
        public dbModel()
            : base("name=dbModel")
        { 
        }

        public virtual DbSet<Quejas> Quejas { get; set; }
        public virtual DbSet<Config_mail> Config_mail { get; set; }
        public virtual DbSet<Graficos> Graficos { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        public virtual DbSet<Casos> Casos { get; set; }
        public virtual DbSet<Estados_Casos> Estados_Casos { get; set; }
        public virtual DbSet<Resoluciones_Historico> Resoluciones_Historico { get; set; }
        public virtual DbSet<Comentarios_Casos> Comentarios_Casos { get; set; }
        public virtual DbSet<Casos_Usuarios> Casos_Usuarios { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
