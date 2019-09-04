namespace FactWeb.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccreditationRole",
                c => new
                    {
                        AccreditationRoleId = c.Int(nullable: false, identity: true),
                        AccreditationRoleName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccreditationRoleId);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressTypeId = c.Int(nullable: false),
                        AddressStreet1 = c.String(),
                        AddressStreet2 = c.String(),
                        AddressCity = c.String(),
                        AddressCounty = c.String(),
                        AddressState = c.String(),
                        AddressZipCode = c.String(),
                        AddressCountry = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.AddressType", t => t.AddressTypeId)
                .Index(t => t.AddressTypeId);
            
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        AddressTypeId = c.Int(nullable: false, identity: true),
                        AddressTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AddressTypeId);
            
            CreateTable(
                "dbo.OrganizationAddress",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.OrganizationId, t.AddressId })
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false, identity: true),
                        PrimaryUserId = c.Int(),
                        OrganizationNumber = c.String(),
                        OrganizationName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationId);
            
            CreateTable(
                "dbo.OrganizationFacility",
                c => new
                    {
                        OrganizationFacilityId = c.Int(nullable: false, identity: true),
                        OrganizationId = c.Int(nullable: false),
                        FacilityId = c.Int(nullable: false),
                        StrongRelation = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrganizationFacilityId)
                .ForeignKey("dbo.Facility", t => t.FacilityId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.Facility",
                c => new
                    {
                        FacilityId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ServiceTypeId = c.Int(nullable: false),

                    FacilityNumber = c.Guid(nullable: true),
                    CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FacilityId)
                .ForeignKey("dbo.ServiceType", t => t.ServiceTypeId)
                .Index(t => t.ServiceTypeId);
            
            CreateTable(
                "dbo.FacilityAddress",
                c => new
                    {
                        FacilityId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.FacilityId, t.AddressId })
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.Facility", t => t.FacilityId)
                .Index(t => t.FacilityId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.ServiceType",
                c => new
                    {
                        ServiceTypeId = c.Int(nullable: false, identity: true),
                        ServiceTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ServiceTypeId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        OrganizationId = c.Int(),
                        RoleId = c.Int(nullable: false),
                        UserFirstName = c.String(nullable: false, maxLength: 100),
                        UserLastName = c.String(nullable: false, maxLength: 100),
                        UserEmailAddress = c.String(nullable: false, maxLength: 100),
                        UserPreferredPhoneNumber = c.String(maxLength: 20),
                        UserWorkPhoneNumber = c.String(maxLength: 20),
                        UserPassword = c.String(nullable: false, maxLength: 200),
                        UserIsActive = c.Boolean(nullable: false),
                        UserLastLoginDate = c.DateTime(),
                        UserPasswordChangeDate = c.DateTime(nullable: false),
                        UserPasswordResetToken = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.OrganizationId)
                .Index(t => t.RoleId)
                .Index(t => t.UserEmailAddress, unique: true, name: "ix_user_emailAddress");
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserAddress",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        AddressId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.UserId, t.AddressId })
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        ApplicationTypeId = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationId)
                .ForeignKey("dbo.ApplicationType", t => t.ApplicationTypeId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId)
                .Index(t => t.ApplicationTypeId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.ApplicationType",
                c => new
                    {
                        ApplicationTypeId = c.Int(nullable: false, identity: true),
                        ApplicationTypeName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationTypeId);
            
            CreateTable(
                "dbo.ApplicationTypeCategory",
                c => new
                    {
                        ApplicationTypeCategoryId = c.Int(nullable: false, identity: true),
                        ApplicationTypeId = c.Int(nullable: false),
                        InspectionCategoryId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationTypeCategoryId)
                .ForeignKey("dbo.ApplicationType", t => t.ApplicationTypeId)
                .ForeignKey("dbo.InspectionCategory", t => t.InspectionCategoryId)
                .Index(t => t.ApplicationTypeId)
                .Index(t => t.InspectionCategoryId);
            
            CreateTable(
                "dbo.InspectionCategory",
                c => new
                    {
                        InspectionCategoryId = c.Int(nullable: false, identity: true),
                        InspectionCategoryName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InspectionCategoryId);
            
            CreateTable(
                "dbo.InspectionScheduleDetail",
                c => new
                    {
                        InspectionScheduleDetailId = c.Int(nullable: false, identity: true),
                        InspectionScheduleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        AccreditatioRoleId = c.Int(nullable: false),
                        InspectionCategoryId = c.Int(nullable: false),
                        IsLead = c.Boolean(nullable: false),
                        IsMentor = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                        AccreditationRole_Id = c.Int(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.InspectionScheduleDetailId)
                .ForeignKey("dbo.AccreditationRole", t => t.AccreditationRole_Id)
                .ForeignKey("dbo.InspectionCategory", t => t.InspectionCategoryId)
                .ForeignKey("dbo.InspectionSchedule", t => t.InspectionScheduleId)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.InspectionScheduleId)
                .Index(t => t.InspectionCategoryId)
                .Index(t => t.AccreditationRole_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.InspectionSchedule",
                c => new
                    {
                        InspectionScheduleId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        OrganziationId = c.Int(nullable: false),
                        InspectionDate = c.DateTime(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                        CompletionDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InspectionScheduleId);
            
            CreateTable(
                "dbo.InspectionScheduleFacility",
                c => new
                    {
                        InspectionScheduleFacilityId = c.Int(nullable: false, identity: true),
                        FacilityID = c.Int(nullable: false),
                        InspectionScheduleId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InspectionScheduleFacilityId)
                .ForeignKey("dbo.Facility", t => t.FacilityID)
                .ForeignKey("dbo.InspectionSchedule", t => t.InspectionScheduleId)
                .Index(t => t.FacilityID)
                .Index(t => t.InspectionScheduleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InspectionScheduleFacility", "InspectionScheduleId", "dbo.InspectionSchedule");
            DropForeignKey("dbo.InspectionScheduleFacility", "FacilityID", "dbo.Facility");
            DropForeignKey("dbo.InspectionScheduleDetail", "User_Id", "dbo.User");
            DropForeignKey("dbo.InspectionScheduleDetail", "InspectionScheduleId", "dbo.InspectionSchedule");
            DropForeignKey("dbo.InspectionScheduleDetail", "InspectionCategoryId", "dbo.InspectionCategory");
            DropForeignKey("dbo.InspectionScheduleDetail", "AccreditationRole_Id", "dbo.AccreditationRole");
            DropForeignKey("dbo.ApplicationTypeCategory", "InspectionCategoryId", "dbo.InspectionCategory");
            DropForeignKey("dbo.ApplicationTypeCategory", "ApplicationTypeId", "dbo.ApplicationType");
            DropForeignKey("dbo.Application", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Application", "ApplicationTypeId", "dbo.ApplicationType");
            DropForeignKey("dbo.UserAddress", "UserId", "dbo.User");
            DropForeignKey("dbo.UserAddress", "AddressId", "dbo.Address");
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.User", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.OrganizationFacility", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Facility", "ServiceTypeId", "dbo.ServiceType");
            DropForeignKey("dbo.OrganizationFacility", "FacilityId", "dbo.Facility");
            DropForeignKey("dbo.FacilityAddress", "FacilityId", "dbo.Facility");
            DropForeignKey("dbo.FacilityAddress", "AddressId", "dbo.Address");
            DropForeignKey("dbo.OrganizationAddress", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.OrganizationAddress", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Address", "AddressTypeId", "dbo.AddressType");
            DropIndex("dbo.InspectionScheduleFacility", new[] { "InspectionScheduleId" });
            DropIndex("dbo.InspectionScheduleFacility", new[] { "FacilityID" });
            DropIndex("dbo.InspectionScheduleDetail", new[] { "User_Id" });
            DropIndex("dbo.InspectionScheduleDetail", new[] { "AccreditationRole_Id" });
            DropIndex("dbo.InspectionScheduleDetail", new[] { "InspectionCategoryId" });
            DropIndex("dbo.InspectionScheduleDetail", new[] { "InspectionScheduleId" });
            DropIndex("dbo.ApplicationTypeCategory", new[] { "InspectionCategoryId" });
            DropIndex("dbo.ApplicationTypeCategory", new[] { "ApplicationTypeId" });
            DropIndex("dbo.Application", new[] { "OrganizationId" });
            DropIndex("dbo.Application", new[] { "ApplicationTypeId" });
            DropIndex("dbo.UserAddress", new[] { "AddressId" });
            DropIndex("dbo.UserAddress", new[] { "UserId" });
            DropIndex("dbo.User", "ix_user_emailAddress");
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.User", new[] { "OrganizationId" });
            DropIndex("dbo.FacilityAddress", new[] { "AddressId" });
            DropIndex("dbo.FacilityAddress", new[] { "FacilityId" });
            DropIndex("dbo.Facility", new[] { "ServiceTypeId" });
            DropIndex("dbo.OrganizationFacility", new[] { "FacilityId" });
            DropIndex("dbo.OrganizationFacility", new[] { "OrganizationId" });
            DropIndex("dbo.OrganizationAddress", new[] { "AddressId" });
            DropIndex("dbo.OrganizationAddress", new[] { "OrganizationId" });
            DropIndex("dbo.Address", new[] { "AddressTypeId" });
            DropTable("dbo.InspectionScheduleFacility");
            DropTable("dbo.InspectionSchedule");
            DropTable("dbo.InspectionScheduleDetail");
            DropTable("dbo.InspectionCategory");
            DropTable("dbo.ApplicationTypeCategory");
            DropTable("dbo.ApplicationType");
            DropTable("dbo.Application");
            DropTable("dbo.UserAddress");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.ServiceType");
            DropTable("dbo.FacilityAddress");
            DropTable("dbo.Facility");
            DropTable("dbo.OrganizationFacility");
            DropTable("dbo.Organization");
            DropTable("dbo.OrganizationAddress");
            DropTable("dbo.AddressType");
            DropTable("dbo.Address");
            DropTable("dbo.AccreditationRole");
        }
    }
}
