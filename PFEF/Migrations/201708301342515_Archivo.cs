namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Archivo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archivoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ruta = c.String(),
                        IdContenido_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.IdContenido_Id)
                .Index(t => t.IdContenido_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Archivoes", "IdContenido_Id", "dbo.Contenidos");
            DropIndex("dbo.Archivoes", new[] { "IdContenido_Id" });
            DropTable("dbo.Archivoes");
        }
    }
}
