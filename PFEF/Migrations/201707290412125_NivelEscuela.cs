namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NivelEscuela : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Escuelas", "NivEduEscuela_Id", c => c.Int());
            CreateIndex("dbo.Escuelas", "NivEduEscuela_Id");
            AddForeignKey("dbo.Escuelas", "NivEduEscuela_Id", "dbo.NivelesEducativos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Escuelas", "NivEduEscuela_Id", "dbo.NivelesEducativos");
            DropIndex("dbo.Escuelas", new[] { "NivEduEscuela_Id" });
            DropColumn("dbo.Escuelas", "NivEduEscuela_Id");
        }
    }
}
