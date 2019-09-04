namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21620161 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Distance",
                c => new
                    {
                        DistanceId = c.Int(nullable: false, identity: true),
                        AddressId = c.Int(nullable: false),
                        DistanceLongitude = c.String(),
                        DistanceLatitude = c.String(),
                        DistanceInMiles = c.Double(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DistanceId)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Distance", "AddressId", "dbo.Address");
            DropIndex("dbo.Distance", new[] { "AddressId" });
            DropTable("dbo.Distance");
        }
    }
}
