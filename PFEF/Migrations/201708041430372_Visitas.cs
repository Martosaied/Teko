namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visitas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visitas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contador = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        Contenido_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.Contenido_Id)
                .ForeignKey("dbo.Usuarios", t => t.User_Id)
                .Index(t => t.Contenido_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitas", "User_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Visitas", "Contenido_Id", "dbo.Contenidos");
            DropIndex("dbo.Visitas", new[] { "User_Id" });
            DropIndex("dbo.Visitas", new[] { "Contenido_Id" });
            DropTable("dbo.Visitas");
        }
    }
}
