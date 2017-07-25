namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdd : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Contenidos", new[] { "Usuarios_Id1" });
            DropColumn("dbo.Contenidos", "Usuarios_Id");
            RenameColumn(table: "dbo.Contenidos", name: "Usuarios_Id1", newName: "Usuarios_Id");
            AlterColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int());
            CreateIndex("dbo.Contenidos", "Usuarios_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Contenidos", new[] { "Usuarios_Id" });
            AlterColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Contenidos", name: "Usuarios_Id", newName: "Usuarios_Id1");
            AddColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Contenidos", "Usuarios_Id1");
        }
    }
}
