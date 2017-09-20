namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Badget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contenidos", "Badget", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contenidos", "Badget");
        }
    }
}
