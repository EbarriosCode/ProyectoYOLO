namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Temp_UserNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chat", "NombreUsuarioEnvia", c => c.String());
            AddColumn("dbo.Chat", "NombreUsuarioRecibe", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chat", "NombreUsuarioRecibe");
            DropColumn("dbo.Chat", "NombreUsuarioEnvia");
        }
    }
}
