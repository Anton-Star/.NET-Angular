namespace FactWeb.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationResponseStatus",
                c => new
                    {
                        ApplicationResponseStatus = c.Int(nullable: false, identity: true),
                        ApplicationResponseStatusName = c.String(),
                        CreatedBy = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 100),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApplicationResponseStatus);
            
            AddColumn("dbo.ApplicationResponse", "ApplicationResponseStatusId", c => c.Int());
            CreateIndex("dbo.ApplicationResponse", "ApplicationResponseStatusId");
            AddForeignKey("dbo.ApplicationResponse", "ApplicationResponseStatusId", "dbo.ApplicationResponseStatus", "ApplicationResponseStatus");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationResponse", "ApplicationResponseStatusId", "dbo.ApplicationResponseStatus");
            DropIndex("dbo.ApplicationResponse", new[] { "ApplicationResponseStatusId" });
            DropColumn("dbo.ApplicationResponse", "ApplicationResponseStatusId");
            DropTable("dbo.ApplicationResponseStatus");
        }
    }
}
