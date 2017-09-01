using Microsoft.AspNet.Identity.EntityFramework;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data
{
    public class DbEntities : IdentityDbContext<ApplicationUser>
    {
        public DbEntities() : base("DbEntities") { }

        public virtual DbSet<Contenidos> Contenidos { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Escuelas> Escuelas { get; set; }
        public virtual DbSet<Materias> Materias { get; set; }
        public virtual DbSet<Valoraciones> Valoraciones { get; set; }
        public virtual DbSet<NivelesEducativos> NivelesEducativos { get; set; }
        public virtual DbSet<TiposContenidos> TiposContenidos { get; set; }
        public virtual DbSet<Visitas> Visitas { get; set; }
        public virtual DbSet<Archivos> Archivos { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }


        //Falta la configuracion de los demas
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContenidosConfiguration());
        }
    }
}
