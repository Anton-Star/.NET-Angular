namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05112016_UserChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Credential",
                c => new
                    {
                        CredentialId = c.Int(nullable: false, identity: true),
                        CredentialName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CredentialId);
            
            CreateTable(
                "dbo.UserCredential",
                c => new
                    {
                        UserCredentialId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CredentialId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserCredentialId)
                .ForeignKey("dbo.Credential", t => t.CredentialId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CredentialId);
            
            AddColumn("dbo.User", "UserMedicalLicenseExpiry", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCredential", "UserId", "dbo.User");
            DropForeignKey("dbo.UserCredential", "CredentialId", "dbo.Credential");
            DropIndex("dbo.UserCredential", new[] { "CredentialId" });
            DropIndex("dbo.UserCredential", new[] { "UserId" });
            DropColumn("dbo.User", "UserMedicalLicenseExpiry");
            DropTable("dbo.UserCredential");
            DropTable("dbo.Credential");
        }
    }
}
