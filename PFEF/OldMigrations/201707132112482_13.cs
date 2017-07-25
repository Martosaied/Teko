namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Usuarios", "InstitucionActualId", "dbo.Escuelas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "InstitucionActual_Id", "dbo.Escuelas");
            DropIndex("dbo.Usuarios", new[] { "InstitucionActual_Id" });
            DropColumn("dbo.Usuarios", "InstitucionActual_Id");
        }
    }
}
