namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgChanges102016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationNetcordMembership",
                c => new
                    {
                        OrganizationNetcordMembershipId = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        NetcordMembershipId = c.Guid(nullable: false),
                        OrganizationNetcordMembershipDate = c.DateTime(nullable: false),
                        OrganizationNetcordMembershipIsCurrent = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                        NetcordMembershipType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.OrganizationNetcordMembershipId)
                .ForeignKey("dbo.NetcordMembershipType", t => t.NetcordMembershipType_Id)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.NetcordMembershipType_Id);
            
            CreateTable(
                "dbo.NetcordMembershipType",
                c => new
                    {
                        NetcordMembershipTypeId = c.Guid(nullable: false),
                        NetcordMembershipTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NetcordMembershipTypeId);
            
            AddColumn("dbo.Organization", "OrganizationBaaDocumentPath", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationNetcordMembership", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.OrganizationNetcordMembership", "NetcordMembershipType_Id", "dbo.NetcordMembershipType");
            DropIndex("dbo.OrganizationNetcordMembership", new[] { "NetcordMembershipType_Id" });
            DropIndex("dbo.OrganizationNetcordMembership", new[] { "OrganizationId" });
            DropColumn("dbo.Organization", "OrganizationBaaDocumentPath");
            DropTable("dbo.NetcordMembershipType");
            DropTable("dbo.OrganizationNetcordMembership");
        }
    }
}
