namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "InstitucionActual_Id", c => c.Int());
            CreateIndex("dbo.Usuarios", "InstitucionActual_Id");
            AddForeignKey("dbo.Usuarios", "InstitucionActual_Id", "dbo.Escuelas", "Id");
        }
    }
}
