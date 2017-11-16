namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reserva_RelationManyToMany_ViajeReserva : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reserva",
                c => new
                    {
                        ReservaId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        NumAsientos = c.Int(nullable: false),
                        Aceptado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReservaId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.ViajeReserva",
                c => new
                    {
                        ViajeReservaId = c.Int(nullable: false, identity: true),
                        ViajeId = c.Int(nullable: false),
                        ReservaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ViajeReservaId)
                .ForeignKey("dbo.Reserva", t => t.ReservaId, cascadeDelete: true)
                .ForeignKey("dbo.Viaje", t => t.ViajeId, cascadeDelete: true)
                .Index(t => t.ViajeId)
                .Index(t => t.ReservaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ViajeReserva", "ViajeId", "dbo.Viaje");
            DropForeignKey("dbo.ViajeReserva", "ReservaId", "dbo.Reserva");
            DropForeignKey("dbo.Reserva", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ViajeReserva", new[] { "ReservaId" });
            DropIndex("dbo.ViajeReserva", new[] { "ViajeId" });
            DropIndex("dbo.Reserva", new[] { "ApplicationUserId" });
            DropTable("dbo.ViajeReserva");
            DropTable("dbo.Reserva");
        }
    }
}
