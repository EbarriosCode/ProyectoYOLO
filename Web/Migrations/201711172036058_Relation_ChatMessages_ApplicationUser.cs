namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relation_ChatMessages_ApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chat", "FechaCreacion", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Chat", "UsuarioEnvia", c => c.String(maxLength: 128));
            AlterColumn("dbo.Chat", "UsuarioRecibe", c => c.String(maxLength: 128));
            AlterColumn("dbo.Chat", "Mensaje", c => c.String(nullable: false));
            CreateIndex("dbo.Chat", "UsuarioEnvia");
            CreateIndex("dbo.Chat", "UsuarioRecibe");
            AddForeignKey("dbo.Chat", "UsuarioEnvia", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Chat", "UsuarioRecibe", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chat", "UsuarioRecibe", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chat", "UsuarioEnvia", "dbo.AspNetUsers");
            DropIndex("dbo.Chat", new[] { "UsuarioRecibe" });
            DropIndex("dbo.Chat", new[] { "UsuarioEnvia" });
            AlterColumn("dbo.Chat", "Mensaje", c => c.String());
            AlterColumn("dbo.Chat", "UsuarioRecibe", c => c.String());
            AlterColumn("dbo.Chat", "UsuarioEnvia", c => c.String());
            DropColumn("dbo.Chat", "FechaCreacion");
        }
    }
}
