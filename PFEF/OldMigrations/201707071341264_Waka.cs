namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Waka : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "UserName", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "UserName");
        }
    }
}
