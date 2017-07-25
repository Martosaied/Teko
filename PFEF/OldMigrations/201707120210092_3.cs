namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InfoUsuarioViewModels", "EscuelaActual_Id", "dbo.Escuelas");
            DropForeignKey("dbo.Materias", "InfoUsuarioViewModel_Id", "dbo.InfoUsuarioViewModels");
            DropIndex("dbo.Materias", new[] { "InfoUsuarioViewModel_Id" });
            DropIndex("dbo.InfoUsuarioViewModels", new[] { "EscuelaActual_Id" });
            AddColumn("dbo.Usuarios", "Intereses", c => c.String());
            DropColumn("dbo.Materias", "InfoUsuarioViewModel_Id");
            DropColumn("dbo.Usuarios", "MateriasInteres");
            DropColumn("dbo.Usuarios", "OtrosIntereses");
            DropTable("dbo.InfoUsuarioViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InfoUsuarioViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Descripcion = c.String(),
                        RutaFoto = c.String(),
                        DemasIntereses = c.String(),
                        EscuelaActual_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Usuarios", "OtrosIntereses", c => c.String());
            AddColumn("dbo.Usuarios", "MateriasInteres", c => c.String());
            AddColumn("dbo.Materias", "InfoUsuarioViewModel_Id", c => c.Int());
            DropColumn("dbo.Usuarios", "Intereses");
            CreateIndex("dbo.InfoUsuarioViewModels", "EscuelaActual_Id");
            CreateIndex("dbo.Materias", "InfoUsuarioViewModel_Id");
            AddForeignKey("dbo.Materias", "InfoUsuarioViewModel_Id", "dbo.InfoUsuarioViewModels", "Id");
            AddForeignKey("dbo.InfoUsuarioViewModels", "EscuelaActual_Id", "dbo.Escuelas", "Id");
        }
    }
}
