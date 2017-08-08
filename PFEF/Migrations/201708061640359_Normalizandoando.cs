namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Normalizandoando : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Contenidos", name: "NivelesEducativosId", newName: "NivelesEducativos_Id");
            RenameIndex(table: "dbo.Contenidos", name: "IX_NivelesEducativosId", newName: "IX_NivelesEducativos_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Contenidos", name: "IX_NivelesEducativos_Id", newName: "IX_NivelesEducativosId");
            RenameColumn(table: "dbo.Contenidos", name: "NivelesEducativos_Id", newName: "NivelesEducativosId");
        }
    }
}
