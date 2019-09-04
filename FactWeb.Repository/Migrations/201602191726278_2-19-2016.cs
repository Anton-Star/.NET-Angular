namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2192016 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Distance", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Distance", "UserId", "dbo.User");
            DropIndex("dbo.Distance", new[] { "UserId" });
            DropIndex("dbo.Distance", new[] { "AddressId" });
            DropPrimaryKey("dbo.FacilityAddress");
            DropPrimaryKey("dbo.UserAddress");
            AddColumn("dbo.Address", "AddressLogitude", c => c.String());
            AddColumn("dbo.Address", "AddressLatitude", c => c.String());
            AddColumn("dbo.FacilityAddress", "FacilityAddressId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.UserAddress", "UserAddressId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Distance", "FacilityAddressId", c => c.Int(nullable: false));
            AddColumn("dbo.Distance", "UserAddressId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.FacilityAddress", "FacilityAddressId");
            AddPrimaryKey("dbo.UserAddress", "UserAddressId");
            CreateIndex("dbo.Distance", "FacilityAddressId");
            CreateIndex("dbo.Distance", "UserAddressId");
            AddForeignKey("dbo.Distance", "FacilityAddressId", "dbo.FacilityAddress", "FacilityAddressId");
            AddForeignKey("dbo.Distance", "UserAddressId", "dbo.UserAddress", "UserAddressId");
            DropColumn("dbo.Distance", "UserId");
            DropColumn("dbo.Distance", "AddressId");
            DropColumn("dbo.Distance", "DistanceLongitude");
            DropColumn("dbo.Distance", "DistanceLatitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Distance", "DistanceLatitude", c => c.String());
            AddColumn("dbo.Distance", "DistanceLongitude", c => c.String());
            AddColumn("dbo.Distance", "AddressId", c => c.Int(nullable: false));
            AddColumn("dbo.Distance", "UserId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Distance", "UserAddressId", "dbo.UserAddress");
            DropForeignKey("dbo.Distance", "FacilityAddressId", "dbo.FacilityAddress");
            DropIndex("dbo.Distance", new[] { "UserAddressId" });
            DropIndex("dbo.Distance", new[] { "FacilityAddressId" });
            DropPrimaryKey("dbo.UserAddress");
            DropPrimaryKey("dbo.FacilityAddress");
            DropColumn("dbo.Distance", "UserAddressId");
            DropColumn("dbo.Distance", "FacilityAddressId");
            DropColumn("dbo.UserAddress", "UserAddressId");
            DropColumn("dbo.FacilityAddress", "FacilityAddressId");
            DropColumn("dbo.Address", "AddressLatitude");
            DropColumn("dbo.Address", "AddressLogitude");
            AddPrimaryKey("dbo.UserAddress", new[] { "UserId", "AddressId" });
            AddPrimaryKey("dbo.FacilityAddress", new[] { "FacilityId", "AddressId" });
            CreateIndex("dbo.Distance", "AddressId");
            CreateIndex("dbo.Distance", "UserId");
            AddForeignKey("dbo.Distance", "UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.Distance", "AddressId", "dbo.Address", "AddressId");
        }
    }
}
