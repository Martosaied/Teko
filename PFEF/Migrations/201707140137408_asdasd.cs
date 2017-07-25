namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Favoritos", "IdContenido_Id", "dbo.Contenidos");
            DropForeignKey("dbo.Favoritos", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Intereses", "IdMaterias_Id", "dbo.Materias");
            DropForeignKey("dbo.Intereses", "IdUsuario_Id", "dbo.Usuarios");
            DropIndex("dbo.Favoritos", new[] { "IdContenido_Id" });
            DropIndex("dbo.Favoritos", new[] { "IdUsuario_Id" });
            DropIndex("dbo.Intereses", new[] { "IdMaterias_Id" });
            DropIndex("dbo.Intereses", new[] { "IdUsuario_Id" });
            DropTable("dbo.Favoritos");
            DropTable("dbo.Intereses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Intereses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdMaterias_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Favoritos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContenido_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Intereses", "IdUsuario_Id");
            CreateIndex("dbo.Intereses", "IdMaterias_Id");
            CreateIndex("dbo.Favoritos", "IdUsuario_Id");
            CreateIndex("dbo.Favoritos", "IdContenido_Id");
            AddForeignKey("dbo.Intereses", "IdUsuario_Id", "dbo.Usuarios", "Id");
            AddForeignKey("dbo.Intereses", "IdMaterias_Id", "dbo.Materias", "Id");
            AddForeignKey("dbo.Favoritos", "IdUsuario_Id", "dbo.Usuarios", "Id");
            AddForeignKey("dbo.Favoritos", "IdContenido_Id", "dbo.Contenidos", "Id");
        }
    }
}
