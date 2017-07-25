namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contenidos", "UsuariosId", c => c.Int(nullable: false));
            CreateIndex("dbo.Contenidos", "UsuariosId");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contenidos", "UsuariosId", "dbo.Usuarios");
            DropIndex("dbo.Contenidos", new[] { "UsuariosId" });
            AlterColumn("dbo.Contenidos", "UsuariosId", c => c.Int());
            RenameColumn(table: "dbo.Contenidos", name: "UsuariosId", newName: "Usuarios_Id");
            CreateIndex("dbo.Contenidos", "Usuarios_Id");
            AddForeignKey("dbo.Contenidos", "Usuarios_Id", "dbo.Usuarios", "Id");
        }
    }
}
