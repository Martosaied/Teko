namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsNotificable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comentarios", "IsNotificable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comentarios", "IsNotificable");
        }
    }
}
