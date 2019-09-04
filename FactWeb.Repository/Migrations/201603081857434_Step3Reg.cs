namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Step3Reg : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserJobFunction", name: "Id", newName: "UserJobFunctionId");
            CreateTable(
                "dbo.UserLanguage",
                c => new
                    {
                        UserLanguageId = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserLanguageId)
                .ForeignKey("dbo.Language", t => t.LanguageId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        LanguageId = c.Guid(nullable: false),
                        LanguageName = c.String(),
                        LanguageOrder = c.Int(nullable: false),
                        LanguageIsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            CreateTable(
                "dbo.UserMembership",
                c => new
                    {
                        UserMembershipId = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        MembershipId = c.Guid(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserMembershipId)
                .ForeignKey("dbo.Membership", t => t.MembershipId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MembershipId);
            
            CreateTable(
                "dbo.Membership",
                c => new
                    {
                        MembershipId = c.Guid(nullable: false),
                        MembershipName = c.String(),
                        MembershipOrder = c.Int(nullable: false),
                        MembershipIsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MembershipId);
            
            AddColumn("dbo.JobFunction", "JobFunctionIsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMembership", "UserId", "dbo.User");
            DropForeignKey("dbo.UserMembership", "MembershipId", "dbo.Membership");
            DropForeignKey("dbo.UserLanguage", "UserId", "dbo.User");
            DropForeignKey("dbo.UserLanguage", "LanguageId", "dbo.Language");
            DropIndex("dbo.UserMembership", new[] { "MembershipId" });
            DropIndex("dbo.UserMembership", new[] { "UserId" });
            DropIndex("dbo.UserLanguage", new[] { "LanguageId" });
            DropIndex("dbo.UserLanguage", new[] { "UserId" });
            DropColumn("dbo.JobFunction", "JobFunctionIsActive");
            DropTable("dbo.Membership");
            DropTable("dbo.UserMembership");
            DropTable("dbo.Language");
            DropTable("dbo.UserLanguage");
            RenameColumn(table: "dbo.UserJobFunction", name: "UserJobFunctionId", newName: "Id");
        }
    }
}
