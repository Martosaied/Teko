namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RutaOut : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contenidos", "Ruta");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contenidos", "Ruta", c => c.String());
        }
    }
}
