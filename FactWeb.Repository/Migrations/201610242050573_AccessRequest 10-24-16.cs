namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessRequest102416 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessRequest",
                c => new
                    {
                        RequestAccessId = c.String(nullable: false, maxLength: 128),
                        MasterServiceTypeId = c.Int(),
                        RequestAccessOrganizationName = c.String(),
                        RequestAccessOrganizationAddress = c.String(),
                        RequestAccessDirectorName = c.String(),
                        RequestAccessDirectorEmailAddress = c.String(),
                        RequestAccessPrimaryContactName = c.String(),
                        RequestAccessPrimaryContactEmailAddress = c.String(),
                        RequestAccessPrimaryContactPhoneNumber = c.String(),
                        RequestAccessMasterServiceTypeOtherComment = c.String(),
                        RequestAccessOtherComments = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RequestAccessId)
                .ForeignKey("dbo.MasterServiceType", t => t.MasterServiceTypeId)
                .Index(t => t.MasterServiceTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccessRequest", "MasterServiceTypeId", "dbo.MasterServiceType");
            DropIndex("dbo.AccessRequest", new[] { "MasterServiceTypeId" });
            DropTable("dbo.AccessRequest");
        }
    }
}
