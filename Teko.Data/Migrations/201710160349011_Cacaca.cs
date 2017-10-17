namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cacaca : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contenidos", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Contenidos", "BajaLogica");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contenidos", "BajaLogica", c => c.Boolean(nullable: false));
            DropColumn("dbo.Contenidos", "IsDeleted");
        }
    }
}
