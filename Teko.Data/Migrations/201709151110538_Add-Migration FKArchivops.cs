namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationFKArchivops : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Archivos", "IdContenido_Id", "dbo.Contenidos");
            DropIndex("dbo.Archivos", new[] { "IdContenido_Id" });
            RenameColumn(table: "dbo.Archivos", name: "IdContenido_Id", newName: "IdContenido");
            AlterColumn("dbo.Archivos", "IdContenido", c => c.Int(nullable: false));
            CreateIndex("dbo.Archivos", "IdContenido");
            AddForeignKey("dbo.Archivos", "IdContenido", "dbo.Contenidos", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Archivos", "IdContenido", "dbo.Contenidos");
            DropIndex("dbo.Archivos", new[] { "IdContenido" });
            AlterColumn("dbo.Archivos", "IdContenido", c => c.Int());
            RenameColumn(table: "dbo.Archivos", name: "IdContenido", newName: "IdContenido_Id");
            CreateIndex("dbo.Archivos", "IdContenido_Id");
            AddForeignKey("dbo.Archivos", "IdContenido_Id", "dbo.Contenidos", "Id");
        }
    }
}
