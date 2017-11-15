namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Distancia_Duracion_ModelViaje : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Viaje", "Distancia", c => c.String());
            AddColumn("dbo.Viaje", "Duracion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Viaje", "Duracion");
            DropColumn("dbo.Viaje", "Distancia");
        }
    }
}
