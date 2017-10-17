namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reportes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContenido = c.Int(nullable: false),
                        IdUsuario = c.String(maxLength: 128),
                        Titulo = c.String(),
                        Texto = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.IdContenido, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdContenido)
                .Index(t => t.IdUsuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reportes", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reportes", "IdContenido", "dbo.Contenidos");
            DropIndex("dbo.Reportes", new[] { "IdUsuario" });
            DropIndex("dbo.Reportes", new[] { "IdContenido" });
            DropTable("dbo.Reportes");
        }
    }
}
