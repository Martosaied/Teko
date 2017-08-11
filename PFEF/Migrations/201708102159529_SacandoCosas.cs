namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SacandoCosas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "PerfilCompleto", c => c.Boolean(nullable: false));
            DropColumn("dbo.Usuarios", "InteresesMaterias");
            DropColumn("dbo.Usuarios", "InteresesProfesores");
            DropColumn("dbo.Usuarios", "InteresesEscuelas");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "InteresesEscuelas", c => c.String());
            AddColumn("dbo.Usuarios", "InteresesProfesores", c => c.String());
            AddColumn("dbo.Usuarios", "InteresesMaterias", c => c.String());
            DropColumn("dbo.Usuarios", "PerfilCompleto");
        }
    }
}
