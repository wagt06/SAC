namespace SAC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using SAC.Models.Resoluciones;
    using SAC.Models.Seguridad;

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
        public virtual DbSet<TiposQuejas> TiposQuejas { get; set; }

        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Permiso> Permiso { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permiso>()
                         .Property(e => e.Modulo)
                         .IsUnicode(false);

            modelBuilder.Entity<Permiso>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

              modelBuilder.Entity<Permiso>()
                .HasMany(e => e.Rol)
                .WithMany(e => e.Permiso)
                .Map(m => m.ToTable("PermisoDenegadoPorRol").MapLeftKey("PermisoID").MapRightKey("RollID"));

            modelBuilder.Entity<Rol>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.Rol)
                .HasForeignKey(e => e.Roll)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Correo)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Contraseña)
                .IsUnicode(false);
     

    }

        public System.Data.Entity.DbSet<SAC.Models.Resoluciones.Comentarios_vista> Comentarios_vista { get; set; }
    }
}
