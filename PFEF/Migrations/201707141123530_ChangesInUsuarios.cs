namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInUsuarios : DbMigration
    {
        public override void Up()
        {

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "InstitucionActualId", "dbo.Escuelas");
            DropIndex("dbo.Usuarios", new[] { "InstitucionActualId" });
            AlterColumn("dbo.Usuarios", "InstitucionActualId", c => c.Int());
            AlterColumn("dbo.Usuarios", "InstitucionActualId", c => c.String());
            RenameColumn(table: "dbo.Usuarios", name: "InstitucionActualId", newName: "InstitucionActual_Id");
            AddColumn("dbo.Usuarios", "InstitucionActualId", c => c.String());
            CreateIndex("dbo.Usuarios", "InstitucionActual_Id");
            AddForeignKey("dbo.Usuarios", "InstitucionActual_Id", "dbo.Escuelas", "Id");
        }
    }
}
