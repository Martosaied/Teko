namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _as : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Usuarios",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserName = c.String(),
                    Nombre = c.String(),
                    Apellido = c.String(),
                    RutaFoto = c.String(),
                    InstitucionActual = c.String(),
                    Descripcion = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuarios", "UserName", c => c.Int(nullable: false));
        }
    }
}
