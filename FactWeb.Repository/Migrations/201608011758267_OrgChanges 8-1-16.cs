namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgChanges8116 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Organization", name: "BAAExecutionDate", newName: "OrganizationBAAExecutionDate");
            RenameColumn(table: "dbo.Organization", name: "BAADocumentVersion", newName: "OrganizationBAADocumentVersion");
            RenameColumn(table: "dbo.Organization", name: "BAAVerifiedDate", newName: "OrganizationBAAVerifiedDate");
            RenameColumn(table: "dbo.Organization", name: "AccreditationDate", newName: "OrganizationAccreditationDate");
            RenameColumn(table: "dbo.Organization", name: "AccreditationExpirationDate", newName: "OrganizationAccreditationExpirationDate");
            RenameColumn(table: "dbo.Organization", name: "AccreditationExtensionDate", newName: "OrganizationAccreditationExtensionDate");
            RenameColumn(table: "dbo.Organization", name: "Comments", newName: "OrganizationComments");
            AddColumn("dbo.Organization", "OrganizationDescription", c => c.String());
            AddColumn("dbo.Organization", "OrganizationSpatialRelationship", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "OrganizationSpatialRelationship");
            DropColumn("dbo.Organization", "OrganizationDescription");
            RenameColumn(table: "dbo.Organization", name: "OrganizationComments", newName: "Comments");
            RenameColumn(table: "dbo.Organization", name: "OrganizationAccreditationExtensionDate", newName: "AccreditationExtensionDate");
            RenameColumn(table: "dbo.Organization", name: "OrganizationAccreditationExpirationDate", newName: "AccreditationExpirationDate");
            RenameColumn(table: "dbo.Organization", name: "OrganizationAccreditationDate", newName: "AccreditationDate");
            RenameColumn(table: "dbo.Organization", name: "OrganizationBAAVerifiedDate", newName: "BAAVerifiedDate");
            RenameColumn(table: "dbo.Organization", name: "OrganizationBAADocumentVersion", newName: "BAADocumentVersion");
            RenameColumn(table: "dbo.Organization", name: "OrganizationBAAExecutionDate", newName: "BAAExecutionDate");
        }
    }
}
