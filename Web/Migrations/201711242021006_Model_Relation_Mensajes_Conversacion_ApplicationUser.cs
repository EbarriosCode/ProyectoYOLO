namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model_Relation_Mensajes_Conversacion_ApplicationUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chat", "UsuarioEnvia", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chat", "UsuarioRecibe", "dbo.AspNetUsers");
            DropIndex("dbo.Chat", new[] { "UsuarioEnvia" });
            DropIndex("dbo.Chat", new[] { "UsuarioRecibe" });
            CreateTable(
                "dbo.Conversacion",
                c => new
                    {
                        ConversacionId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        MensajeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConversacionId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Mensaje", t => t.MensajeId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.MensajeId);
            
            CreateTable(
                "dbo.Mensaje",
                c => new
                    {
                        MensajeId = c.Int(nullable: false, identity: true),
                        UsuarioEnvia = c.String(maxLength: 128),
                        UsuarioRecibe = c.String(maxLength: 128),
                        Mensaje = c.String(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MensajeId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioEnvia)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioRecibe)
                .Index(t => t.UsuarioEnvia)
                .Index(t => t.UsuarioRecibe);
            
            DropTable("dbo.Chat");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Chat",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        UsuarioEnvia = c.String(maxLength: 128),
                        UsuarioRecibe = c.String(maxLength: 128),
                        NombreUsuarioEnvia = c.String(),
                        NombreUsuarioRecibe = c.String(),
                        Mensaje = c.String(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ChatId);
            
            DropForeignKey("dbo.Conversacion", "MensajeId", "dbo.Mensaje");
            DropForeignKey("dbo.Mensaje", "UsuarioRecibe", "dbo.AspNetUsers");
            DropForeignKey("dbo.Mensaje", "UsuarioEnvia", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversacion", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Mensaje", new[] { "UsuarioRecibe" });
            DropIndex("dbo.Mensaje", new[] { "UsuarioEnvia" });
            DropIndex("dbo.Conversacion", new[] { "MensajeId" });
            DropIndex("dbo.Conversacion", new[] { "ApplicationUserId" });
            DropTable("dbo.Mensaje");
            DropTable("dbo.Conversacion");
            CreateIndex("dbo.Chat", "UsuarioRecibe");
            CreateIndex("dbo.Chat", "UsuarioEnvia");
            AddForeignKey("dbo.Chat", "UsuarioRecibe", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Chat", "UsuarioEnvia", "dbo.AspNetUsers", "Id");
        }
    }
}
