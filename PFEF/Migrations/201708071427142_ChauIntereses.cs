namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChauIntereses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InteresesEscuelas", "IdEscuela_Id", "dbo.Escuelas");
            DropForeignKey("dbo.InteresesEscuelas", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.InteresesMaterias", "IdMateria_Id", "dbo.Materias");
            DropForeignKey("dbo.InteresesMaterias", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.InteresesProfesores", "IdUsuario_Id", "dbo.Usuarios");
            DropIndex("dbo.InteresesEscuelas", new[] { "IdEscuela_Id" });
            DropIndex("dbo.InteresesEscuelas", new[] { "IdUsuario_Id" });
            DropIndex("dbo.InteresesMaterias", new[] { "IdMateria_Id" });
            DropIndex("dbo.InteresesMaterias", new[] { "IdUsuario_Id" });
            DropIndex("dbo.InteresesProfesores", new[] { "IdUsuario_Id" });
            DropTable("dbo.InteresesEscuelas");
            DropTable("dbo.InteresesMaterias");
            DropTable("dbo.InteresesProfesores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InteresesProfesores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Profesor = c.String(),
                        Contador = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InteresesMaterias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contador = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IdMateria_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InteresesEscuelas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contador = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IdEscuela_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.InteresesProfesores", "IdUsuario_Id");
            CreateIndex("dbo.InteresesMaterias", "IdUsuario_Id");
            CreateIndex("dbo.InteresesMaterias", "IdMateria_Id");
            CreateIndex("dbo.InteresesEscuelas", "IdUsuario_Id");
            CreateIndex("dbo.InteresesEscuelas", "IdEscuela_Id");
            AddForeignKey("dbo.InteresesProfesores", "IdUsuario_Id", "dbo.Usuarios", "Id");
            AddForeignKey("dbo.InteresesMaterias", "IdUsuario_Id", "dbo.Usuarios", "Id");
            AddForeignKey("dbo.InteresesMaterias", "IdMateria_Id", "dbo.Materias", "Id");
            AddForeignKey("dbo.InteresesEscuelas", "IdUsuario_Id", "dbo.Usuarios", "Id");
            AddForeignKey("dbo.InteresesEscuelas", "IdEscuela_Id", "dbo.Escuelas", "Id");
        }
    }
}
