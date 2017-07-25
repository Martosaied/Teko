namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepairFKUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contenidos", "Usuarios_Id", "dbo.Usuarios");
            DropIndex("dbo.Contenidos", new[] { "Usuarios_Id" });
            RenameColumn(table: "dbo.Contenidos", name: "Usuarios_Id", newName: "UsuariosId");
            AlterColumn("dbo.Contenidos", "UsuariosId", c => c.Int(nullable: true));
            CreateIndex("dbo.Contenidos", "UsuariosId");
            AddForeignKey("dbo.Contenidos", "UsuariosId", "dbo.Usuarios", "Id", cascadeDelete: true);
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
