namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Match : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Organization", name: "OrganizationAccreditationExpirationDate", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Organization", name: "OrganizationAccreditedSinceDate", newName: "OrganizationAccreditationExpirationDate");
            RenameColumn(table: "dbo.Organization", name: "__mig_tmp__0", newName: "OrganizationAccreditedSinceDate");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Organization", name: "OrganizationAccreditedSinceDate", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Organization", name: "OrganizationAccreditationExpirationDate", newName: "OrganizationAccreditedSinceDate");
            RenameColumn(table: "dbo.Organization", name: "__mig_tmp__0", newName: "OrganizationAccreditationExpirationDate");
        }
    }
}
