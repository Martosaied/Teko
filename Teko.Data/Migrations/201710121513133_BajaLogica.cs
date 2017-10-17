namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BajaLogica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contenidos", "BajaLogica", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contenidos", "BajaLogica");
        }
    }
}
