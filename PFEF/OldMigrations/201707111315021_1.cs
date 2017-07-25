namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Escuelas", t => t.EscuelaActual_Id)
                .Index(t => t.EscuelaActual_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Materias", "InfoUsuarioViewModel_Id", "dbo.InfoUsuarioViewModels");
            DropForeignKey("dbo.InfoUsuarioViewModels", "EscuelaActual_Id", "dbo.Escuelas");
            DropIndex("dbo.InfoUsuarioViewModels", new[] { "EscuelaActual_Id" });
            DropIndex("dbo.Materias", new[] { "InfoUsuarioViewModel_Id" });
            DropColumn("dbo.Materias", "InfoUsuarioViewModel_Id");
            DropTable("dbo.InfoUsuarioViewModels");
        }
    }
}
