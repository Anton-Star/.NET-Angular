namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrueVaultGroups : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Address", new[] { "AddressCountry" });
            AddColumn("dbo.Organization", "OrganizationDocumentLibraryGroupId", c => c.String());
            AlterColumn("dbo.Address", "AddressCountry", c => c.Int());
            CreateIndex("dbo.Address", "AddressCountry");
            DropColumn("dbo.Organization", "OrganizationDocumentLibraryUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organization", "OrganizationDocumentLibraryUserId", c => c.String());
            DropIndex("dbo.Address", new[] { "AddressCountry" });
            AlterColumn("dbo.Address", "AddressCountry", c => c.Int(nullable: false));
            DropColumn("dbo.Organization", "OrganizationDocumentLibraryGroupId");
            CreateIndex("dbo.Address", "AddressCountry");
        }
    }
}
