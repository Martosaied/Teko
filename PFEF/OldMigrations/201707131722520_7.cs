namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _7 : DbMigration
    {
        public override void Up()
        {
        
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "InstitucionActualId", c => c.String());
            DropForeignKey("dbo.Usuarios", "InstitucionActual_Id1", "dbo.Escuelas");
            DropIndex("dbo.Usuarios", new[] { "InstitucionActual_Id1" });
            AlterColumn("dbo.Usuarios", "InstitucionActual_Id", c => c.Int());
            DropColumn("dbo.Usuarios", "InstitucionActual_Id1");
            CreateIndex("dbo.Usuarios", "InstitucionActual_Id");
            AddForeignKey("dbo.Usuarios", "InstitucionActual_Id", "dbo.Escuelas", "Id");
        }
    }
}
