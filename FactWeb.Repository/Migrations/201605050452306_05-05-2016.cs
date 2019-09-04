namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05052016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationResponseComment",
                c => new
                    {
                        ApplicationResponseCommentId = c.Int(nullable: false, identity: true),
                        ApplicationResponseId = c.Int(nullable: false),
                        ApplicationResponseCommentRFIComment = c.String(),
                        ApplicationResponseCommentCitationComment = c.String(),
                        FromUser = c.Guid(),
                        ToUser = c.Guid(),
                        DocumentId = c.Guid(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationResponseCommentId)
                .ForeignKey("dbo.ApplicationResponse", t => t.ApplicationResponseId)
                .ForeignKey("dbo.User", t => t.FromUser)
                .ForeignKey("dbo.User", t => t.ToUser)
                .ForeignKey("dbo.Document", t => t.DocumentId)
                .Index(t => t.ApplicationResponseId)
                .Index(t => t.FromUser)
                .Index(t => t.ToUser)
                .Index(t => t.DocumentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationResponseComment", "DocumentId", "dbo.Document");
            DropForeignKey("dbo.ApplicationResponseComment", "ToUser", "dbo.User");
            DropForeignKey("dbo.ApplicationResponseComment", "FromUser", "dbo.User");
            DropForeignKey("dbo.ApplicationResponseComment", "ApplicationResponseId", "dbo.ApplicationResponse");
            DropIndex("dbo.ApplicationResponseComment", new[] { "DocumentId" });
            DropIndex("dbo.ApplicationResponseComment", new[] { "ToUser" });
            DropIndex("dbo.ApplicationResponseComment", new[] { "FromUser" });
            DropIndex("dbo.ApplicationResponseComment", new[] { "ApplicationResponseId" });
            DropTable("dbo.ApplicationResponseComment");
        }
    }
}
