namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contenidos", "UsuariosId", "dbo.Usuarios");
            DropIndex("dbo.Contenidos", new[] { "UsuariosId" });
            DropColumn("dbo.Contenidos", "UsuariosId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contenidos", "Usuarios_Id", "dbo.Usuarios");
            DropIndex("dbo.Contenidos", new[] { "Usuarios_Id" });
            AlterColumn("dbo.Contenidos", "Usuarios_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Contenidos", name: "Usuarios_Id", newName: "UsuariosId");
            AddForeignKey("dbo.Contenidos", "UsuariosId", "dbo.Usuarios", "Id", cascadeDelete: true);
        }
    }
}
