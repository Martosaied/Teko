namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Otra : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Archivoes", newName: "Archivos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Archivos", newName: "Archivoes");
        }
    }
}
