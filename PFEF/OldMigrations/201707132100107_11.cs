namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "InstitucionActualId", c => c.Int());
            CreateIndex("dbo.Usuarios", "InstitucionActualId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "ContenidosDes", c => c.String());
            DropIndex("dbo.Usuarios", new[] { "InstitucionActual_Id" });
            AlterColumn("dbo.Usuarios", "InstitucionActual_Id", c => c.String());
            DropColumn("dbo.Usuarios", "InstitucionActualId");
            RenameColumn(table: "dbo.Usuarios", name: "InstitucionActual_Id", newName: "InstitucionActual_Id1");
            AddColumn("dbo.Usuarios", "InstitucionActual_Id", c => c.String());
            CreateIndex("dbo.Usuarios", "InstitucionActual_Id1");
        }
    }
}
