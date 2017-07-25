namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUserNametblUser : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Usuarios", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "UserName", c => c.String());
        }
    }
}
