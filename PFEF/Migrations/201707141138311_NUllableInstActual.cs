namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NUllableInstActual : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuarios", "InstitucionActualId", "dbo.Escuelas");
            DropIndex("dbo.Usuarios", new[] { "InstitucionActualId" });
            AlterColumn("dbo.Usuarios", "InstitucionActualId", c => c.Int());
            CreateIndex("dbo.Usuarios", "InstitucionActualId");
            AddForeignKey("dbo.Usuarios", "InstitucionActualId", "dbo.Escuelas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "InstitucionActualId", "dbo.Escuelas");
            DropIndex("dbo.Usuarios", new[] { "InstitucionActualId" });
            AlterColumn("dbo.Usuarios", "InstitucionActualId", c => c.Int(nullable: false));
            CreateIndex("dbo.Usuarios", "InstitucionActualId");
            AddForeignKey("dbo.Usuarios", "InstitucionActualId", "dbo.Escuelas", "Id", cascadeDelete: true);
        }
    }
}
