namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_ChatMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chat",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        UsuarioEnvia = c.String(),
                        UsuarioRecibe = c.String(),
                        Mensaje = c.String(),
                    })
                .PrimaryKey(t => t.ChatId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Chat");
        }
    }
}
