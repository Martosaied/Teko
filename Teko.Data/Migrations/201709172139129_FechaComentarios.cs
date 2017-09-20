namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaComentarios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comentarios", "FechaPublicacion", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comentarios", "FechaPublicacion");
        }
    }
}
