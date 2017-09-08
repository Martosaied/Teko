using Microsoft.AspNet.Identity.EntityFramework;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data
{
    public class DbEntities : IdentityDbContext<Usuarios>
    {
        public DbEntities() : base("DefaultConnection") { }

        public virtual DbSet<Contenidos> Contenidos { get; set; }
        //public virtual DbSet<Usuarios> Usuarios { get; set; }
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
        public static DbContext Create()
        {
            return new DbEntities();
        }

        //Falta la configuracion de los demas

    }
}
