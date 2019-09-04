namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrgDocumentLibrary112316 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "OrganizationDocumentLibraryAccessToken", c => c.String());
            AddColumn("dbo.Organization", "OrganizationDocumentLibraryAccessTokenExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "OrganizationDocumentLibraryAccessTokenExpirationDate");
            DropColumn("dbo.Organization", "OrganizationDocumentLibraryAccessToken");
        }
    }
}
