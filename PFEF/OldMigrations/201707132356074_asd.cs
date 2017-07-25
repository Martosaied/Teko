namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contenidos1", "EscuelasId", "dbo.Escuelas");
            DropForeignKey("dbo.Contenidos1", "MateriasId", "dbo.Materias");
            DropForeignKey("dbo.Contenidos1", "NivelesEducativosId", "dbo.NivelesEducativos");
            DropForeignKey("dbo.Contenidos1", "TiposContenidosId", "dbo.TiposContenidos");
            DropForeignKey("dbo.Contenidos1", "Usuarios_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Contenidos", "Usuarios_Id", "dbo.Usuarios");
            DropIndex("dbo.Contenidos", new[] { "Usuarios_Id" });
            DropIndex("dbo.Contenidos1", new[] { "EscuelasId" });
            DropIndex("dbo.Contenidos1", new[] { "MateriasId" });
            DropIndex("dbo.Contenidos1", new[] { "NivelesEducativosId" });
            DropIndex("dbo.Contenidos1", new[] { "TiposContenidosId" });
            DropIndex("dbo.Contenidos1", new[] { "Usuarios_Id" });
            AlterColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Contenidos", "Usuarios_Id1");
            AddForeignKey("dbo.Contenidos", "Usuarios_Id1", "dbo.Usuarios", "Id");
            DropTable("dbo.Contenidos1");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Contenidos1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Ruta = c.String(),
                        Profesor = c.String(),
                        Cursada = c.Int(nullable: false),
                        EscuelasId = c.Int(),
                        MateriasId = c.Int(),
                        NivelesEducativosId = c.Int(),
                        TiposContenidosId = c.Int(),
                        IPop = c.Int(),
                        IDes = c.Int(),
                        FechaSubida = c.DateTime(),
                        Usuarios_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Contenidos", "Usuarios_Id1", "dbo.Usuarios");
            DropIndex("dbo.Contenidos", new[] { "Usuarios_Id1" });
            AlterColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int());
            CreateIndex("dbo.Contenidos1", "Usuarios_Id");
            CreateIndex("dbo.Contenidos1", "TiposContenidosId");
            CreateIndex("dbo.Contenidos1", "NivelesEducativosId");
            CreateIndex("dbo.Contenidos1", "MateriasId");
            CreateIndex("dbo.Contenidos1", "EscuelasId");
            AddForeignKey("dbo.Contenidos1", "Usuarios_Id", "dbo.Usuarios", "Id");
            AddForeignKey("dbo.Contenidos1", "TiposContenidosId", "dbo.TiposContenidos", "Id");
            AddForeignKey("dbo.Contenidos1", "NivelesEducativosId", "dbo.NivelesEducativos", "Id");
            AddForeignKey("dbo.Contenidos1", "MateriasId", "dbo.Materias", "Id");
            AddForeignKey("dbo.Contenidos1", "EscuelasId", "dbo.Escuelas", "Id");
        }
    }
}
