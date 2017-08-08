namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeInIntereses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InteresesEscuelas", "LastUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.InteresesMaterias", "LastUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.InteresesProfesores", "LastUpdate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InteresesProfesores", "LastUpdate");
            DropColumn("dbo.InteresesMaterias", "LastUpdate");
            DropColumn("dbo.InteresesEscuelas", "LastUpdate");
        }
    }
}
