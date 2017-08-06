namespace TaxiManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RideRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TMRideRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                        FinishTime = c.DateTime(),
                        StartLocation = c.String(),
                        FinalDestination = c.String(),
                        Status = c.Int(nullable: false),
                        UserGUID = c.String(),
                        Driver_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Driver_Id)
                .Index(t => t.Driver_Id);
            
            AddColumn("dbo.AspNetUsers", "PersonalName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TMRideRequests", "Driver_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TMRideRequests", new[] { "Driver_Id" });
            DropColumn("dbo.AspNetUsers", "PersonalName");
            DropTable("dbo.TMRideRequests");
        }
    }
}
