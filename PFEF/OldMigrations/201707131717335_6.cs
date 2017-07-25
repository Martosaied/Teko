namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "InstitucionActual_Id", c => c.Int());
            CreateIndex("dbo.Usuarios", "InstitucionActual_Id");
            AddForeignKey("dbo.Usuarios", "InstitucionActual_Id", "dbo.Escuelas", "Id");
            DropColumn("dbo.Usuarios", "InstitucionActual");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "InstitucionActual", c => c.String());
            DropForeignKey("dbo.Usuarios", "InstitucionActual_Id", "dbo.Escuelas");
            DropIndex("dbo.Usuarios", new[] { "InstitucionActual_Id" });
            DropColumn("dbo.Usuarios", "InstitucionActual_Id");
            DropColumn("dbo.Usuarios", "InstitucionActualId");
        }
    }
}
