namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02292016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationType",
                c => new
                    {
                        OrganizationTypeId = c.Int(nullable: false, identity: true),
                        OrganizationTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationTypeId);
            
            AddColumn("dbo.Organization", "OrganizationPhoneUS", c => c.String());
            AddColumn("dbo.Organization", "OrganizationPhoneUSExt", c => c.Int(nullable: false));
            AddColumn("dbo.Organization", "OrganizationPhone", c => c.String());
            AddColumn("dbo.Organization", "OrganizationPhoneExt", c => c.Int(nullable: false));
            AddColumn("dbo.Organization", "OrganizationFaxUS", c => c.String());
            AddColumn("dbo.Organization", "OrganizationFaxUSExt", c => c.Int(nullable: false));
            AddColumn("dbo.Organization", "OrganizationFax", c => c.String());
            AddColumn("dbo.Organization", "OrganizationFaxExt", c => c.Int(nullable: false));
            AddColumn("dbo.Organization", "OrganizationEmail", c => c.String());
            AddColumn("dbo.Organization", "OrganizationWebSite", c => c.String());
            AddColumn("dbo.Organization", "OrganizationTypeId", c => c.Int());
            CreateIndex("dbo.Organization", "OrganizationTypeId");
            AddForeignKey("dbo.Organization", "OrganizationTypeId", "dbo.OrganizationType", "OrganizationTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "OrganizationTypeId", "dbo.OrganizationType");
            DropIndex("dbo.Organization", new[] { "OrganizationTypeId" });
            DropColumn("dbo.Organization", "OrganizationTypeId");
            DropColumn("dbo.Organization", "OrganizationWebSite");
            DropColumn("dbo.Organization", "OrganizationEmail");
            DropColumn("dbo.Organization", "OrganizationFaxExt");
            DropColumn("dbo.Organization", "OrganizationFax");
            DropColumn("dbo.Organization", "OrganizationFaxUSExt");
            DropColumn("dbo.Organization", "OrganizationFaxUS");
            DropColumn("dbo.Organization", "OrganizationPhoneExt");
            DropColumn("dbo.Organization", "OrganizationPhone");
            DropColumn("dbo.Organization", "OrganizationPhoneUSExt");
            DropColumn("dbo.Organization", "OrganizationPhoneUS");
            DropTable("dbo.OrganizationType");
        }
    }
}
