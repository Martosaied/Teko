namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "FechaNacimiento", c => c.DateTime(nullable: false));
            AddColumn("dbo.Usuarios", "MateriasInteres", c => c.String());
            AddColumn("dbo.Usuarios", "OtrosIntereses", c => c.String());
            AddColumn("dbo.Usuarios", "ContenidosFav", c => c.String());
            AddColumn("dbo.Usuarios", "ContenidosDes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "ContenidosDes");
            DropColumn("dbo.Usuarios", "ContenidosFav");
            DropColumn("dbo.Usuarios", "OtrosIntereses");
            DropColumn("dbo.Usuarios", "MateriasInteres");
            DropColumn("dbo.Usuarios", "FechaNacimiento");
        }
    }
}
