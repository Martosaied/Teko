namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTable : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Escuelas", t => t.EscuelasId)
                .ForeignKey("dbo.Materias", t => t.MateriasId)
                .ForeignKey("dbo.NivelesEducativos", t => t.NivelesEducativosId)
                .ForeignKey("dbo.TiposContenidos", t => t.TiposContenidosId)
                .ForeignKey("dbo.Usuarios", t => t.Usuarios_Id)
                .Index(t => t.EscuelasId)
                .Index(t => t.MateriasId)
                .Index(t => t.NivelesEducativosId)
                .Index(t => t.TiposContenidosId)
                .Index(t => t.Usuarios_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contenidos1", "Usuarios_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Contenidos1", "TiposContenidosId", "dbo.TiposContenidos");
            DropForeignKey("dbo.Contenidos1", "NivelesEducativosId", "dbo.NivelesEducativos");
            DropForeignKey("dbo.Contenidos1", "MateriasId", "dbo.Materias");
            DropForeignKey("dbo.Contenidos1", "EscuelasId", "dbo.Escuelas");
            DropIndex("dbo.Contenidos1", new[] { "Usuarios_Id" });
            DropIndex("dbo.Contenidos1", new[] { "TiposContenidosId" });
            DropIndex("dbo.Contenidos1", new[] { "NivelesEducativosId" });
            DropIndex("dbo.Contenidos1", new[] { "MateriasId" });
            DropIndex("dbo.Contenidos1", new[] { "EscuelasId" });
            DropIndex("dbo.Contenidos", new[] { "Usuarios_Id" });
            AlterColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int(nullable: false));
            DropTable("dbo.Contenidos1");
            RenameColumn(table: "dbo.Contenidos", name: "Usuarios_Id", newName: "Usuarios_Id1");
            AddColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Contenidos", "Usuarios_Id1");
        }
    }
}
