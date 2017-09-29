namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKEscuela : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Escuelas", "NivEduEscuela_Id", "dbo.NivelesEducativos");
            DropIndex("dbo.Escuelas", new[] { "NivEduEscuela_Id" });
            AlterColumn("dbo.Escuelas", "NivEduEscuela_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Escuelas", "NivEduEscuela_Id");
            AddForeignKey("dbo.Escuelas", "NivEduEscuela_Id", "dbo.NivelesEducativos", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Escuelas", "NivEduEscuela_Id", "dbo.NivelesEducativos");
            DropIndex("dbo.Escuelas", new[] { "NivEduEscuela_Id" });
            AlterColumn("dbo.Escuelas", "NivEduEscuela_Id", c => c.Int());
            CreateIndex("dbo.Escuelas", "NivEduEscuela_Id");
            AddForeignKey("dbo.Escuelas", "NivEduEscuela_Id", "dbo.NivelesEducativos", "Id");
        }
    }
}
