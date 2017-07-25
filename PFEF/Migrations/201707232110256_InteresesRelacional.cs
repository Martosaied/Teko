namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InteresesRelacional : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InteresesEscuelas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contador = c.Int(nullable: false),
                        IdEscuela_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Escuelas", t => t.IdEscuela_Id)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario_Id)
                .Index(t => t.IdEscuela_Id)
                .Index(t => t.IdUsuario_Id);
            
            CreateTable(
                "dbo.InteresesMaterias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contador = c.Int(nullable: false),
                        IdMateria_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Materias", t => t.IdMateria_Id)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario_Id)
                .Index(t => t.IdMateria_Id)
                .Index(t => t.IdUsuario_Id);
            
            CreateTable(
                "dbo.InteresesProfesores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Profesor = c.String(),
                        Contador = c.Int(nullable: false),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario_Id)
                .Index(t => t.IdUsuario_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InteresesProfesores", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.InteresesMaterias", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.InteresesMaterias", "IdMateria_Id", "dbo.Materias");
            DropForeignKey("dbo.InteresesEscuelas", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.InteresesEscuelas", "IdEscuela_Id", "dbo.Escuelas");
            DropIndex("dbo.InteresesProfesores", new[] { "IdUsuario_Id" });
            DropIndex("dbo.InteresesMaterias", new[] { "IdUsuario_Id" });
            DropIndex("dbo.InteresesMaterias", new[] { "IdMateria_Id" });
            DropIndex("dbo.InteresesEscuelas", new[] { "IdUsuario_Id" });
            DropIndex("dbo.InteresesEscuelas", new[] { "IdEscuela_Id" });
            DropTable("dbo.InteresesProfesores");
            DropTable("dbo.InteresesMaterias");
            DropTable("dbo.InteresesEscuelas");
        }
    }
}
