namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Valoration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Valoraciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Valoracion = c.Int(nullable: false),
                        Contenido_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.Contenido_Id)
                .ForeignKey("dbo.Usuarios", t => t.User_Id)
                .Index(t => t.Contenido_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Contenidos", "ValoracionPromedio", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Valoraciones", "User_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Valoraciones", "Contenido_Id", "dbo.Contenidos");
            DropIndex("dbo.Valoraciones", new[] { "User_Id" });
            DropIndex("dbo.Valoraciones", new[] { "Contenido_Id" });
            DropColumn("dbo.Contenidos", "ValoracionPromedio");
            DropTable("dbo.Valoraciones");
        }
    }
}
