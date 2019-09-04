namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04052016_Organization_Changes_368 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "OrganizationId", "dbo.Organization");
            CreateTable(
                "dbo.AccreditationStatus",
                c => new
                    {
                        AccreditationStatusId = c.Int(nullable: false, identity: true),
                        AccreditationStatusName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccreditationStatusId);
            
            CreateTable(
                "dbo.BAAOwner",
                c => new
                    {
                        BAAOwnerId = c.Int(nullable: false, identity: true),
                        BAAOwnerName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BAAOwnerId);
            
            AddColumn("dbo.Organization", "OrganizationDirectorId", c => c.Guid());
            AddColumn("dbo.Organization", "AccreditationStatusId", c => c.Int());
            AddColumn("dbo.Organization", "BAAOwnerId", c => c.Int());
            AddColumn("dbo.Organization", "BAAExecutionDate", c => c.DateTime());
            AddColumn("dbo.Organization", "BAADocumentVersion", c => c.String());
            AddColumn("dbo.Organization", "BAAVerifiedDate", c => c.DateTime());
            AddColumn("dbo.Organization", "AccreditationDate", c => c.DateTime());
            AddColumn("dbo.Organization", "AccreditationExpirationDate", c => c.DateTime());
            AddColumn("dbo.Organization", "AccreditationExtensionDate", c => c.DateTime());
            AddColumn("dbo.Organization", "Comments", c => c.String());
            AddColumn("dbo.User", "Organization_Id", c => c.Int());
            CreateIndex("dbo.Organization", "OrganizationDirectorId");
            CreateIndex("dbo.Organization", "AccreditationStatusId");
            CreateIndex("dbo.Organization", "BAAOwnerId");
            CreateIndex("dbo.User", "Organization_Id");
            AddForeignKey("dbo.Organization", "AccreditationStatusId", "dbo.AccreditationStatus", "AccreditationStatusId");
            AddForeignKey("dbo.Organization", "BAAOwnerId", "dbo.BAAOwner", "BAAOwnerId");
            AddForeignKey("dbo.Organization", "OrganizationDirectorId", "dbo.User", "UserId");
            AddForeignKey("dbo.User", "Organization_Id", "dbo.Organization", "OrganizationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "Organization_Id", "dbo.Organization");
            DropForeignKey("dbo.Organization", "OrganizationDirectorId", "dbo.User");
            DropForeignKey("dbo.Organization", "BAAOwnerId", "dbo.BAAOwner");
            DropForeignKey("dbo.Organization", "AccreditationStatusId", "dbo.AccreditationStatus");
            DropIndex("dbo.User", new[] { "Organization_Id" });
            DropIndex("dbo.Organization", new[] { "BAAOwnerId" });
            DropIndex("dbo.Organization", new[] { "AccreditationStatusId" });
            DropIndex("dbo.Organization", new[] { "OrganizationDirectorId" });
            DropColumn("dbo.User", "Organization_Id");
            DropColumn("dbo.Organization", "Comments");
            DropColumn("dbo.Organization", "AccreditationExtensionDate");
            DropColumn("dbo.Organization", "AccreditationExpirationDate");
            DropColumn("dbo.Organization", "AccreditationDate");
            DropColumn("dbo.Organization", "BAAVerifiedDate");
            DropColumn("dbo.Organization", "BAADocumentVersion");
            DropColumn("dbo.Organization", "BAAExecutionDate");
            DropColumn("dbo.Organization", "BAAOwnerId");
            DropColumn("dbo.Organization", "AccreditationStatusId");
            DropColumn("dbo.Organization", "OrganizationDirectorId");
            DropTable("dbo.BAAOwner");
            DropTable("dbo.AccreditationStatus");
            AddForeignKey("dbo.User", "OrganizationId", "dbo.Organization", "OrganizationId");
        }
    }
}
