namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgDocumentLibrary112216 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "OrganizationDocumentLibraryUserId", c => c.String());
            AddColumn("dbo.Organization", "OrganizationDocumentLibraryVaultId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "OrganizationDocumentLibraryVaultId");
            DropColumn("dbo.Organization", "OrganizationDocumentLibraryUserId");
        }
    }
}
