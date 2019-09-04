namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11242016 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrganizationNetcordMembership", "NetcordMembershipType_Id", "dbo.NetcordMembershipType");
            DropForeignKey("dbo.OrganizationNetcordMembership", "OrganizationId", "dbo.Organization");
            DropIndex("dbo.OrganizationNetcordMembership", new[] { "OrganizationId" });
            DropIndex("dbo.OrganizationNetcordMembership", new[] { "NetcordMembershipType_Id" });
            AddColumn("dbo.Facility", "NetcordMembershipTypeId", c => c.Guid());
            AddColumn("dbo.Facility", "FacilityProvisionalMembershipDate", c => c.DateTime());
            AddColumn("dbo.Facility", "FacilityAssociateMembershipDate", c => c.DateTime());
            AddColumn("dbo.Facility", "FacilityFullMembershipDate", c => c.DateTime());
            CreateIndex("dbo.Facility", "NetcordMembershipTypeId");
            AddForeignKey("dbo.Facility", "NetcordMembershipTypeId", "dbo.NetcordMembershipType", "NetcordMembershipTypeId");
            DropTable("dbo.OrganizationNetcordMembership");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.OrganizationNetcordMembershipId);
            
            DropForeignKey("dbo.Facility", "NetcordMembershipTypeId", "dbo.NetcordMembershipType");
            DropIndex("dbo.Facility", new[] { "NetcordMembershipTypeId" });
            DropColumn("dbo.Facility", "FacilityFullMembershipDate");
            DropColumn("dbo.Facility", "FacilityAssociateMembershipDate");
            DropColumn("dbo.Facility", "FacilityProvisionalMembershipDate");
            DropColumn("dbo.Facility", "NetcordMembershipTypeId");
            CreateIndex("dbo.OrganizationNetcordMembership", "NetcordMembershipType_Id");
            CreateIndex("dbo.OrganizationNetcordMembership", "OrganizationId");
            AddForeignKey("dbo.OrganizationNetcordMembership", "OrganizationId", "dbo.Organization", "OrganizationId");
            AddForeignKey("dbo.OrganizationNetcordMembership", "NetcordMembershipType_Id", "dbo.NetcordMembershipType", "NetcordMembershipTypeId");
        }
    }
}
