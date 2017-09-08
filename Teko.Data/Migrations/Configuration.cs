namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Teko.Data.DbEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Teko.Data.DbEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            /*context.NivelesEducativos.AddOrUpdate(
                p => p.Nombre,
                new Model.NivelesEducativos { Nombre ="Terciario"},
                new Model.NivelesEducativos { Nombre = "Secundario" },
                new Model.NivelesEducativos { Nombre = "Universitario" },
                new Model.NivelesEducativos { Nombre = "Primario" }
                );

            context.TiposContenidos.AddOrUpdate(
                p => p.Nombre,
                new Model.TiposContenidos { Nombre = "Resumen" },
                new Model.TiposContenidos { Nombre = "Trabajo practico" },
                new Model.TiposContenidos { Nombre = "Prueba" },
                new Model.TiposContenidos { Nombre = "Apunte" }
                );

            context.Materias.AddOrUpdate(
                p => p.Nombre,
                new Model.Materias { Nombre = "Matematica" },
                new Model.Materias { Nombre = "Lengua" },
                new Model.Materias { Nombre = "Geografia" },
                new Model.Materias { Nombre = "Ciencias Sociales" }
                );
            context.Escuelas.AddOrUpdate(
                p => p.Nombre,
                new Model.Escuelas { Nombre = "Matematica" },
                new Model.Escuelas { Nombre = "Lengua" },
                new Model.Escuelas { Nombre = "Geografia" },
                new Model.Escuelas { Nombre = "Ciencias Sociales" }
                );*/
        }
    }
}
