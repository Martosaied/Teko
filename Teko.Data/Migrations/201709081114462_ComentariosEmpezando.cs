namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComentariosEmpezando : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comentarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Texto = c.String(),
                        ContenidoId = c.Int(nullable: false),
                        UsuarioId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comentarios", t => t.ParentId)
                .ForeignKey("dbo.Contenidos", t => t.ContenidoId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId)
                .Index(t => t.ParentId)
                .Index(t => t.ContenidoId)
                .Index(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comentarios", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comentarios", "ContenidoId", "dbo.Contenidos");
            DropForeignKey("dbo.Comentarios", "ParentId", "dbo.Comentarios");
            DropIndex("dbo.Comentarios", new[] { "UsuarioId" });
            DropIndex("dbo.Comentarios", new[] { "ContenidoId" });
            DropIndex("dbo.Comentarios", new[] { "ParentId" });
            DropTable("dbo.Comentarios");
        }
    }
}
