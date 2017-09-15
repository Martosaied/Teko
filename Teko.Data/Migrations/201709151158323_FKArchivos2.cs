namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKArchivos2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Valoraciones", "Contenido_Id", "dbo.Contenidos");
            DropIndex("dbo.Valoraciones", new[] { "Contenido_Id" });
            RenameColumn(table: "dbo.Valoraciones", name: "Contenido_Id", newName: "IdContenido");
            AlterColumn("dbo.Valoraciones", "IdContenido", c => c.Int(nullable: false));
            CreateIndex("dbo.Valoraciones", "IdContenido");
            AddForeignKey("dbo.Valoraciones", "IdContenido", "dbo.Contenidos", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Valoraciones", "IdContenido", "dbo.Contenidos");
            DropIndex("dbo.Valoraciones", new[] { "IdContenido" });
            AlterColumn("dbo.Valoraciones", "IdContenido", c => c.Int());
            RenameColumn(table: "dbo.Valoraciones", name: "IdContenido", newName: "Contenido_Id");
            CreateIndex("dbo.Valoraciones", "Contenido_Id");
            AddForeignKey("dbo.Valoraciones", "Contenido_Id", "dbo.Contenidos", "Id");
        }
    }
}
