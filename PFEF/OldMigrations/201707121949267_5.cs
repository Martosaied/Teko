namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "InteresesMaterias", c => c.String());
            AddColumn("dbo.Usuarios", "InteresesProfesores", c => c.String());
            AddColumn("dbo.Usuarios", "InteresesEscuelas", c => c.String());
            DropColumn("dbo.Usuarios", "Intereses");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Intereses", c => c.String());
            DropColumn("dbo.Usuarios", "InteresesEscuelas");
            DropColumn("dbo.Usuarios", "InteresesProfesores");
            DropColumn("dbo.Usuarios", "InteresesMaterias");
        }
    }
}
