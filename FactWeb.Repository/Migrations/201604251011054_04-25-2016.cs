namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04252016 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Distance", "FacilityAddressId", "dbo.FacilityAddress");
            DropIndex("dbo.Distance", new[] { "FacilityAddressId" });
            CreateTable(
                "dbo.SiteAddress",
                c => new
                    {
                        SiteAddressId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteAddressId)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.AddressId);
            
            AddColumn("dbo.Distance", "SiteAddressId", c => c.Int(nullable: false));
            CreateIndex("dbo.Distance", "SiteAddressId");
            AddForeignKey("dbo.Distance", "SiteAddressId", "dbo.SiteAddress", "SiteAddressId");
            DropColumn("dbo.Distance", "FacilityAddressId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Distance", "FacilityAddressId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Distance", "SiteAddressId", "dbo.SiteAddress");
            DropForeignKey("dbo.SiteAddress", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteAddress", "AddressId", "dbo.Address");
            DropIndex("dbo.SiteAddress", new[] { "AddressId" });
            DropIndex("dbo.SiteAddress", new[] { "SiteId" });
            DropIndex("dbo.Distance", new[] { "SiteAddressId" });
            DropColumn("dbo.Distance", "SiteAddressId");
            DropTable("dbo.SiteAddress");
            CreateIndex("dbo.Distance", "FacilityAddressId");
            AddForeignKey("dbo.Distance", "FacilityAddressId", "dbo.FacilityAddress", "FacilityAddressId");
        }
    }
}
